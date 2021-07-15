using Assets.Scripts.Enums;
using System;
using System.Collections;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    public Material blockMaterial;
    public Block[,,] chunkBlocks;
    void Start()
    {
        StartCoroutine(GenerateChunk(10));
    }

    IEnumerator GenerateChunk(int chunkSize)
    {
        chunkBlocks = new Block[chunkSize, chunkSize, chunkSize];

        for (int z = 0; z < chunkSize; z++)
        {
            for (int y = 0; y < chunkSize; y++)
            {
                for (int x = 0; x < chunkSize; x++)
                {
                    bool oddX = x % 2 == 0;
                    float zOffset = oddX ? 0 : (float)(0.5 * Math.Sqrt(3) / 2);
                    
                    float posX = x - (x * 0.25f);
                    float posY = y * 0.5f;
                    float posZ = z - zOffset - z * (1f - (float)(0.5 * Math.Sqrt(3)));

                    chunkBlocks[x, y, z] = new Block(
                        BlockTypeEnum.DIRT, 
                        this.gameObject,
                        new Vector3(posX, posY, posZ),
                        new Vector3Int(x, y, z), 
                        blockMaterial);
                }
            }
        }

        for (int z = 0; z < chunkSize; z++)
        {
            for (int y = 0; y < chunkSize; y++)
            {
                for (int x = 0; x < chunkSize; x++)
                {
                    chunkBlocks[x, y, z].CreateBlock();
                }
            }
        }

        CombineSides();
        yield return null;
    }

    private void CombineSides()
    {
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combineSides = new CombineInstance[meshFilters.Length];

        int index = 0;
        foreach (MeshFilter meshFilter in meshFilters)
        {
            combineSides[index].mesh = meshFilter.sharedMesh;
            combineSides[index].transform = meshFilter.transform.localToWorldMatrix;
            index++;
        }

        MeshFilter blockMeshFilter = (MeshFilter)this.gameObject.AddComponent(typeof(MeshFilter));
        blockMeshFilter.mesh = new Mesh();
        blockMeshFilter.mesh.CombineMeshes(combineSides);

        MeshRenderer blockMeshRenderer = (MeshRenderer)this.gameObject.AddComponent(typeof(MeshRenderer));
        blockMeshRenderer.material = blockMaterial;

        foreach (Transform side in this.transform)
        {
            Destroy(side.gameObject);
        }
    }
}
