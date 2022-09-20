using Assets.Scripts.Generator;
using UnityEngine;
using UnityEngine.UI;


public class WorldGenerator : MonoBehaviour
{
    [Header("Generowanie wody")]
    [Range(0f, 1f)]
    [SerializeField]
    private float waterSpread;

    [Range(1, 500)]
    [SerializeField]
    private int waterAreaSize;

    [Header("Temperatura")]
    [Range(-0.5f, 0.5f)]
    [SerializeField]
    private float extraTemperature;
    [Range(0f, 1f)]
    [SerializeField]
    private float heightImpact;

    [Header("Generowanie biomów")]
    [Range(1f, 200f)]
    [SerializeField]
    private float biomeSize;

    [Header("Obiekty")]
    [SerializeField] 
    private Button generateWorldButton;
    [SerializeField] 
    private Image canvasImage;
    
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
                Color biomeColor = BiomeUtils.SelectBiomeColor(
                    y * zoom, 
                    x * zoom,
                    waterSpread,
                    waterAreaSize,
                    extraTemperature,
                    heightImpact,
                    biomeSize);


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