using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollableImage : MonoBehaviour
{
    [SerializeField] RawImage scrollableImage;
    [SerializeField]float _x, _y;

    // Update is called once per frame
    void Update()
    {
        scrollableImage.uvRect = new Rect(scrollableImage.uvRect.position + new Vector2(_x, _y) * Time.deltaTime, scrollableImage.uvRect.size);
    }
}
