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

    void Start()
    {
        Texture2D atlas = GetTextureAtlas();
        Material material = new Material(Shader.Find("Standard"));
        material.mainTexture = atlas;
        material.SetFloat("_Glossiness", 0);
        material.SetFloat("_SpecularHighlights", 0f);
        this.blockMaterial = material;
        StartCoroutine(BuildWorld());
    }

    IEnumerator BuildWorld()
    {
        for (int i = 0; i < columntHeight; i++)
        {
            
        }

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

        foreach(KeyValuePair<string, Chunk> chunk in chunks)
        {
            chunk.Value.DrawChunk(chunkSize);

            yield return null;
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
}
