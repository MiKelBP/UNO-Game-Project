using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityNpgsql;

namespace Backend { 
    public class LoginControlador : MonoBehaviour
    {
        [SerializeField] private InputField nombreInput;
        [SerializeField] private InputField passwordInput;
        [SerializeField] private Text errorText;
        [SerializeField] private GameObject menuLogin;
        [SerializeField] private GameObject menuPrincipal;
        private const string PlayerPrefsNameKey = "PlayerName";

        public void iniciarSesion()
        {
            string nombre = nombreInput.text;
            string password = passwordInput.text;
            bool errorOcurrido = false;
            errorText.gameObject.SetActive(false);


            if (nombre != null || nombre != "")
            {
                nombre = nombreInput.text;
		        PlayerPrefs.SetString(PlayerPrefsNameKey, nombre);
            }
            else{
                errorLogin("Los campos son obligatorios");
                errorOcurrido = true;
            }

            if (password == null || nombre == "")
            {
                errorLogin("El campo contraseña no puede estar vacío");
                errorOcurrido = true;
            }

            if (!errorOcurrido)
            {
                try
                {
                    nombreInput.enabled = false;
                    passwordInput.enabled = false;
                    ClientSend.SendLogin(nombre, password.GetHashCode());
                }
                catch (Exception ex)
                {
                    errorLogin(ex.ToString());
                }
            }
        }

        public void ComprobarLogin(string msg)
        {
            nombreInput.enabled = true;
            passwordInput.enabled = true;
            if ( msg == "OK" )
            {
                menuLogin.SetActive(false);
                menuPrincipal.SetActive(true);
                GameController gc = GameObject.Find("GameControler").GetComponent<GameController>();
                gc.tomaMiNombre(nombreInput.text);
                nombreInput.text = "";
                passwordInput.text = "";
            }
            else
            {
                errorLogin("Contraseña incorrecta");
            }
        }

        public void GetnombreJugador()
        {
            if (PlayerPrefs.HasKey(PlayerPrefsNameKey))
            {
                nombreInput.text = PlayerPrefs.GetString(PlayerPrefsNameKey);
            }
        }

        private void errorLogin(string err)
        {
            Debug.Log("Error al iniciar sesión: " + err);
            errorText.text = err;
            errorText.gameObject.SetActive(true);
            passwordInput.text = "";
        }
    }
}
