using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sala : MonoBehaviour
{
    public Text sala;
    // Start is called before the first frame update
    void Start()
    {
        sala.text = GameController.sala;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
