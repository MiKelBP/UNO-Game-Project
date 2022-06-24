using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PerfilDatabase : MonoBehaviour
{
    public static List<Sprite> avataresList = new List<Sprite>();
    private string path = "characters";

    void Awake()
    {
        avataresList.Add(Load(path, "characters_0"));
        avataresList.Add(Load(path, "characters_1"));
        //avataresList.Add(Load("spritesheet_uno", "spritesheet_uno_0"));
        avataresList.Add(Load(path, "characters_2"));
        avataresList.Add(Load(path, "characters_3"));
        avataresList.Add(Load(path, "characters_4"));
        avataresList.Add(Load(path, "characters_5"));
        avataresList.Add(Load(path, "characters_6"));
        avataresList.Add(Load(path, "characters_7"));
        avataresList.Add(Load(path, "characters_8"));
        avataresList.Add(Load(path, "characters_9"));
        avataresList.Add(Load(path, "characters_10"));
        avataresList.Add(Load(path, "characters_11"));
        avataresList.Add(Load(path, "characters_12"));
        avataresList.Add(Load(path, "characters_13"));
        avataresList.Add(Load(path, "characters_14"));
        avataresList.Add(Load(path, "characters_15"));
        avataresList.Add(Load(path, "characters_16"));
        avataresList.Add(Load(path, "characters_17"));
        avataresList.Add(Load(path, "characters_18"));
        avataresList.Add(Load(path, "characters_19"));
    }

    Sprite Load( string imageName, string spriteName)
    {
        Sprite[] all = Resources.LoadAll<Sprite>( imageName);
 
        foreach( var s in all)
        {
            if (s.name == spriteName)
            {
                return s;
            }
        }
        return null;
    }
}
