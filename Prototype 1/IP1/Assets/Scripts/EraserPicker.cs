using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EraserPicker : MonoBehaviour
{
    private void OnMouseDown()
    {
        PenController pen = FindObjectOfType<PenController>();
        if (pen != null)
        {
            pen.isEraser = true;
        }
    }
}


