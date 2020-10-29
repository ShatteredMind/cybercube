using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class MyShaderBehavior : MonoBehaviour
{
    [Tooltip("Number of pixels per 1 unit of size in world coordinates.")]
    [Range(16, 8182)]
    public int textureSize = 64;

    private readonly Color c_color = new Color(0, 0, 0, 0);

    private Material m_material;
    private Texture2D m_texture;
    private bool m_isEnabled = false;

    private object m_lockFlag = new object();
    private float particleSystemTimer;
    private PaintProjectileManager manager;
    private List<ParticleCollisionEvent> collisions;

    [SerializeField] private ParticleSystem partSystem;
    [SerializeField] private Texture2D bloodTexture;

    void Start()
    {
        Renderer renderer = GetComponent<Renderer>();
        collisions = new List<ParticleCollisionEvent>();

        if (null == renderer)
            return;

        foreach (Material material in renderer.materials)
        {
            if (material.shader.name.Contains("Custom/PaintShaders/"))
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

    public void PaintOnColored(Vector2 textureCoord, float[,] splashTexture, Color color)
    {
        MyPaintOn(textureCoord, splashTexture, color);
    }

    public void ChangeParticleSystem(ParticleSystem particleSystem)
    {
        particleSystemTimer = Time.time;
        partSystem = particleSystem;
    }

    private void MyPaintOn(Vector2 textureCoord, float[,] splashTexture, Color targetColor)
    {
        if (m_isEnabled)
        {
            lock (m_lockFlag)
            {
                int reqnx = splashTexture.GetLength(0);
                int reqny = splashTexture.GetLength(1);
                int reqX = (int)(textureCoord.x * textureSize) - (reqnx / 2);
                int reqY = (int)(textureCoord.y * textureSize) - (reqny / 2);
                int right = m_texture.width - 1;
                int bottom = m_texture.height - 1;

                int x = IntMax(reqX, 0);
                int y = IntMax(reqY, 0);
                // int nx = IntMin(x + reqnx, right) - x;
                // int ny = IntMin(y + reqny, bottom) - y;
                int nx = Random.Range(3, 10);
                int ny = Random.Range(4, 7);
                Color[] pixels = m_texture.GetPixels(x, y, nx, ny);
                int counter = 0;

                for (int i = 0; i < nx; ++i)
                {
                    for (int j = 0; j < ny; ++j)
                    {
                        pixels[counter] = targetColor;
                        counter++;

/*                        float currAlpha = splashTexture[i, j];
                        if (currAlpha == 1)
                            pixels[counter] = targetColor;
                        else
                        {
                            Color currColor = pixels[counter];
                            Color newColor = Color.Lerp(currColor, targetColor, currAlpha);
                            newColor.a = pixels[counter].a + currAlpha;
                            pixels[counter] = newColor;
                        }
                        counter++;*/
                    }
                }
                // m_texture.SetPixels32(x, y, nx, ny, pixels);
                m_texture.SetPixels(x, y, nx, ny, pixels);
                // Это страшная залупа и костыль. Но весь код примерно такой
                // glTexSubImage2D must be used
                if (Time.time - particleSystemTimer > 0.666f)
                {
                    particleSystemTimer = 0f;
                    m_texture.Apply(false, false);
                }
            }
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        partSystem.GetCollisionEvents(gameObject, collisions);

        foreach (ParticleCollisionEvent collisionEvent in collisions)
        {
            PaintParticleIntersection(collisionEvent, Color.red);
        }
    }

    private int IntMax(int a, int b)
    {
        return a > b ? a : b;
    }

    private int IntMin(int a, int b)
    {
        return a < b ? a : b;
    }

    private Range Middle(int middle)
    {
        return new Range(middle - 5, middle + 5);
    }

    private void PaintParticleIntersection(ParticleCollisionEvent collisionEvent, Color color)
    {
        RaycastHit hit;
        manager = PaintProjectileManager.GetInstance();
        if (Physics.Raycast(collisionEvent.intersection, manager.GetSphereRay(0), out hit))
        {
            if (hit.collider is MeshCollider)
            {
                PaintOnColored(hit.textureCoord2, manager.GetRandomProjectileSplash(), color);
            }
        }
    }
}
