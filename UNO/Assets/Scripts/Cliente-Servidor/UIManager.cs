using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Backend;

public class UIManager : MonoBehaviour
{
    public UIManager instance;
    [SerializeField] private InputField salaInput;
    [SerializeField] private InputField cartasInput;
    [SerializeField] private InputField cartaInput;
    [SerializeField] private InputField nombreInput;
    public GameObject botonMonedas;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }

    //Ocultar el botón en la pantalla de inicio
    private void Start()
    {
        //botonMonedas.SetActive(false);
    }

    public void ConnectToServer()
    {
        Client.instance.ConnectToServer();
    }

    public void SendCardToServer()
    {
        string sala = salaInput.text;
        int carta = Int32.Parse(cartaInput.text);
        ClientSend.SendCard(sala, carta,"");
    }

    public void CambiarEscena()
    {
        //GameController.tomaSala("mira");
        SceneManager.LoadScene("Main");
    }

    public void CrearSala()
    {
        string sala = salaInput.text;
        string nombre = nombreInput.text;
        ClientSend.EntraSala(sala, true, nombre, "Avatar 1");
    }

    public void EntrarSala()
    {
        string sala = salaInput.text;
        string nombre = nombreInput.text;
        ClientSend.EntraSala(sala, false, nombre, "Avatar 1");
    }

    public void PedirCartas()
    {
        string sala = salaInput.text;
        int numCartas = Int32.Parse(cartasInput.text);
        ClientSend.RobarCartas(sala, numCartas);
    }
}
