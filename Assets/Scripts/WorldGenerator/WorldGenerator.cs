using Assets.Scripts.Generator;
using UnityEngine;
using UnityEngine.UI;


public class WorldGenerator : MonoBehaviour
{
    [SerializeField] private Button generateWorldButton;
    [SerializeField] private Image canvasImage;
    private Texture2D texture;
    private int tileSize = 8;
    private int zoom = 16;
    
    void Start()
    {
        texture = new Texture2D(1920, 1080);
        canvasImage.material.mainTexture = texture;
        generateWorldButton.onClick.AddListener(DrawWorld);

        DrawWorld();
    }

    public void DrawWorld()
    {
        ChunkUtils.GenerateRandomOffset();

        for (int y = 0; y < texture.height / tileSize; y++)
        {
            for (int x = 0; x < texture.width / tileSize; x++)
            {
                Color biomeColor = BiomeUtils.SelectBiomeColor(y * zoom, x * zoom);

                for (int i = 0; i < tileSize; i++)
                {
                    for (int j = 0; j < tileSize; j++)
                    {
                        texture.SetPixel(x * tileSize + i, y * tileSize + j, biomeColor);
                    }
                }
            }
        }
        texture.Apply();
    }

}