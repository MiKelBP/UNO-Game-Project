using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPrincipalControlador : MonoBehaviour
{
    public GameObject panelPequeño;
    public GameObject menuPrincipal;
    public GameObject menuSalas;
     public GameObject shopUI;
    // Start is called before the first frame update
    void Start()
    {
        panelPequeño.SetActive(true);
    }

    public void entrarJuego()
    {
        menuPrincipal.SetActive(false);
        menuSalas.SetActive(true);
    }

    public void OpenShop ()
	{
		menuPrincipal.SetActive (false);
        shopUI.SetActive (true);
	}

    // Update is called once per frame
}
