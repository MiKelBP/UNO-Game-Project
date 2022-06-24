using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityNpgsql;


public class editarPerfil : MonoBehaviour
{
    public NpgsqlConnection con;

    [SerializeField] private InputField emailJugadorInput; //donde escribe el jugador
    [SerializeField] private InputField contrasegnaJugadorInput; //donde escribe el jugador
    private string nombreJugador;
    string consulta0; 
    private PerfilManager fotoPerfil;

    // Start is called before the first frame update
    void Start()
    {
        GameController gc = GameObject.Find("GameControler").GetComponent<GameController>();
        nombreJugador = gc.miNombre; 
        llamadaSeleccionarAvatar();
    }

    public void editarDatosPerfil()
    {   
        try{
            if(contrasegnaJugadorInput.text.Equals("") && contrasegnaJugadorInput.text.Equals("") ){ // los 2 vacios
                //se selecciona el avatar desde fuera
            }
            else if(emailJugadorInput.text.Equals("")){
                actualizarcontrasegna();
            }
            else if(contrasegnaJugadorInput.text.Equals("")){
                actualizarCorreo();
            }
            else{
                actualizarcontrasegna();
                actualizarCorreo();
            }
        }
        catch (Exception ex)
        {
            Debug.Log("Error actualizando los datos de los jugadores en el Perfil:(" + ex);
        }
    }

    public void actualizarCorreo()
    {   
        try{
            con = dbManager.getDBConnection();
            consulta0 = @"UPDATE jugador SET correo = '"+ emailJugadorInput.text + "' WHERE nombre= '"+ nombreJugador + "' ";
            using var cmd = new NpgsqlCommand(consulta0,con);
            cmd.ExecuteNonQuery();

            con.Close();
        }
        catch (Exception ex)
        {
            Debug.Log("Error actualizando los el correo de los jugadores en el Perfil:(" + ex);
        }
    }

    public void actualizarcontrasegna()
    {   
        try{
            con = dbManager.getDBConnection();
        
            consulta0 = @"UPDATE jugador SET hash_contraseña = '"+ contrasegnaJugadorInput.text.GetHashCode() + "' WHERE nombre= '"+ nombreJugador + "' ";
            using var cmd = new NpgsqlCommand(consulta0,con);
            cmd.ExecuteNonQuery();

            con.Close();
        }
        catch (Exception ex)
        {
            Debug.Log("Error actualizando la contraseña de los jugadores en el Perfil:(" + ex);
        }
    }

    public void llamadaSeleccionarAvatar(){
        fotoPerfil = FindObjectOfType<PerfilManager>();
        fotoPerfil.SeleccionarAvatar();
    }
}
