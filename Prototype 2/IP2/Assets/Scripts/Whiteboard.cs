using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whiteboard : MonoBehaviour
{
    public Texture2D texture;
    public Vector2 textureSize = new Vector2(2048, 2048);
    public Color backgroundColor = Color.white; 

    void Start()
    {
        var r = GetComponent<Renderer>();
        texture = new Texture2D((int)textureSize.x, (int)textureSize.y);
        
        ClearBoard();
        
        r.material.mainTexture = texture;
    }

    public void ClearBoard()
    {
        Color[] backgroundPixels = new Color[(int)textureSize.x * (int)textureSize.y];
        for (int i = 0; i < backgroundPixels.Length; i++)
        {
            backgroundPixels[i] = backgroundColor;
        }
        texture.SetPixels(backgroundPixels);
        texture.Apply();
    }
}
