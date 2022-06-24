using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitGame : MonoBehaviour
{
    public void ExitGameFunc()
    {
        Application.Quit();
        Debug.Log("El juego se está cerrando...");
    }
}