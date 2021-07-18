using Assets.Scripts;
using Assets.Scripts.Enums;
using Assets.Scripts.Generator;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    public Texture2D[] atlasTextures;
    public static Dictionary<string, Rect> atlasDictionary = new Dictionary<string, Rect>();
    public static Dictionary<string, Chunk> chunks = new Dictionary<string, Chunk>();

    private int columntHeight = 1;
    private int chunkSize = 10;
    private int worldSize = 5;
    Material blockMaterial;

    public static List<BlockType> blockTypes = new List<BlockType>();

    void Start()
    {
        Texture2D atlas = GetTextureAtlas();
        Material material = new Material(Shader.Find("Standard"));
        material.mainTexture = atlas;
        material.SetFloat("_Glossiness", 0);
        material.SetFloat("_SpecularHighlights", 0f);
        this.blockMaterial = material;
        ChunkUtils.GenerateRandomOffset();

        GenerateBlockTypes();
        GenerateWorld();
        StartCoroutine(BuildWorld());
    }

    IEnumerator BuildWorld()
    {
        foreach(KeyValuePair<string, Chunk> chunk in chunks)
        {
            chunk.Value.DrawChunk(chunkSize);

            yield return null;
        }    
    }

    private void GenerateWorld()
    {
        for (int z = 0; z < worldSize; z++)
        {
            for (int x = 0; x < worldSize; x++)
            {
                for (int y = 0; y < columntHeight; y++)
                {
                    Vector3 chunkPosition = new Vector3(
                        x * chunkSize * 0.75f,
                        y * chunkSize * 0.5f,
                        z * chunkSize - z - z * (1f - (float)(0.5 * Math.Sqrt(3) / 2)));
                    string chunkName = GetChunkName(chunkPosition);

                    Chunk chunk = new Chunk(chunkName, chunkPosition, blockMaterial);
                    chunk.chunkObject.transform.parent = this.transform;
                    chunks.Add(chunkName, chunk);
                }
            }
        }

    }

    private string GetChunkName(Vector3 chunkPosition)
    {
        return (int)chunkPosition.x + "_" + 
            (int)chunkPosition.y + "_" +
            (int)chunkPosition.z;
    }

    private Texture2D GetTextureAtlas()
    {
        Texture2D textureAtlas = new Texture2D(8192, 8192);
        Rect[] rectCoordinates = textureAtlas.PackTextures(atlasTextures, 0, 8192, false);
        textureAtlas.Apply();

        for (int i = 0; i < rectCoordinates.Length; i++)
        {
            atlasDictionary.Add(atlasTextures[i].name.ToLower(), rectCoordinates[i]);
        }

        return textureAtlas;
    }

    private void GenerateBlockTypes()
    {
        BlockType air = new BlockType()
        {
            Name = "air",
            IsTransparent = true,
            EverySideSame = true
        };
        air.SideUV = SetBlockTypeUv("air");
        air.TopUV = SetBlockTypeUv("air");
        air.BottomUV = SetBlockTypeUv("air");
        blockTypes.Add(air);



        BlockType dirt = new BlockType()
        {
            Name = "dirt",
            IsTransparent = false,
            EverySideSame = true
        };
        dirt.SideUV = SetBlockTypeUv("dirt");
        dirt.TopUV = SetBlockTypeUv("dirt", BlockSideEnum.TOP);
        dirt.BottomUV = dirt.TopUV;
        blockTypes.Add(dirt);



        BlockType brick = new BlockType()
        {
            Name = "brick",
            IsTransparent = false,
            EverySideSame = true
        };
        brick.SideUV = SetBlockTypeUv("brick");
        brick.TopUV = SetBlockTypeUv("brick", BlockSideEnum.TOP);
        brick.BottomUV = brick.TopUV;
        blockTypes.Add(brick);



        BlockType grass = new BlockType()
        {
            Name = "grass",
            IsTransparent = false,
            EverySideSame = false
        };
        grass.SideUV = SetBlockTypeUv("grass_side");
        grass.TopUV = SetBlockTypeUv("grass", BlockSideEnum.TOP);
        grass.BottomUV = SetBlockTypeUv("dirt", BlockSideEnum.BOTTOM);
        blockTypes.Add(grass);
    }

    private Vector2[] SetBlockTypeUv(string name, BlockSideEnum side = BlockSideEnum.FRONT)
    {
        if (name == "air")
        {
            return new Vector2[4]
            {
                new Vector2(0, 0),
                new Vector2(1, 0),
                new Vector2(0, 1),
                new Vector2(1, 1)
            };
        }
        else if (side != BlockSideEnum.BOTTOM && side != BlockSideEnum.TOP)
        {
            return new Vector2[4]
            {
                new Vector2(
                    atlasDictionary[name].x + (atlasDictionary[name].width * 0.25f),
                    atlasDictionary[name].y + (atlasDictionary[name].height * 0.25f)),
                new Vector2(
                    atlasDictionary[name].x + (atlasDictionary[name].width * 0.75f),
                    atlasDictionary[name].y + (atlasDictionary[name].height * 0.25f)),
                new Vector2(
                    atlasDictionary[name].x + (atlasDictionary[name].width * 0.25f),
                    atlasDictionary[name].y + (atlasDictionary[name].height * 0.75f)),
                new Vector2(
                    atlasDictionary[name].x + (atlasDictionary[name].width * 0.75f),
                    atlasDictionary[name].y + (atlasDictionary[name].height * 0.75f)),
            };
            
        }
        else
        {
            return new Vector2[7]
            {
                new Vector2(
                    atlasDictionary[name].x + (atlasDictionary[name].width * 0.25f),
                    atlasDictionary[name].y),
                new Vector2(
                    atlasDictionary[name].x + (atlasDictionary[name].width * 0.75f),
                    atlasDictionary[name].y),
                new Vector2(
                    atlasDictionary[name].x,
                    atlasDictionary[name].y + (atlasDictionary[name].height * 0.5f)),
                new Vector2(
                    atlasDictionary[name].x + (atlasDictionary[name].width),
                    atlasDictionary[name].y + (atlasDictionary[name].height * 0.5f)),
                new Vector2(
                    atlasDictionary[name].x + (atlasDictionary[name].width * 0.25f),
                    atlasDictionary[name].y + (atlasDictionary[name].height)),
                new Vector2(
                    atlasDictionary[name].x + (atlasDictionary[name].width * 0.75f),
                    atlasDictionary[name].y + (atlasDictionary[name].height)),
                new Vector2(
                    atlasDictionary[name].x + (atlasDictionary[name].width * 0.5f),
                    atlasDictionary[name].y + (atlasDictionary[name].height * 0.5f))
            };
        }
        
    }
}
