using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public GameObject pantalla;
    public GameObject b1;
    public GameObject b2;
    public GameObject b_nuevo;



    // Start is called before the first frame update
    /*void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }*/
    public void ayudita1(){
        if (pantalla != null){
            pantalla.SetActive(true);
        }
    }

    public void ayudita2(){
        if (pantalla != null){
            pantalla.SetActive(true);
        }
        if (b1 != null){
            b1.SetActive(true);
        }
        b_nuevo.SetActive(false);
    }

    public void ayudita3(){
        if (pantalla != null){
            pantalla.SetActive(true);
        }

        if (b1 != null){
            b1.SetActive(true);
        }

        if (b2 != null){
            b2.SetActive(true);
        }
        b_nuevo.SetActive(false);
    }

    public void ayudita1_false(){
        if (pantalla != null){
            pantalla.SetActive(false);
        }
    }

    public void ayudita2_false(){
        if (pantalla != null){
            pantalla.SetActive(false);
        }
        if (b1 != null){
            b1.SetActive(false);
        }
        b_nuevo.SetActive(true); 
    }

    public void ayudita3_false(){
        if (pantalla != null){
            pantalla.SetActive(false);
        }

        if (b1 != null){
            b1.SetActive(false);
        }

        if (b2 != null){
            b2.SetActive(false);
        }
        b_nuevo.SetActive(true);

    }


}
