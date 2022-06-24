using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityNpgsql;

public class PerfilManager : MonoBehaviour
{
    public TMP_Text nombre;
	public Image imagen;
	public CharacterShopDatabase characterDB;

	private int selectedOption;
    private int contador = 0;
	private int nCaracteres;
	private List<int> listaAdquiridos;
	private string nombreJugador;
	private string miavatar; 
	private string avatar;

	static NpgsqlConnection con;

	
	public PerfilManager(){
        listaAdquiridos = new List<int>();
        selectedOption = 0;
    }

    public void Start ()
	{
		characterDB.ReinicioPurchaseCharacter();

		//Fill the shop's UI list with items
		GenerateShopItemsUI ();

		GameController gc = GameObject.Find("GameControler").GetComponent<GameController>();
		nombreJugador = gc.miNombre;

		GameController gc1 = GameObject.Find("GameControler").GetComponent<GameController>();
		miavatar = gc1.miAvatar;
		selectedOption = Convert.ToInt32(miavatar);
		updateCharacter(selectedOption);

	}
    void GenerateShopItemsUI ()
	{	
		//Loop throw save purchased items and make them as purchased in the Database array
		int nCaracteres =  GameDataManager.GetAllPurchasedCharacter ().Count;
		for (int i = 0; i < nCaracteres; i++) {
			int purchasedCharacterIndex = GameDataManager.GetPurchasedCharacter (i);
			characterDB.PurchaseCharacter (purchasedCharacterIndex);
		}

		//Generate Items
		for (int i = 0; i < characterDB.CharactersCount; i++) {
			//Create a Character and its corresponding UI element (uiItem)

			Character character = characterDB.GetCharacter (i);

			if (character.isPurchased) {
				//Character is Purchased
				if(!listaAdquiridos.Contains(i)){
					listaAdquiridos.Add(i);
				}
			}
		}
	}

	public void NextOption()
	{
		contador++;

		if(contador >=  listaAdquiridos.Count)
		{   
			contador = 0;
		}
        selectedOption = listaAdquiridos[contador];
		updateCharacter(selectedOption);
	}

	public void BackOption()
	{
        contador--;
		if(contador <  0)
		{   
			contador = listaAdquiridos.Count-1;
		}
        selectedOption = listaAdquiridos[contador];
		updateCharacter(selectedOption);
	}

	private void updateCharacter(int selectedOption)
	{
		Character character = characterDB.GetCharacter(selectedOption);
		imagen.sprite = character.image;
		nombre.text = character.name;
	}

	public void SeleccionarAvatar()
	{
        try
        {
            con = dbManager.getDBConnection();
			
            string consulta0 = @"UPDATE jugador SET avatar_nombre = '"+ selectedOption +"' WHERE nombre='"+ nombreJugador +"' ";
			using var cmd0 = new NpgsqlCommand(consulta0,con);

            cmd0.ExecuteNonQuery();

			Debug.Log("opcion seleccionada" + selectedOption);


			con.Close();
        }
        catch (Exception ex)
        {
            Debug.Log("No se ha podido cambiar el avatar del jugador en perfil Manager(" + ex);
        }
	}

	public void obtenerAvatar()
	{
        try{
            con = dbManager.getDBConnection();
            string consulta0 = "SELECT * FROM jugador WHERE nombre= '"+ nombreJugador +"' ";
            using var cmd = new NpgsqlCommand(consulta0,con);

            using NpgsqlDataReader rdr = cmd.ExecuteReader();
            
            cmd.CommandText = consulta0;

            while (rdr.Read())
            {   
                avatar = rdr.GetString(6);
				Debug.Log("obteniendo avatar..." + avatar);
            }
            selectedOption = Convert.ToInt32(avatar);
			Debug.Log("El avatar obtenido de la bbdd es " + selectedOption );
			updateCharacter(selectedOption);

			GameController gc = GameObject.Find("GameControler").GetComponent<GameController>();
			gc.tomaTuAvatar(avatar);

            con.Close();
        }
        catch (Exception ex)
        {
            Debug.Log("No se ha obtener el caracter del jugador en perfil Manager: (" + ex);
        }
	}
}
