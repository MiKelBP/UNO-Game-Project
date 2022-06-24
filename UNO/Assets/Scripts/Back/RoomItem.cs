using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RoomItem : MonoBehaviour
{
    public Text roomName;
    public Text numJugadores;
    controladorSalas manager;

    private void Start()
    {
        manager = FindObjectOfType<controladorSalas>();
    }

    public void SetNombreSala(string _roomName, int _numJugadores){
        roomName.text =  _roomName;
        if (_numJugadores == -1)
        {
            numJugadores.text = "En partida";
            numJugadores.color = Color.red;
        }
        else
        {
            numJugadores.text = _numJugadores + "/4";
            numJugadores.fontSize = 22;
            numJugadores.color = Color.green;
        }
    }

    public void OnClickItem()
    {
      manager.entrarSala(roomName.text);  
    }
}
