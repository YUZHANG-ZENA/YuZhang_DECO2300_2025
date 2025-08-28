using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteboardDragger : MonoBehaviour
{
    public Transform whiteboard; 
    public float dragSmooth = 10f; 
    private bool isDragging = false;

    private Plane dragPlane;
    private Vector3 offset;
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void OnMouseDown()
    {
        dragPlane = new Plane(-cam.transform.forward, whiteboard.position);

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (dragPlane.Raycast(ray, out float enter))
        {
            Vector3 hitPoint = ray.GetPoint(enter);
            offset = whiteboard.position - hitPoint;
        }

        isDragging = true;
    }

    void OnMouseDrag()
    {
        if (!isDragging) return;

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (dragPlane.Raycast(ray, out float enter))
        {
            Vector3 hitPoint = ray.GetPoint(enter);
            Vector3 targetPos = hitPoint + offset;

            whiteboard.position = Vector3.Lerp(whiteboard.position, targetPos, Time.deltaTime * dragSmooth);
        }
    }

    void OnMouseUp()
    {
        isDragging = false;
    }
}

