using System.Collections.Generic;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityNpgsql;

//Shop Data Holder
[System.Serializable] public class CharactersShopData
{
	public List<int> purchasedCharactersIndexes = new List<int> ();
	public List<String> listaAvatarBD = new List<String> ();
}
//Player Data Holder
[System.Serializable] public class PlayerData
{
	public int coins = 0;
	public int selectedCharacterIndex = 0;
}

public static class GameDataManager
{
	static PlayerData playerData = new PlayerData ();
	static CharactersShopData charactersShopData = new CharactersShopData ();
	
	static NpgsqlConnection con;

	static Character selectedCharacter;
	static string nombreJugador;

	//Player Data Methods -----------------------------------------------------------------------------
	public static Character GetSelectedCharacter ()
	{
		return selectedCharacter;
	}

	public static void SetSelectedCharacter (Character character, int index)
	{
		selectedCharacter = character;
		playerData.selectedCharacterIndex = index;
		//SavePlayerData ();
	}

	public static int GetSelectedCharacterIndex ()
	{
		return playerData.selectedCharacterIndex;
	}

	//J1 por defecto, habrá que cambiar el nombre del jugador
	public static int GetCoins ()
	{
		try
        {   
			GameController gc = GameObject.Find("GameControler").GetComponent<GameController>();
        	nombreJugador = gc.miNombre;
            con = dbManager.getDBConnection();
			//J1 por defecto, habrá que cambiar el nombre del jugador
            string consulta = "SELECT * FROM jugador WHERE nombre= '"+nombreJugador+"'";
            using var cmd = new NpgsqlCommand(consulta,con);

            using NpgsqlDataReader rdr = cmd.ExecuteReader();
            
            cmd.CommandText = consulta;
            
            //Leo la base de datos y añado las Personas de una sala a la lista
            while (rdr.Read())
            {   
				playerData.coins = rdr.GetInt32(5);
				Debug.Log("Monedas de jugador" + playerData.coins);
            }
			
			con.Close();

        }
        catch (Exception ex)
        {
            Debug.Log("Error Obteniendo las monedas/puntos :(" + ex);
        }

		return playerData.coins;
	}

	//J1 por defecto, habrá que cambiar el nombre del jugador
	public static void AddCoins (int amount)
	{
		try
        {
			GameController gc = GameObject.Find("GameControler").GetComponent<GameController>();
        	nombreJugador = gc.miNombre;
            con = dbManager.getDBConnection();
			playerData.coins += amount;

			//Cambiar el JUGADOR_Nombre, le he puesto J1 x defecto
            string consulta0 = @"UPDATE jugador SET puntos = '"+playerData.coins+"' WHERE nombre='"+nombreJugador+"'";
            using var cmd0 = new NpgsqlCommand(consulta0,con);

            cmd0.ExecuteNonQuery();

			//Meto en una lista los caracteres que ha comprado un jugador
			Debug.Log("Las monedas añadidas son " + playerData.coins);

			GameSharedUI.Instance.Start();

			con.Close();
        }
        catch (Exception ex)
        {
            Debug.Log("No se ha podido añadir las monedas al jugador(" + ex);
        }
	}

	public static bool CanSpendCoins (int amount)
	{
		Debug.Log("Monedas actuales: " + playerData.coins);
		return (playerData.coins >= amount);
	}

	public static void SpendCoins (int amount)
	{
		try
        {
			GameController gc = GameObject.Find("GameControler").GetComponent<GameController>();
        	nombreJugador = gc.miNombre;
            con = dbManager.getDBConnection();
			playerData.coins -= amount;

			//Cambiar el JUGADOR_Nombre, le he puesto J1 x defecto
            string consulta0 = @"UPDATE jugador SET puntos = '"+playerData.coins+"' WHERE nombre='"+nombreJugador+"'";
            using var cmd0 = new NpgsqlCommand(consulta0,con);

            cmd0.ExecuteNonQuery();

			//Meto en una lista los caracteres que ha comprado un jugador
			Debug.Log("Las monedas que hay ahora después de restar: " + playerData.coins);

			con.Close();
        }
        catch (Exception ex)
        {
            Debug.Log("No se ha podido restar las monedas al jugador(" + ex);
        }
	}

	//Characters Shop Data Methods -----------------------------------------------------------------------------
	
	//J1 por defecto, habrá que cambiar el nombre del jugador
	public static void AddPurchasedCharacter (int characterIndex)
	{
		try
        {
			GameController gc = GameObject.Find("GameControler").GetComponent<GameController>();
        	nombreJugador = gc.miNombre;
			string indice_string = Convert.ToString(characterIndex);
            con = dbManager.getDBConnection();

			//Cambiar el JUGADOR_Nombre, le he puesto J1 x defecto
            string consulta0 = @"INSERT INTO desbloqueado_avatar VALUES ('"+nombreJugador+"','" + indice_string + "')";
            using var cmd0 = new NpgsqlCommand(consulta0,con);

            cmd0.ExecuteNonQuery();

			//Meto en una lista los caracteres que ha comprado un jugador
			charactersShopData.purchasedCharactersIndexes.Add (characterIndex);
			Debug.Log("Se ha añadido el caracter " + charactersShopData.purchasedCharactersIndexes [characterIndex] );
			
			if (characterIndex == 0){
				controladorLogro cL = GameObject.Find("controladorLogros").GetComponent<controladorLogro>();
				cL.CompraPomposo();
			}
			else if (characterIndex == 1){
				controladorLogro cL = GameObject.Find("controladorLogros").GetComponent<controladorLogro>();
				cL.CompraDaddyYankee();
			}
			con.Close();
        }
        catch (Exception ex)
        {
            Debug.Log("No se ha podido añadir el Avatar al jugador(" + ex);
        }
	}

	
	public static List<int> GetAllPurchasedCharacter ()
	{
        try
        {   
			GameController gc = GameObject.Find("GameControler").GetComponent<GameController>();
        	nombreJugador = gc.miNombre;
            con = dbManager.getDBConnection();
			//J1 por defecto, habrá que cambiar el nombre del jugador
            string consulta = "SELECT * FROM desbloqueado_avatar WHERE JUGADOR_Nombre= '"+nombreJugador+"'";
            using var cmd = new NpgsqlCommand(consulta,con);

            using NpgsqlDataReader rdr = cmd.ExecuteReader();
            
            cmd.CommandText = consulta;
            
            //Leo la base de datos y añado las Personas de una sala a la lista
            while (rdr.Read())
            {   
				if(!charactersShopData.listaAvatarBD.Contains(rdr.GetString(1))){
					charactersShopData.listaAvatarBD.Add(rdr.GetString(1));
				}
            }

			foreach(string avatar in charactersShopData.listaAvatarBD)
			{
				if(!charactersShopData.purchasedCharactersIndexes.Contains(Convert.ToInt32(avatar)))
				{
					charactersShopData.purchasedCharactersIndexes.Add (Convert.ToInt32(avatar));
				}
				
			}
			
			con.Close();

        }
        catch (Exception ex)
        {
            Debug.Log("Error Obteniendo los avatares de una persona :(" + ex);
        }

		return charactersShopData.purchasedCharactersIndexes;
	}

	public static int GetPurchasedCharacter (int index)
	{
		return charactersShopData.purchasedCharactersIndexes [index];
	}

}
