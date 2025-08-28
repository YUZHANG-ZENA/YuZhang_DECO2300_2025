using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pen : MonoBehaviour
{
    public Color currentColor = Color.black;
    public bool isEraser = false;
    private Whiteboard board;

    void Update()
    {
        if (Input.GetMouseButton(0)) 
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.CompareTag("Whiteboard"))
                {
                    if (board == null)
                        board = hit.collider.GetComponent<Whiteboard>();

                    Vector2 uv = hit.textureCoord;
                    Color drawColor = isEraser ? Color.white : currentColor;
                    board.DrawAtUV(uv, drawColor, 5);
                }
            }
        }
    }
}

