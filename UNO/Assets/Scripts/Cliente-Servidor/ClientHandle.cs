using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cards;
using Backend;
using UnityEngine.SceneManagement;
using System;

public class ClientHandle : MonoBehaviour
{
    public static void Welcome(Packet _packet)
    {
        string _msg = _packet.ReadString();
        int _myId = _packet.ReadInt();

        Debug.Log($"Message from server: {_msg}");
        Client.instance.myId = _myId;
        ClientSend.WelcomeReceived();
    }

    public static void JugadoresRecibidos(Packet _packet)
    {
        string nombre1 = _packet.ReadString();
        int orden1 = _packet.ReadInt();
    }

    public static void CartaRecibida(Packet _packet) //Recibe carta de un jugador contrario del server
    { 
        PlayerDeck player = GameObject.Find("PlayerDeck").GetComponent<PlayerDeck>();
        int idCarta = _packet.ReadInt();
        string nombre = _packet.ReadString();
        //Información extra como por ejemplo el color al que cambia o las cartas acumuladas
        //(si se echa un +2 se acumulan 2, si luego se echa otro +2 sobre este se acumulan 4, etc)
        string infoExtra = _packet.ReadString(); 

        player.jugadaContraria(nombre, idCarta,infoExtra);
    }

    public static void CartasRobadas(Packet _packet) //Recibe carta que ha pedido robar del server
    {
        //string nombre = _packet.ReadString();
        int numCartas = _packet.ReadInt();
        PlayerDeck player = GameObject.Find("PlayerDeck").GetComponent<PlayerDeck>();

        for(int i = 0; i < numCartas; i++)
        {   
            int idCarta = _packet.ReadInt();
            player.indicesCartas.Add(idCarta);
            
        }
    }

    public static void LoginACK(Packet _packet)
    {
        string msg = _packet.ReadString();
        Debug.Log("Estado del login: " + msg);
        if (msg.Equals("OK"))
        {
            string correo = _packet.ReadString();
            string avatar = _packet.ReadString();
            GameController gc = GameObject.Find("GameControler").GetComponent<GameController>();
            gc.miCorreo = correo;
            gc.tomaTuAvatar(avatar);
        }
        LoginControlador lg = GameObject.Find("controladorLogin").GetComponent<LoginControlador>();
        lg.ComprobarLogin(msg);
    }

    public static void RegisterACK(Packet _packet)
    {
        string msg = _packet.ReadString();
        Debug.Log("Estado del registro: " + msg);
        RegisterControlador rc = GameObject.Find("controladorRegister").GetComponent<RegisterControlador>();
        rc.ComprobarRegister(msg);
    }

    public static void JugadorNuevoEnSala(Packet _packet)
    {
        string nombre = _packet.ReadString();
        string avatar = _packet.ReadString();
        int turno = _packet.ReadInt();
        bool estaListo = _packet.ReadBool();
        controladorSalas cs = GameObject.Find("controladorSalas").GetComponent<controladorSalas>();
        cs.updatePlayersList(nombre, avatar, false);

        if(estaListo == true)
        {
            cs.ponerJugadorListo(nombre);
        }

        GameController gc = GameObject.Find("GameControler").GetComponent<GameController>();
        gc.nuevoJugador(nombre, avatar, turno);
        Debug.Log("Nuevo jugador en la sala, de nombre: " + nombre + " y avatar " + avatar);
    }
    
    public static void RecibirInicioPartida(Packet _packet)
    {
        //FRONT END, TODO: hace falta algo aqui?
        int turnoInicial = _packet.ReadInt();
        GameController gc = GameObject.Find("GameControler").GetComponent<GameController>();
        gc.tomaTurnoInical(turnoInicial);
        Debug.Log("Empezando partida...");
        SceneLoader sl = GameObject.Find("SceneLoader").GetComponent<SceneLoader>();
        sl.LoadPartida();
    }

    /*public static void ActualizarContador(Packet _packet)
    {
        //NOSOTROS DESPU�S
        int numListos = _packet.ReadInt();
        controladorSalas cs = GameObject.Find("controladorSalas").GetComponent<controladorSalas>();
        cs.numListosTexto.text = "Jugadores listos: " + numListos;
        Debug.Log("Actualizaci�n del contador de jugadores listos");
    }*/

    public static void SalaACK(Packet _packet)
    {
        int numCotillas = 0;
        string msg = _packet.ReadString();
        Debug.Log("Sala ACK recibido: " + msg);
        if (msg.Equals("OK"))
        {
            numCotillas = _packet.ReadInt();
        }
        controladorSalas cs = GameObject.Find("controladorSalas").GetComponent<controladorSalas>();
        cs.SalaACKRecibido(msg, numCotillas);
        //TODO: FRONTEND, si todo ha ido bien, devuelve OK, si no, devuelve ER
    }

    public static void ListaSalas(Packet _packet)
    {
        string nombreSala;
        int jugadoresEnSala;
        int numSalas = _packet.ReadInt();
        List<controladorSalas.DatosSala> datosSalas = new List<controladorSalas.DatosSala>();
        for(int i = 0; i < numSalas; i++)
        {
            nombreSala = _packet.ReadString();
            jugadoresEnSala = _packet.ReadInt();
            controladorSalas.DatosSala d = new controladorSalas.DatosSala(nombreSala, jugadoresEnSala);
            datosSalas.Add(d);
        }
        string combinedString = string.Join(",", datosSalas.ToArray());
        //Debug.Log("Las salas encontradas son: " + combinedString);
        try
        {
            controladorSalas cs = GameObject.Find("controladorSalas").GetComponent<controladorSalas>();
            cs.listaSalas = datosSalas;
            cs.updateListaSalas();
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    public static void BorrarJugadorDeSala(Packet _packet)
    {
        string nombre;
        nombre = _packet.ReadString();
        Debug.Log("El usuario " + nombre + " ha abandonado la sala");
        try
        {
            controladorSalas cs = GameObject.Find("controladorSalas").GetComponent<controladorSalas>();
            cs.eliminarJugadorDeLista(nombre);
        }
        catch(Exception e)
        {
            Debug.Log(e);
        }
         
        GameController gc = GameObject.Find("GameControler").GetComponent<GameController>();
        gc.eliminarJugadorDeGameController(nombre);
    }

    public static void JugadorListo(Packet packet)
    {
        string nombre;
        nombre = packet.ReadString();
        Debug.Log("El usuario " + nombre + " esta listo");
        controladorSalas cs = GameObject.Find("controladorSalas").GetComponent<controladorSalas>();
        cs.ponerJugadorListo(nombre);
    }

    public static void BorrarJugadorDePartida(Packet _packet)
    {
        string nombre = _packet.ReadString();

        try
        {
            PlayerDeck player = GameObject.Find("PlayerDeck").GetComponent<PlayerDeck>();
            player.jugadorEliminado(nombre);
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    public static void FinalizarPartida(Packet _packet)
    {
        string ganador = _packet.ReadString();

        //FRONTEND
        PlayerDeck player = GameObject.Find("PlayerDeck").GetComponent<PlayerDeck>();

        player.hayGanador(ganador);

    }

    public static void CambiarNumCotillas(Packet _packet)
    {
        bool sumar = _packet.ReadBool();
        Debug.Log("Hay que sumar uno a los cotillas " + sumar);
        controladorSalas cs = GameObject.Find("controladorSalas").GetComponent<controladorSalas>();
        cs.CambioNumCotillas(sumar);
    }
}
