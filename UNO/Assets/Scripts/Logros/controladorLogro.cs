using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class controladorLogro : MonoBehaviour
{
    [Header ("Pop up")]
    [SerializeField] GameObject popUp; 
    private string nombreJugador;

    public void reinicia(){
        LogrosManager lm = GameObject.Find("LogrosManagers").GetComponent<LogrosManager>();
		lm.GenerarLogros();
    }

    public void entraAunaSala()//0
    {
        GameController gc = GameObject.Find("GameControler").GetComponent<GameController>();
		nombreJugador = gc.miNombre;
        if(!LogroDataManager.haConseguidoLogro(nombreJugador, "0")){
            LogroDataManager.AddLogro(nombreJugador, "0");
            GameDataManager.AddCoins(200);
            Debug.Log("Logro añadido entrar sala");
		    StartCoroutine(showpopUp());
        }
    }
    public void CompraPomposo(){ //1
        GameController gc = GameObject.Find("GameControler").GetComponent<GameController>();
		nombreJugador = gc.miNombre;
        if(!LogroDataManager.haConseguidoLogro(nombreJugador, "1")){
            LogroDataManager.AddLogro(nombreJugador, "1");
            GameDataManager.AddCoins(250);
            Debug.Log("Logro añadido CompraPomposo");
            StartCoroutine(showpopUp());
            
        }
    }
    public void CompraDaddyYankee(){ //2
        GameController gc = GameObject.Find("GameControler").GetComponent<GameController>();
		nombreJugador = gc.miNombre;
        if(!LogroDataManager.haConseguidoLogro(nombreJugador, "2")){
            LogroDataManager.AddLogro(nombreJugador, "2");
            GameDataManager.AddCoins(250);
            Debug.Log("Logro añadido CompraDaddyYankee");
            StartCoroutine(showpopUp());
            
        }
    }
    public void gana1Partida()//3
    {
        GameController gc = GameObject.Find("GameControler").GetComponent<GameController>();
		nombreJugador = gc.miNombre;
        if(!LogroDataManager.haConseguidoLogro(nombreJugador, "3")){
            if(LogroDataManager.obtenerPartidasGanadas() == 1){
                LogroDataManager.AddLogro(nombreJugador, "3");
                GameDataManager.AddCoins(250);
                Debug.Log("Logro añadido ganar 1 partida");
                StartCoroutine(showpopUp());
            }
        }
    }

    public void gana10Partidas(){ //4
        GameController gc = GameObject.Find("GameControler").GetComponent<GameController>();
		nombreJugador = gc.miNombre;
        if(!LogroDataManager.haConseguidoLogro(nombreJugador, "4")){
            if(LogroDataManager.obtenerPartidasGanadas() == 10){
                LogroDataManager.AddLogro(nombreJugador, "4");
                GameDataManager.AddCoins(250);
                Debug.Log("Logro añadido ganar 10 partidas");
                StartCoroutine(showpopUp());
            }
        }
    }

    public void cambiaAvatar(){ //5
        GameController gc = GameObject.Find("GameControler").GetComponent<GameController>();
		nombreJugador = gc.miNombre;
        if(!LogroDataManager.haConseguidoLogro(nombreJugador, "5")){
            LogroDataManager.AddLogro(nombreJugador, "5");
            GameDataManager.AddCoins(300);
            Debug.Log("Logro añadido cambiar Avatar");
            StartCoroutine(showpopUp());
            
        }
    }

    public void compraAvatares()//6
    {
        GameController gc = GameObject.Find("GameControler").GetComponent<GameController>();
		nombreJugador = gc.miNombre;
        if(!LogroDataManager.haConseguidoLogro(nombreJugador, "6")){
            int nLogros =  LogroDataManager.GetAllPurchasedLogros ().Count;
            if(nLogros == 10){
                LogroDataManager.AddLogro(nombreJugador, "6");
                GameDataManager.AddCoins(250);
                Debug.Log("Logro añadido comprar Avatares");
                StartCoroutine(showpopUp());
            }
        }
    }

    public void gana5Partidas(){ //7
        GameController gc = GameObject.Find("GameControler").GetComponent<GameController>();
		nombreJugador = gc.miNombre;
        if(!LogroDataManager.haConseguidoLogro(nombreJugador, "7")){
            if(LogroDataManager.obtenerPartidasGanadas() == 5){
                LogroDataManager.AddLogro(nombreJugador, "7");
                GameDataManager.AddCoins(350);
                Debug.Log("Logro añadido ganar 7 partidas");
                StartCoroutine(showpopUp());
            }
        }
    }

    public void consigue1000Monedas(){ //8
        if(!LogroDataManager.haConseguidoLogro(nombreJugador, "8")){
            if(GameDataManager.GetCoins() == 1000){
                LogroDataManager.AddLogro(nombreJugador, "8");
                GameDataManager.AddCoins(450);
                Debug.Log("Logro añadido conseguir 1000 monedas");
                StartCoroutine(showpopUp());
            }
        }
    }
    public void consigue5000Monedas(){ //9
        if(!LogroDataManager.haConseguidoLogro(nombreJugador, "9")){
            if(GameDataManager.GetCoins() == 5000){
                LogroDataManager.AddLogro(nombreJugador, "9");
                GameDataManager.AddCoins(1000);
                Debug.Log("Logro añadido conseguir 5000 monedas");
                StartCoroutine(showpopUp());
            }
        }
    }

    public IEnumerator showpopUp()
    {
        //Pongo a falso el menu lobbies
        popUp.SetActive(true);

        yield return new WaitForSeconds(3);
        print("3 seconds has passed");
        
        //Entrar a la sala creada
        popUp.SetActive(false);
    }
}