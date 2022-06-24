using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityNpgsql;
using TMPro;

public class controladorSalas : MonoBehaviour
{
    [SerializeField] private InputField crearSalaInput;
    [SerializeField] private GameObject lobbyPanel;
    [SerializeField] private GameObject roomPanel;
    [SerializeField] public TextMeshProUGUI nombreSalaTexto;
    [SerializeField] private GameObject menuPruebas;
    [SerializeField] private GameObject menuSalas;
    [SerializeField] private Text numeroCotillas;

    public struct DatosSala
    {
        public string sala;
        public int jugadoresEnSala;
        public DatosSala(string s, int j)
        {
            this.sala = s;
            this.jugadoresEnSala = j;
        }
    }
    bool estaEnSala;
    public string nombreJugador;
    private string roomName;
    public RoomItem roomItemPrefab;
    public PlayerItem playerItemPrefab; 
    //lista para las salas que hay disponibles
    List<RoomItem> roomItemsList = new List<RoomItem>();
    //The Transform component determines the Position, Rotation, and Scale of each object in the scene. Every GameObject has a Transform.
    public Transform contentObject;
    public List<DatosSala> listaSalas; //Salas disponibles que pueden o no tener personas

    //----------------------------------Objetos jugador-----------------------------------------------------
    public List<PlayerItem> playerItemList = new List<PlayerItem>();
    public Transform playerItemParent;
    private HashSet<string> listaSalasNoVacias;  //Salas con personas ya dentro, para tener jugadores

    public NpgsqlConnection con;
    
    public controladorSalas(){
        listaSalas = new List<DatosSala>();
        listaSalasNoVacias = new HashSet<string>();
    }

    private void Start()
    { 
        estaEnSala = false;
        GameController gc = GameObject.Find("GameControler").GetComponent<GameController>();
        nombreJugador = gc.miNombre;
        InvokeRepeating("obtenerlistaSala", 2.0f, 2.0f);
    }

    public void crearSala()
    {   
        if(crearSalaInput.text.Length >= 1){
            try
            {
                roomName = crearSalaInput.text;
                GameController gc = GameObject.Find("GameControler").GetComponent<GameController>();
                ClientSend.EntraSala(roomName, true, nombreJugador, gc.miAvatar);
                Debug.Log("Sala creada " + roomName);
            }
            catch (Exception ex)
            {
                Debug.Log("Error con la base de datos (crear salas) :(" + ex);
            }
            //Llamada al servidor para crear la sala
            //ClientSend.EntraSala(crearSalaInput.text, true);
        }
    }

    public long obtenerNPersonas(string nombreSala)
    {
        long total = -1; //variable para guardar cuánta gente hay en una sala
        try
        {
            con = dbManager.getDBConnection();
            string consulta0 = "SELECT COUNT(*) FROM entra_sala WHERE (sala_nombre= '"+ nombreSala + "' AND es_cotilla = 'false')";
            using var cmd0 = new NpgsqlCommand(consulta0,con);

            using NpgsqlDataReader rdr0 = cmd0.ExecuteReader();
            
            cmd0.CommandText = consulta0;

            while (rdr0.Read())
            {   
                total = rdr0.GetInt64(0);
            }
            return total;
        }

        catch (Exception ex)
        {
            Debug.Log("Error onteniendo el nº de personas en una sala:(" + ex);
        }

        return total;

    }

    public void estaListo()
    {
        ClientSend.EstoyListo(roomName);
    }

    public void entrarSala(string nombreSala){
        
        //Cargarmelo cuando vea q funcione
        //nombreJugador = datosJugador.GetDatos();
        // hasta aqui
        roomName = nombreSala;
        try
        {
            GameController gc = GameObject.Find("GameControler").GetComponent<GameController>();
            ClientSend.EntraSala(roomName, false, nombreJugador,gc.miAvatar);
            Debug.Log("Jugador ha entrado a la sala " +  nombreSala + " con avatar " + gc.miAvatar);

            //Espero a añadir un jugador a una sala y cambio de escena

            controladorLogro cL = GameObject.Find("controladorLogros").GetComponent<controladorLogro>();
		    cL.entraAunaSala();
        }
        catch (Exception ex)
        {
            Debug.Log("Error con la base de datos (entrar Sala) :(" + ex);
        }
    }

    public void SalaACKRecibido(string msg, int numCotillas)
    {
        if (msg == "OK")
        {
            GameController gc = GameObject.Find("GameControler").GetComponent<GameController>();
            estaEnSala = true;
            updatePlayersList(nombreJugador,gc.miAvatar , true);
            pantalla_entraPersonaSala(roomName, numCotillas);
            AudioSource sonidoSala = GameObject.Find("SonidoEntrarSala").GetComponent<AudioSource>();
            sonidoSala.Play();
            gc.tomaSala(roomName);
        }
        else
        {
            Debug.Log("Error, la sala estaba llena");
        }
    }

    public void obtenerlistaSala(){
        if(estaEnSala == false)
        {
            ClientSend.PedirSalas();
        }
    }

    public void obtenerPersonasSala(string nombreSala){
        try
        {   
            Debug.Log("Obteniendo Personas Sala");
            con = dbManager.getDBConnection();
            string consulta = "SELECT * FROM entra_sala WHERE sala_nombre= '"+ nombreSala + "'";
            using var cmd = new NpgsqlCommand(consulta,con);

            using NpgsqlDataReader rdr = cmd.ExecuteReader();
            
            cmd.CommandText = consulta;
            
            //Leo la base de datos y añado las Personas de una sala a la lista
            while (rdr.Read())
            {   
                listaSalasNoVacias.Add(rdr.GetString(2));
                Debug.Log(rdr.GetString(2));
            }
            con.Close();
        }
        catch (Exception ex)
        {
            Debug.Log("Error con la base de datos obtener las personas en una sala :(" + ex);
        }
        Invoke("updatePlayersList",3);
    }

    public void updateListaSalas()
    {   
        //Primero voy a borrar todas las salas que haya
        foreach(RoomItem item in roomItemsList)
        {
            Destroy(item.gameObject);
        }
        roomItemsList.Clear();

        //Ahora vuelvo a llenar la lista. Tendré que coger toda la info de la bbdd y guardarla en una lista
        foreach(DatosSala datosSala in listaSalas)
        {
            RoomItem newRoom = Instantiate(roomItemPrefab,contentObject);
            newRoom.SetNombreSala(datosSala.sala, datosSala.jugadoresEnSala);
            roomItemsList.Add(newRoom);
        }
    }

    public void updatePlayersList(string nombre, string avatar, bool esElJugador)
    {
        /*foreach (PlayerItem item in playerItemList)
        {
            Destroy(item.gameObject);
        }
        playerItemList.Clear();*/

        PlayerItem newPlayerItem = Instantiate(playerItemPrefab, playerItemParent);
        newPlayerItem.SetPlayerInfo(nombre, avatar);
        if(esElJugador == true)
        {
            newPlayerItem.cambiarColor();
        }
        playerItemList.Add(newPlayerItem);
    }

    public void pantalla_entraPersonaSala(string nombreSala, int numCotillas)
    {
        //yield return new WaitForSeconds(3);
        //print("3 seconds has passed");

         //Pongo a falso el menu lobbies
        lobbyPanel.SetActive(false);
        
        //Entrar a la sala creada
        roomPanel.SetActive(true);
        numeroCotillas.text = "" + numCotillas;
        nombreSalaTexto.text = "Sala: " +  nombreSala;
    }

    public void onClickLeaveRoom()
    {
        GameController gc = GameObject.Find("GameControler").GetComponent<GameController>();
        gc.borrarListaJugadores();
        estaEnSala = false;
        foreach (PlayerItem item in playerItemList)
        {
            Destroy(item.gameObject);
        }
        playerItemList.Clear();
        roomPanel.SetActive(false);
        lobbyPanel.SetActive(true);
        ClientSend.BorrarDeSala(roomName);
    }

    public void ponerJugadorListo(string nombre)
    {
        foreach (PlayerItem item in playerItemList)
        {
            if (item.playerName.text == nombre)
            {
                item.estaListo();
            }
        }
    }

    public void eliminarJugadorDeLista(string nombre)
    {
        foreach (PlayerItem item in playerItemList)
        {
            if(item.playerName.text == nombre)
            {
                Destroy(item.gameObject);
            }
        }
    }

    public void CambioNumCotillas(bool sumar)
    {
        int n = Int32.Parse(numeroCotillas.text);
        Debug.Log("Anterior numero de cotillas " + n);
        if (sumar) { n++; }
        else { n--; }
        numeroCotillas.text = "" + n;
        Debug.Log("Nuevo numero de cotillas " + numeroCotillas.text);
    }
}
