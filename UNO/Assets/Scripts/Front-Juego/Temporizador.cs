using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Temporizador : MonoBehaviour
{

    public Text tiempoText;
    public static float tiempo = 0.0f;
    public static int comenzar = 0;

    // Update is called once per frame
    void Update()
    {
        if(comenzar == 1){
            tiempo -= Time.deltaTime;
            tiempoText.text = "" + tiempo.ToString("f0");
        } else{
            tiempo = 10;
        }
    }
}