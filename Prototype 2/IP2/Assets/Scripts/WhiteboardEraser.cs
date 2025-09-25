using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;

public class WhiteboardEraser : MonoBehaviour
{
    [SerializeField] private Transform _tip;
    [SerializeField] private int _eraserSize = 10;
    [SerializeField] private Color _backgroundColor = Color.white; 

    private Color[] _eraseColors;
    private float _tipHeight;

    private RaycastHit _touch;
    private Whiteboard _whiteboard;
    private Vector2 _touchPos, _lastTouchPos;
    private bool _touchedLastFrame;
    private Quaternion _lastTouchRot;
    
    void Start()
    {
        _tipHeight = _tip.localScale.y;
        _eraseColors = Enumerable.Repeat(_backgroundColor, _eraserSize * _eraserSize).ToArray();
    }

    void Update()
    {
        Erase();
    }

    private void Erase()
    {
        if (Physics.Raycast(_tip.position, transform.up, out _touch, _tipHeight))
        {
            if (_touch.transform.CompareTag("Whiteboard"))
            {
                if (_whiteboard == null)
                {
                    _whiteboard = _touch.transform.GetComponent<Whiteboard>();
                }

                _touchPos = new Vector2(_touch.textureCoord.x, _touch.textureCoord.y);

                var x = (int)(_touchPos.x * _whiteboard.textureSize.x - (_eraserSize / 2));
                var y = (int)(_touchPos.y * _whiteboard.textureSize.y - (_eraserSize / 2));

                if (y < 0 || y > _whiteboard.textureSize.y || x < 0 || x > _whiteboard.textureSize.x) return;

                if (_touchedLastFrame)
                {
                    _whiteboard.texture.SetPixels(x, y, _eraserSize, _eraserSize, _eraseColors);

                    for (float f = 0.01f; f < 1.00f; f += 0.01f)
                    {
                        var lerpX = (int)Mathf.Lerp(_lastTouchPos.x, x, f);
                        var lerpY = (int)Mathf.Lerp(_lastTouchPos.y, y, f);
                        _whiteboard.texture.SetPixels(lerpX, lerpY, _eraserSize, _eraserSize, _eraseColors);
                    }

                    transform.rotation = _lastTouchRot;
                    _whiteboard.texture.Apply();
                }

                _lastTouchPos = new Vector2(x, y);
                _lastTouchRot = transform.rotation;
                _touchedLastFrame = true;
                return;
            }
        }

        _whiteboard = null;
        _touchedLastFrame = false;
    }

    public void SetBackgroundColor(Color color)
    {
        _backgroundColor = color;
        _eraseColors = Enumerable.Repeat(_backgroundColor, _eraserSize * _eraserSize).ToArray();
    }

    public void SetEraserSize(int size)
    {
        _eraserSize = Mathf.Max(1, size);
        _eraseColors = Enumerable.Repeat(_backgroundColor, _eraserSize * _eraserSize).ToArray();
    }
}
