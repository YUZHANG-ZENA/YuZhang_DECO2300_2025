This document is a place to paste raw prompts and references for a particular activity. This allows for better referencing in code and documentaion.

---

Me:

"The current corner handle is not flexible when dragging the corner, it's hard to control, can you help me?"

Claude:

using UnityEngine;

public class SimpleCornerHandle : MonoBehaviour
{
    [Header("Corner Settings")]
    public CornerType cornerType;
    public Transform whiteboard;        // Whiteboard Transform
    public float minSize = 0.5f;       // Minimum size
    public float maxSize = 5f;         // Maximum size
    
    private bool isDragging = false;
    private Vector3 startMousePos;
    private Vector3 startWhiteboardPos;
    private Vector3 startWhiteboardScale;
    private Camera cam;
    private Vector3 oppositeCorner; // Opposite corner world coordinates
    
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
        
        // Automatically find whiteboard if not set
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
        
        // Calculate opposite corner coordinates
        CalculateOppositeCorner();
    }
    
    void OnMouseDrag()
    {
        if (!isDragging) return;
        
        Vector3 currentMousePos = Input.mousePosition;
        
        // Convert mouse movement to world coordinate movement
        Vector3 mouseWorldStart = cam.ScreenToWorldPoint(new Vector3(startMousePos.x, startMousePos.y, 
            cam.WorldToScreenPoint(whiteboard.position).z));
        Vector3 mouseWorldCurrent = cam.ScreenToWorldPoint(new Vector3(currentMousePos.x, currentMousePos.y, 
            cam.WorldToScreenPoint(whiteboard.position).z));
        Vector3 worldDelta = mouseWorldCurrent - mouseWorldStart;
        
        // Calculate new size and position based on corner type
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
        // Get current corner position in world space
        Vector3 currentCornerPos = GetCornerWorldPosition() + worldDelta;
        
        // Calculate new center point (midpoint of diagonal)
        Vector3 newCenter = (oppositeCorner + currentCornerPos) / 2f;
        
        // Calculate new size
        Vector3 sizeVector = currentCornerPos - oppositeCorner;
        Vector3 newScale = new Vector3(
            Mathf.Abs(sizeVector.x),
            Mathf.Abs(sizeVector.y),
            startWhiteboardScale.z
        );
        
        // Clamp to minimum and maximum size
        newScale.x = Mathf.Clamp(newScale.x, minSize, maxSize);
        newScale.y = Mathf.Clamp(newScale.y, minSize, maxSize);
        
        // Apply new position and size
        whiteboard.position = newCenter;
        whiteboard.localScale = newScale;
        
        // Update all corner handle positions
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
        // Update all corner handle positions
        SimpleCornerHandle[] allHandles = whiteboard.GetComponentsInChildren<SimpleCornerHandle>();
        
        foreach (var handle in allHandles)
        {
            handle.transform.position = handle.GetCornerWorldPosition();
        }
    }
}