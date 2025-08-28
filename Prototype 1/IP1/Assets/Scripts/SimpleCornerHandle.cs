using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCornerHandle : MonoBehaviour
{
    [Header("CornerSettings")]
    public CornerType cornerType;
    public Transform whiteboard;        
    public float minSize = 0.5f;      
    public float maxSize = 5f;        
    
    private bool isDragging = false;
    private Vector3 startMousePos;
    private Vector3 startWhiteboardPos;
    private Vector3 startWhiteboardScale;
    private Camera cam;
    private Vector3 oppositeCorner; 
    
    public enum CornerType
    {
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight
    }
    
    void Start()
    {
        cam = Camera.main;
        
        if (whiteboard == null)
        {
            whiteboard = transform.parent;
        }
    }
    
    void OnMouseDown()
    {
        isDragging = true;
        startMousePos = Input.mousePosition;
        startWhiteboardPos = whiteboard.position;
        startWhiteboardScale = whiteboard.localScale;
        
        CalculateOppositeCorner();
    }
    
    void OnMouseDrag()
    {
        if (!isDragging) return;
        
        Vector3 currentMousePos = Input.mousePosition;
        
        Vector3 mouseWorldStart = cam.ScreenToWorldPoint(new Vector3(startMousePos.x, startMousePos.y, 
            cam.WorldToScreenPoint(whiteboard.position).z));
        Vector3 mouseWorldCurrent = cam.ScreenToWorldPoint(new Vector3(currentMousePos.x, currentMousePos.y, 
            cam.WorldToScreenPoint(whiteboard.position).z));
        Vector3 worldDelta = mouseWorldCurrent - mouseWorldStart;
        
        ResizeFromCorner(worldDelta);
    }
    
    void OnMouseUp()
    {
        isDragging = false;
    }
    
    void CalculateOppositeCorner()
    {
        Vector3 scale = whiteboard.localScale;
        Vector3 pos = whiteboard.position;
        
        switch (cornerType)
        {
            case CornerType.TopLeft:
                oppositeCorner = pos + new Vector3(scale.x/2, -scale.y/2, 0);
                break;
            case CornerType.TopRight:
                oppositeCorner = pos + new Vector3(-scale.x/2, -scale.y/2, 0);
                break;
            case CornerType.BottomLeft:
                oppositeCorner = pos + new Vector3(scale.x/2, scale.y/2, 0);
                break;
            case CornerType.BottomRight:
                oppositeCorner = pos + new Vector3(-scale.x/2, scale.y/2, 0);
                break;
        }
    }
    
    void ResizeFromCorner(Vector3 worldDelta)
    {
        Vector3 currentCornerPos = GetCornerWorldPosition() + worldDelta;
        
        Vector3 newCenter = (oppositeCorner + currentCornerPos) / 2f;
        
        Vector3 sizeVector = currentCornerPos - oppositeCorner;
        Vector3 newScale = new Vector3(
            Mathf.Abs(sizeVector.x),
            Mathf.Abs(sizeVector.y),
            startWhiteboardScale.z
        );
        
        newScale.x = Mathf.Clamp(newScale.x, minSize, maxSize);
        newScale.y = Mathf.Clamp(newScale.y, minSize, maxSize);
        
        whiteboard.position = newCenter;
        whiteboard.localScale = newScale;
        
        UpdateAllHandlePositions();
    }
    
    Vector3 GetCornerWorldPosition()
    {
        Vector3 scale = whiteboard.localScale;
        Vector3 pos = whiteboard.position;
        
        switch (cornerType)
        {
            case CornerType.TopLeft:
                return pos + new Vector3(-scale.x/2, scale.y/2, 0);
            case CornerType.TopRight:
                return pos + new Vector3(scale.x/2, scale.y/2, 0);
            case CornerType.BottomLeft:
                return pos + new Vector3(-scale.x/2, -scale.y/2, 0);
            case CornerType.BottomRight:
                return pos + new Vector3(scale.x/2, -scale.y/2, 0);
            default:
                return pos;
        }
    }
    
    void UpdateAllHandlePositions()
    {
        SimpleCornerHandle[] allHandles = whiteboard.GetComponentsInChildren<SimpleCornerHandle>();
        
        foreach (var handle in allHandles)
        {
            handle.transform.position = handle.GetCornerWorldPosition();
        }
    }
}
