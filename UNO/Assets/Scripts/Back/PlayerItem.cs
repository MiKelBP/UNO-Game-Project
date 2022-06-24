using System.Collections.Generic;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerItem : MonoBehaviour
{
    public Text playerName;
    public GameObject button;
    public GameObject imagenListo;
    public Image avatarImage;
    public TMP_Text avatarText;

    public CharacterShopDatabase characterDB;

    public void SetPlayerInfo(string _playerName, string _avatar)
    {
        button.GetComponent<Button>().enabled = false;
        GameObject go = button;
        go.SetActive(false);
        playerName.text = _playerName;
        //avatar = Convert.ToInt32(_avatar);
        Debug.Log("Carga el avatar " + _avatar);
        int a = Int32.Parse(_avatar);
        Character character = characterDB.GetCharacter(a);
        avatarImage.sprite = character.image;
        avatarText.text = character.name;
        //avatarImage.sprite = Load("characters", "characters_" + _avatar);
    }

    Sprite Load(string imageName, string spriteName)
    {
        Sprite[] all = Resources.LoadAll<Sprite>(imageName);

        foreach (var s in all)
        {
            if (s.name == spriteName)
            {
                return s;
            }
        }
        return null;
    }

    public void cambiarColor()
    {
        button.GetComponent<Button>().enabled = true;
        GameObject go = button;
        go.SetActive(true);
        button.GetComponent<Image>().color = Color.red;
    }

    public void accion()
    {
        button.GetComponent<Image>().color = Color.green;
        controladorSalas cs = GameObject.Find("controladorSalas").GetComponent<controladorSalas>();
        cs.estaListo();
    }

    public void estaListo()
    {
        imagenListo.SetActive(true);
    }
}
