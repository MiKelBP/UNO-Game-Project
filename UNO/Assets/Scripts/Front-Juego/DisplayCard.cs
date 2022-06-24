using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DisplayCard : MonoBehaviour
{
    public int id;
    public int valor;
    public string color;
    public string accion;
    public Sprite spriteImagen;

    public bool cardBack;
    public static bool staticCardBack;

    public Image artImage;

    public Transform target;
    private Vector2 position;
    private Vector2 initialPosition;
    public int mover = 0;
    public int apagate = 0;
    public int speed;
    public int movimientoContrario = 0;

    public void laCarta(int id){
        this.id = id;
    }

    // Start is called before the first frame update
    void Start()
    {
        id = 41;
        valor = CardDatabase.cardList[id].valor;
        color = CardDatabase.cardList[id].color;
        accion = CardDatabase.cardList[id].accion;
        spriteImagen = CardDatabase.cardList[id].spriteImagen;

        artImage.sprite = spriteImagen;
        position = gameObject.transform.position;
        initialPosition = position;
    }

    // Update is called once per frame
    void Update()
    {
        valor = CardDatabase.cardList[id].valor;
        color = CardDatabase.cardList[id].color;
        accion = CardDatabase.cardList[id].accion;
        spriteImagen = CardDatabase.cardList[id].spriteImagen;

        artImage.sprite = spriteImagen;
        staticCardBack = cardBack;

        //Para que se mueva
        float step = Time.deltaTime * speed;

        if(mover == 1){
            transform.position = Vector2.MoveTowards(transform.position, target.position, step);
        } else if(apagate == 1){
            if (mover == 0){
                gameObject.transform.position = initialPosition;
                gameObject.SetActive(true);
            } else{
                gameObject.SetActive(false);
            }
        }

        if(movimientoContrario == 1){ //Para gestionar cartas de los contrarios
            gameObject.transform.position = initialPosition;
            gameObject.SetActive(false);
        }
    }
}