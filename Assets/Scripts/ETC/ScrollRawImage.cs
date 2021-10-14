using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollRawImage : MonoBehaviour
{
    private RawImage rawImage;
    private float uvRectY = 0;

    private void Start()
    {
        rawImage = GetComponent<RawImage>();
    }

    private void Update()
    {
        uvRectY -= 0.001f;
        if (uvRectY < -100f)
            uvRectY = 0;
        rawImage.uvRect = new Rect(0, uvRectY, 1, 1);
    }
}
