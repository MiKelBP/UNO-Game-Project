using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Backend
{
    public class RegisterControlador : MonoBehaviour
    {
        [SerializeField] private InputField nombreInput;
        [SerializeField] private InputField correoInput;
        [SerializeField] private InputField passwordInput;
        [SerializeField] private Text errorText;
        [SerializeField] private GameObject menuRegister;
        [SerializeField] private GameObject menuPrincipal;


        public void registrarse()
        {
            string nombre = nombreInput.text;
            string mail = correoInput.text;
            string password = passwordInput.text;
            bool errorOcurrido = false;
            errorText.gameObject.SetActive(false);

            if (nombre == null || nombre == "")
            {
                errorRegistro("Los campos son obligatorios");
                errorOcurrido = true;
            }

            if (mail == null || mail == "")
            {
                errorRegistro("Los campos son obligatorios");
                errorOcurrido = true;
            }

            if (password == null || nombre == "")
            {
                errorRegistro("Los campos son obligatorios");
                errorOcurrido = true;
            }

            if (!errorOcurrido)
            {
                try
                {
                    nombreInput.enabled = false;
                    correoInput.enabled = false;
                    passwordInput.enabled = false;
                    ClientSend.SendRegister(nombre, mail, password.GetHashCode());
                }
                catch (Exception ex)
                {
                    errorRegistro(ex.ToString());
                }
            }

        }

        public void ComprobarRegister(string msg)
        {
            nombreInput.enabled = true;
            correoInput.enabled = true;
            passwordInput.enabled = true;
            if (msg == "OK")
            {
                menuRegister.SetActive(false);
                menuPrincipal.SetActive(true);
                GameController gc = GameObject.Find("GameControler").GetComponent<GameController>();
                gc.tomaMiNombre(nombreInput.text);
                gc.miCorreo = correoInput.text;
                gc.miAvatar = "0";
                nombreInput.text = "";
                correoInput.text = "";
                passwordInput.text = "";
            }
            else
            {
                errorRegistro(msg);
            }
        }

        private void errorRegistro(string err)
        {
            Debug.Log("Error al registrarse: " + err);
            errorText.text = err;
            errorText.gameObject.SetActive(true);
            passwordInput.text = "";
        }
    }
}
