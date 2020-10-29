using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class PaintedSurface : MonoBehaviour
{
    [Tooltip("Number of pixels per 1 unit of size in world coordinates.")]
    [Range(16, 8182)]
    public int textureSize = 64;

    private readonly Color c_color = new Color(1, 1, 1, 0);

    private Material m_material;
    private Texture2D m_texture;
    private bool m_isEnabled = true;

    private object m_lockFlag = new object();

    void Start()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (null == renderer)
            return;

        foreach (Material material in renderer.materials)
        {
            if (material.shader.name.Contains("Custom"))
            {
                m_material = material;
                break;
            }
        }

        if (null != m_material)
        {
            m_texture = new Texture2D(textureSize, textureSize);
            for (int x = 0; x < textureSize; ++x)
                for (int y = 0; y < textureSize; ++y)
                    m_texture.SetPixel(x, y, c_color);
            m_texture.Apply();

            m_material.SetTexture("_DrawingTex", m_texture);
            m_isEnabled = true;
        }
    }

    public void PaintOnColored(Vector2 textureCoord, Texture2D splashTexture, Color color)
    {
        MyPaintOn(textureCoord, splashTexture, color);
    }

    private void MyPaintOn(Vector2 textureCoord, Texture2D splashTexture, Color targetColor)
    {
        if (m_isEnabled)
        {
            int x = (int)(textureCoord.x * textureSize) - (splashTexture.width / 2);
            int y = (int)(textureCoord.y * textureSize) - (splashTexture.height / 2);

            for (int i = 0; i < splashTexture.width; ++i)
                for (int j = 0; j < splashTexture.height; j++)
                {
                    Color result = Color.Lerp(c_color, targetColor, 1);   // resulting color is an addition of splash texture to the texture based on alpha
                    result.a = targetColor.a + 1;                             // but resulting alpha is a sum of alphas (adding transparent color should not make base color more transparent)
                    m_texture.SetPixel(x, y, result);
                }
        }
        m_texture.Apply();
    }

    private int IntMax(int a, int b)
    {
        return a > b ? a : b;
    }

    private int IntMin(int a, int b)
    {
        return a < b ? a : b;
    }
}