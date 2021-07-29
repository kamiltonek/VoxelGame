using Assets.Scripts;
using Assets.Scripts.Enums;
using Assets.Scripts.Generator;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class World : MonoBehaviour
{
    public Texture2D[] atlasTextures;
    public static Dictionary<string, Rect> atlasDictionary = new Dictionary<string, Rect>();
    public static Dictionary<string, Chunk> chunks = new Dictionary<string, Chunk>();

    private float updatePlayerPositionDelay = 0.5f;
    private int columnHeight = 1;
    private int chunkSize = 10;
    private int worldRadius = 5;
    Material blockMaterial;

    GameObject player;
    Vector2 lastPlayerPosition;
    Vector2 currentPlayerPosition;

    public static Dictionary<BlockName, BlockType> blockTypes = new Dictionary<BlockName, BlockType>();

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        UpdatePlayerPosition();
    }

    private void Start()
    {
        player.SetActive(false);
        Texture2D atlas = GetTextureAtlas();
        Material material = new Material(Shader.Find("Standard"));
        material.mainTexture = atlas;
        material.SetFloat("_Glossiness", 0);
        material.SetFloat("_SpecularHighlights", 0f);
        this.blockMaterial = material;
        ChunkUtils.GenerateRandomOffset();

        GenerateBlockTypes();
        GenerateWorld();
        StartCoroutine(BuildWorld(true));
    }

    private void Update()
    {
        updatePlayerPositionDelay -= Time.deltaTime;

        if(updatePlayerPositionDelay <= 0)
        {
            UpdatePlayerPosition();
            updatePlayerPositionDelay = 0.5f;
        }  

        if (currentPlayerPosition != lastPlayerPosition)
        {
            lastPlayerPosition = currentPlayerPosition;
            GenerateWorld();
            StartCoroutine(BuildWorld());
        }
    }

    IEnumerator BuildWorld(bool isFirst = false)
    {
        foreach(Chunk chunk in chunks.Values.ToList())
        {
            if(chunk.status == ChunkStatusEnum.TO_DRAW)
            {
                chunk.DrawChunk(chunkSize);
            }

            yield return null;
        }

        if (isFirst)
        {
            player.SetActive(true);
        }
    }

    private void GenerateWorld()
    {
        for (int z = -worldRadius + (int)currentPlayerPosition.y - 2; z <= worldRadius + (int)currentPlayerPosition.y + 2; z++)
        {
            for (int x = -worldRadius + (int)currentPlayerPosition.x - 2; x <= worldRadius + (int)currentPlayerPosition.x + 2; x++)
            {
                for (int y = 0; y < columnHeight; y++)
                {
                    Vector3 chunkPosition = new Vector3(
                        x * chunkSize * 0.75f,
                        y * chunkSize * 0.5f,
                        z * chunkSize * (float)(Math.Sqrt(3))/2);
                    string chunkName = GetChunkName(x, y, z);
                    Chunk chunk;

                    if (z == -worldRadius + (int)currentPlayerPosition.y - 2 ||
                       z == worldRadius + (int)currentPlayerPosition.y + 2 ||
                       x == -worldRadius + (int)currentPlayerPosition.x - 2 ||
                       x == worldRadius + (int)currentPlayerPosition.x + 2)
                    {
                        if (chunks.TryGetValue(chunkName, out chunk))
                        {
                            chunk.status = ChunkStatusEnum.GENERATED;
                            Destroy(chunk.chunkObject);
                        }

                        continue;
                    }

                    if (z == -worldRadius + (int)currentPlayerPosition.y - 1 ||
                       z == worldRadius + (int)currentPlayerPosition.y + 1 ||
                       x == -worldRadius + (int)currentPlayerPosition.x - 1 ||
                       x == worldRadius + (int)currentPlayerPosition.x + 1)
                    {

                        continue;
                    }

                    if(chunks.TryGetValue(chunkName, out chunk))
                    {
                        if(chunk.status == ChunkStatusEnum.GENERATED)
                        {
                            chunk.RefreshChunk(chunkName, chunkPosition);
                            chunk.chunkObject.transform.parent = this.transform;
                        }
                    }
                    else
                    {
                        chunk = new Chunk(chunkName, chunkPosition, blockMaterial);
                        chunk.chunkObject.transform.parent = this.transform;
                        chunks.Add(chunkName, chunk);
                    }

                }
            }
        }

    }

    private string GetChunkName(int x, int y, int z)
    {
        return x + "_" + y + "_" + z;
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
        blockTypes.Add(BlockName.AIR, air);



        BlockType dirt = new BlockType()
        {
            Name = "dirt",
            IsTransparent = false,
            EverySideSame = true
        };
        dirt.SideUV = SetBlockTypeUv("dirt");
        dirt.TopUV = SetBlockTypeUv("dirt", BlockSideEnum.TOP);
        dirt.BottomUV = dirt.TopUV;
        blockTypes.Add(BlockName.DIRT, dirt);



        BlockType brick = new BlockType()
        {
            Name = "brick",
            IsTransparent = false,
            EverySideSame = true
        };
        brick.SideUV = SetBlockTypeUv("brick");
        brick.TopUV = SetBlockTypeUv("brick", BlockSideEnum.TOP);
        brick.BottomUV = brick.TopUV;
        blockTypes.Add(BlockName.BRICK, brick);



        BlockType grass = new BlockType()
        {
            Name = "grass",
            IsTransparent = false,
            EverySideSame = false
        };
        grass.SideUV = SetBlockTypeUv("grass_side");
        grass.TopUV = SetBlockTypeUv("grass", BlockSideEnum.TOP);
        grass.BottomUV = SetBlockTypeUv("dirt", BlockSideEnum.BOTTOM);
        blockTypes.Add(BlockName.GRASS, grass);



        BlockType snow = new BlockType()
        {
            Name = "snow",
            IsTransparent = false,
            EverySideSame = true
        };
        snow.SideUV = SetBlockTypeUv("snow");
        snow.TopUV = SetBlockTypeUv("snow", BlockSideEnum.TOP);
        snow.BottomUV = snow.TopUV;
        blockTypes.Add(BlockName.SNOW, snow);



        BlockType sand = new BlockType()
        {
            Name = "sand",
            IsTransparent = false,
            EverySideSame = true
        };
        sand.SideUV = SetBlockTypeUv("sand");
        sand.TopUV = SetBlockTypeUv("sand", BlockSideEnum.TOP);
        sand.BottomUV = sand.TopUV;
        blockTypes.Add(BlockName.SAND, sand);
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
    private void UpdatePlayerPosition()
    {
        currentPlayerPosition.x = Mathf.Floor(player.transform.position.x / 7.5f);
        currentPlayerPosition.y = Mathf.Floor(player.transform.position.z / (10 * (float)Math.Sqrt(3) / 2));
    }
}
