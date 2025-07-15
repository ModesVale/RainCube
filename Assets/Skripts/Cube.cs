using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    private void Update()
    {
        ColorChange();
    }

    private void ColorChange()
    {
        GetComponent<Renderer>().material.color = Random.ColorHSV();
    }
}
