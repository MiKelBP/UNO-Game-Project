using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityNpgsql;
using TMPro;


public class Perfil : MonoBehaviour
{
    public NpgsqlConnection con;

    public Text email;
    public Text nombre;
    public Text ganadas;
    public Text jugadas;
    private string nombreJugador;
    private string correoJugador;
    private int ganadasJugador;
    private int jugadasJugador;
    PerfilManager fotoPerfil; 

    private void Start()
    { 
        GameController gc = GameObject.Find("GameControler").GetComponent<GameController>();
        nombreJugador = gc.miNombre; //aqui tienes el nombre de usuairo del jugador
        nombre.text=nombreJugador;
        obtenerEmailGanadasyJugadasPerfil();

        fotoPerfil = FindObjectOfType<PerfilManager>();
        fotoPerfil.obtenerAvatar();
    }

    public void obtenerEmailGanadasyJugadasPerfil()
    {   
        try{
            con = dbManager.getDBConnection();
            string consulta0 = "SELECT * FROM jugador WHERE nombre= '"+ nombreJugador +"'";
            using var cmd = new NpgsqlCommand(consulta0,con);

            using NpgsqlDataReader rdr = cmd.ExecuteReader();
            
            cmd.CommandText = consulta0;

            while (rdr.Read())
            {   
                ganadasJugador = rdr.GetInt32(1);
                jugadasJugador = rdr.GetInt32(2);
                correoJugador = rdr.GetString(3);
            }
            ganadas.text = ganadasJugador.ToString();
            jugadas.text = jugadasJugador.ToString();
            email.text = correoJugador;

            con.Close();
        }
        catch (Exception ex)
        {
            Debug.Log("Error obteniendo el perfil y las partidas ganadas y jugadas de los jugadores en el Perfil:(" + ex);
        }
    }  
}
