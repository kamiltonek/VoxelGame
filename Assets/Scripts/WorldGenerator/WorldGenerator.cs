using Assets.Scripts;
using Assets.Scripts.Enums;
using Assets.Scripts.Generator;
using System;
using UnityEngine;
using UnityEngine.UI;


public class WorldGenerator : MonoBehaviour
{
    private static Color desert = new Color(255 / 255f, 145 / 255f, 0, 255);
    private static Color grassland = new Color(141 / 255f, 179 / 255f, 96 / 255f, 255);
    private static Color jungle = new Color(83 / 255f, 123 / 255f, 9 / 255f, 255);
    private static Color savanna = new Color(189 / 255f, 178 / 255f, 95 / 255f, 255);
    private static Color snow = new Color(245 / 255f, 249 / 255f, 255 / 255f, 255);
    private static Color tajga = new Color(7 / 255f, 249 / 255f, 178 / 255f, 255);
    private static Color tundra = new Color(11 / 255f, 102 / 255f, 89 / 255f, 255);
    private static Color water = new Color(0 / 255f, 136 / 255f, 255 / 255f, 255);
    private static Color ice = new Color(122 / 255f, 173 / 255f, 255 / 255f, 255);
    private static Color river = new Color(0 / 255f, 0 / 255f, 0 / 255f, 255);
    private static Color beach = new Color(250 / 255f, 222 / 255f, 85 / 255f, 255);

    [SerializeField] private Button generateWorldButton;
    [SerializeField] private Image canvasImage;
    private Texture2D texture;
    private int tileSize = 1;
    private int zoom = 1;
    
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
                Biome biome = BiomeUtils.SelectBiome(y * zoom, x * zoom);
                Color biomeColor = new Color();

                if(biome.GetBiomeName() == BiomeNameEnum.GRASSLAND)
                {
                    biomeColor = grassland;
                }
                if (biome.GetBiomeName() == BiomeNameEnum.DESERT)
                {
                    biomeColor = desert;
                }
                if (biome.GetBiomeName() == BiomeNameEnum.ICE)
                {
                    biomeColor = ice;
                }
                if (biome.GetBiomeName() == BiomeNameEnum.JUNGLE)
                {
                    biomeColor = jungle;
                }
                if (biome.GetBiomeName() == BiomeNameEnum.SAVANNA)
                {
                    biomeColor = savanna;
                }
                if (biome.GetBiomeName() == BiomeNameEnum.SNOW_BIOME)
                {
                    biomeColor = snow;
                }
                if (biome.GetBiomeName() == BiomeNameEnum.TAJGA)
                {
                    biomeColor = tajga;
                }
                if (biome.GetBiomeName() == BiomeNameEnum.TUNDRA)
                {
                    biomeColor = tundra;
                }
                if (biome.GetBiomeName() == BiomeNameEnum.WATER)
                {
                    biomeColor = water;
                }
                if (biome.GetBiomeName() == BiomeNameEnum.RIVER)
                {
                    biomeColor = river;
                }
                if (biome.GetBiomeName() == BiomeNameEnum.BEACH)
                {
                    biomeColor = beach;
                }

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