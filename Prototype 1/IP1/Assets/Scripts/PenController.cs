using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenController : MonoBehaviour
{
    public Color currentColor = Color.black;
    public bool isEraser = false;
    public bool isHeld = false;

    private Vector3 offset;
    private Camera cam;
    private Whiteboard board;
    private Vector2? lastUV = null;

    public bool isDrawing = false;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        isDrawing = Input.GetKey(KeyCode.Space);  
    }

    void OnMouseDown()
    {
        isHeld = true;
        Vector3 mouseWorld = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.WorldToScreenPoint(transform.position).z));
        offset = transform.position - mouseWorld;
    }

    void OnMouseDrag()
    {
        if (isHeld)
        {
            Vector3 mouseWorld = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.WorldToScreenPoint(transform.position).z));
            transform.position = mouseWorld + offset;

            if (isDrawing)
            {
                Ray ray = new Ray(transform.position, transform.forward);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    if (hit.collider.CompareTag("Whiteboard"))
                    {
                        if (board == null) board = hit.collider.GetComponent<Whiteboard>();

                        Vector2 uv = hit.textureCoord;
                        Color drawColor = isEraser ? Color.white : currentColor;

                        if (lastUV.HasValue)
                        {
                            Vector2 from = lastUV.Value;
                            Vector2 to = uv;
                            int steps = Mathf.CeilToInt(Vector2.Distance(from, to) * board.texture.width);
                            for (int i = 0; i <= steps; i++)
                            {
                                Vector2 lerpUV = Vector2.Lerp(from, to, i / (float)steps);
                                board.DrawAtUV(lerpUV, drawColor, 5);
                            }
                        }
                        else
                        {
                            board.DrawAtUV(uv, drawColor, 5);
                        }

                        lastUV = uv;
                    }
                }
            }
            else
            {
                lastUV = null; 
            }
        }
    }

    void OnMouseUp()
    {
        isHeld = false;
        lastUV = null;
    }
}




     
 