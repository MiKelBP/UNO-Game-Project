using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class datosJugador : MonoBehaviour
{
    [SerializeField] private InputField nombreInputR; //campo del registro
    [SerializeField] private InputField nombreInputL; //campo del Login
    private static string nombre; 

    public void setDatos()
    {
        if (nombreInputR.text != "") //El usuario se he registrado
        {
            nombre = nombreInputR.text;
        }

        else
        {
            nombre = nombreInputL.text;
        }
        
        Debug.Log("El Usuario guardado es " + nombre);
    }

    public static string GetDatos()
    {
        return nombre;
    }

}
