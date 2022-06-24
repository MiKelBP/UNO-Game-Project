using System.Collections.Generic;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityNpgsql;

[System.Serializable] public class LogrosData
{
    public List<int> purchasedLogrosIndexes = new List<int> ();
	public List<String> listaLogrosBD = new List<String> ();
}

public class LogroDataManager : MonoBehaviour
{
    static NpgsqlConnection con;
    static LogrosData logrosData = new LogrosData ();
    static string nombreJugador;
    static string mi_logro;
    static int ganadasJugador;

	private void Start() {
        GameController gc = GameObject.Find("GameControler").GetComponent<GameController>();
        nombreJugador = gc.miNombre; 
    }
	public static List<int> GetAllPurchasedLogros ()
	{
        try
        {   
            con = dbManager.getDBConnection();
            GameController gc = GameObject.Find("GameControler").GetComponent<GameController>();
		    nombreJugador = gc.miNombre;
			//J1 por defecto, habrá que cambiar el nombre del jugador
            string consulta = "SELECT * FROM obtiene_logro WHERE jugador_nombre= '"+ nombreJugador +"'";
            using var cmd = new NpgsqlCommand(consulta,con);

            using NpgsqlDataReader rdr = cmd.ExecuteReader();
            
            cmd.CommandText = consulta;
            
            //Leo la base de datos y añado las Personas de una sala a la lista
            while (rdr.Read())
            {   
				if(!logrosData.listaLogrosBD.Contains(rdr.GetString(0))){
					logrosData.listaLogrosBD.Add(rdr.GetString(0));
				}
            }

            foreach(string logro in logrosData.listaLogrosBD)
			{
				if(!logrosData.purchasedLogrosIndexes.Contains(Convert.ToInt32(logro)))
				{
					logrosData.purchasedLogrosIndexes.Add (Convert.ToInt32(logro));
				}
				
			}
			
			con.Close();

        }
        catch (Exception ex)
        {
            Debug.Log("Error Obteniendo todos los logros de una persona (GetAllPurchasedLogros):(" + ex);
        }

		return logrosData.purchasedLogrosIndexes;

    }

    public static int GetPurchasedLogro (int index)
	{
		return logrosData.purchasedLogrosIndexes [index];
	}

    public static bool haConseguidoLogro(string nJ, string logro)
    {
        try
        {   
            con = dbManager.getDBConnection();
			//J1 por defecto, habrá que cambiar el nombre del jugador
            string consulta = "SELECT * FROM obtiene_logro WHERE jugador_nombre= '" + nJ +"' AND logro_nombre= '" + logro + "'";
            using var cmd = new NpgsqlCommand(consulta,con);

            using NpgsqlDataReader rdr = cmd.ExecuteReader();
            
            cmd.CommandText = consulta;
            
            //Leo la base de datos y añado las Personas de una sala a la lista
            while (rdr.Read())
            {   
				mi_logro = rdr.GetString(0);
                Debug.Log("logro leido" + mi_logro);
            }
			
			con.Close();

        }
        catch (Exception ex)
        {
            Debug.Log("Error Obteniendo un logros de una persona (haConseguidoLogro) :(" + ex);
        }
        
        if(mi_logro == logro){
            return true;
        }else return false; 
    }

    public static void AddLogro(string nJ, string logro)
    {
        try
        {   
            con = dbManager.getDBConnection();
			//J1 por defecto, habrá que cambiar el nombre del jugador 
            string consulta = @"INSERT INTO obtiene_logro VALUES('" + logro +"','" + nJ +"')";
            using var cmd = new NpgsqlCommand(consulta,con);

            cmd.ExecuteNonQuery();
            
            cmd.CommandText = consulta;
            
			con.Close();
        }
        catch (Exception ex)
        {
            Debug.Log("Error añadiendo los logros de una persona :(" + ex);
        }
    }


    public static int obtenerPartidasGanadas()
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
            }

            con.Close();
        }
        catch (Exception ex)
        {
            Debug.Log("Error las partidas ganadas para los logros:(" + ex);
        }

        return ganadasJugador;
    }  
}
