using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteboardDraggable : MonoBehaviour
{
    [Header("DraggingSettings")]
    public float holdTime = 0.5f;           
    public LayerMask draggableLayer = -1; 
    
    private bool isHolding = false;
    private bool isDragging = false;
    private float holdTimer = 0f;
    private Vector3 lastMousePosition;
    private Vector3 dragOffset;
    private Camera cam;
    private PenController activePen;
    
    void Start()
    {
        cam = Camera.main;
    }
    
    void Update()
    {
        HandleInput();
    }
    
    void HandleInput()
    {
 
        if (Input.GetMouseButtonDown(0))
        {
            StartHolding();
        }
        
        if (Input.GetMouseButton(0))
        {
            if (isHolding && !isDragging)
            {
                holdTimer += Time.deltaTime;
                
                if (holdTimer >= holdTime && !IsPenActive())
                {
                    TryStartDragging();
                }
            }
            
            if (isDragging)
            {
                PerformDrag();
            }
        }
        
        if (Input.GetMouseButtonUp(0))
        {
            StopDragging();
        }
    }
    
    void StartHolding()
    {
        isHolding = true;
        holdTimer = 0f;
        lastMousePosition = Input.mousePosition;
    }
    
    void TryStartDragging()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, draggableLayer))
        {
            if (hit.collider.CompareTag("Whiteboard"))
            {
                isDragging = true;
                
                Vector3 mouseWorldPos = cam.ScreenToWorldPoint(
                    new Vector3(Input.mousePosition.x, Input.mousePosition.y, 
                    cam.WorldToScreenPoint(hit.transform.position).z));
                
                dragOffset = hit.transform.position - mouseWorldPos;
                
                Debug.Log("StartDraggingWhiteboard");
            }
        }
    }
    
    void PerformDrag()
    {
        Vector3 mouseWorldPos = cam.ScreenToWorldPoint(
            new Vector3(Input.mousePosition.x, Input.mousePosition.y,
            cam.WorldToScreenPoint(transform.position).z));
            
        transform.position = mouseWorldPos + dragOffset;
    }
    
    void StopDragging()
    {
        isHolding = false;
        isDragging = false;
        holdTimer = 0f;
        
        if (isDragging)
        {
            Debug.Log("StopDraggingWhiteboard");
        }
    }
    
    bool IsPenActive()
    {
        if (activePen == null)
            activePen = FindObjectOfType<PenController>();
            
        return activePen != null && activePen.isHeld;
    }
    
    void OnDrawGizmos()
    {
        if (isDragging)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(transform.position, transform.localScale);
        }
    }
}
