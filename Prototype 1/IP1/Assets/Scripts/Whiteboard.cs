using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whiteboard : MonoBehaviour
{
    public Texture2D texture;
    private Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
        texture = new Texture2D(1024, 1024, TextureFormat.RGBA32, false);
        ClearBoard();
        rend.material.mainTexture = texture;
    }

    public void DrawAtUV(Vector2 uv, Color color, int penSize = 5)
    {
        int x = (int)(uv.x * texture.width);
        int y = (int)(uv.y * texture.height);

        for (int i = -penSize; i <= penSize; i++)
        {
            for (int j = -penSize; j <= penSize; j++)
            {
                if (i * i + j * j <= penSize * penSize)
                {
                    int px = x + i;
                    int py = y + j;

                    if (px >= 0 && px < texture.width && py >= 0 && py < texture.height)
                    {
                        texture.SetPixel(px, py, color);
                    }
                }
            }
        }
        texture.Apply();
    }

    public void ClearBoard()
    {
        Color[] fill = new Color[texture.width * texture.height];
        for (int i = 0; i < fill.Length; i++) fill[i] = Color.white;
        texture.SetPixels(fill);
        texture.Apply();
    }
    public Texture2D CopyRegion(Rect uvRect)
    {
        int x = Mathf.RoundToInt(uvRect.x * texture.width);
        int y = Mathf.RoundToInt(uvRect.y * texture.height);
        int w = Mathf.RoundToInt(uvRect.width * texture.width);
        int h = Mathf.RoundToInt(uvRect.height * texture.height);

        Texture2D newTex = new Texture2D(w, h, TextureFormat.RGBA32, false);
        Color[] pixels = texture.GetPixels(x, y, w, h);
        newTex.SetPixels(pixels);
        newTex.Apply();

        return newTex;
    }

    public void PasteToBoard(Texture2D region, Vector2 uv)
{
    int x = (int)(uv.x * texture.width);
    int y = (int)(uv.y * texture.height);

    for (int i = 0; i < region.width; i++)
    {
        for (int j = 0; j < region.height; j++)
        {
            Color c = region.GetPixel(i, j);
            if (c.a > 0.01f) 
            {
                int px = x + i;
                int py = y + j;
                if (px >= 0 && px < texture.width && py >= 0 && py < texture.height)
                {
                    texture.SetPixel(px, py, c);
                }
            }
        }
    }
    texture.Apply();
}



    
}

