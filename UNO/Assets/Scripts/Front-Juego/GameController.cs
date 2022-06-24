using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameController : MonoBehaviour
{
    public struct Jugador{
        public string nombreJugador;
        public int turno;
        public string avatar; //TODO: esto sera la imagen ya se vera
        public Jugador(string n, int t, string avatar_)
        {
            this.nombreJugador = n;
            this.turno = t;
            this.avatar = avatar_;
        }
    }

    public static string sala = "salita";
    public static List<Jugador> jugadores = new List<Jugador>();   //de momento sin avatar, ya lo meteremos mas adelante
    public static int numJugadores = 0;
    public static int miTurno;
    public static string miAvatarAux;

    public string miAvatar = "0";
    public static string miNombreJugador = "s";
    public string miNombre;
    public string miCorreo;
    
    public GameController instance;
    
    void Awake(){
        if (instance == null){
            instance = this;
            DontDestroyOnLoad(this);
        } else if (instance != this){
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    void Start(){}

    // Update is called once per frame
    void Update(){

    }

    public void eliminarJugadorDeGameController(string nombre)
    {
        int i  = 0;
        foreach (Jugador j in jugadores)
        {
            if(j.nombreJugador == nombre)
            {
                jugadores.RemoveAt(i);
                break;
            }
            i++;
        }
        numJugadores--;
    }

    public void borrarListaJugadores()
    {
        jugadores.Clear();
        numJugadores = 0;
    }

    public void tomaSala(string sala_){
        sala = sala_;
    }

    public void tomaTuAvatar(string avatar_){
        Debug.Log("Entramos? si AVATAR: " + avatar_);
        miAvatar = avatar_;
        //string[] idAvatar = avatar_.Split(' ');
        miAvatarAux = avatar_;
        Debug.Log("Hemos cambiado la variable" + miAvatarAux);
    }

    public static string getMiAvatar(){
        
        return miAvatarAux;
    }

    public void nuevoJugador(string nombre, string avatar, int turno)
    {
        //Troceamos el nombre del avatar para saber su id:
        //string[] idAvatar = avatar.Split(' ');
        Debug.Log("El avatar del jugador nuevo es " + avatar);
        Jugador j = new Jugador(nombre, turno, avatar);
        jugadores.Add(j);
        numJugadores++;
    }

    public void tomaTurnoInical(int miTurno_)
    {
        miTurno = miTurno_;
        numJugadores++;
    }

    public void tomaMiNombre(string miNombre_)
    {
        miNombre = miNombre_;
        miNombreJugador = miNombre_;
        GameSharedUI gc = GameObject.Find("GameSharedUI").GetComponent<GameSharedUI>();
        gc.Start();
    }

    //Seria algo asi y recibirlos de alguna parte como sala, que se envia en UIManager??????
    public void tomaJugadores(List<Jugador> jugadores_, int numJugadores_){ 
        jugadores = jugadores_;
        numJugadores = numJugadores_;
    }

    public void mostrarEnConsola()
    {
        Debug.Log("Estoy en la sala: " + sala);
        Debug.Log("Me toca en el turno " + miTurno);
        foreach (Jugador j in jugadores)
        {
            Debug.Log("Juego con " + j.nombreJugador + " que tiene el turno " + j.turno);
        }
    }
}
