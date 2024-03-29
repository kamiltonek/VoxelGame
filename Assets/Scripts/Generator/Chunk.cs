﻿using Assets.Scripts;
using Assets.Scripts.Enums;
using Assets.Scripts.Generator;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Chunk
{
    public Block[,,] chunkBlocks;
    public GameObject chunkObject;
    public ChunkStatusEnum status;

    private Material[] blockMaterials;
    public int vertexIndex { get; set; }

    public List<Vector3> vertices = new List<Vector3>();
    public List<int> triangles = new List<int>();
    public List<int> transparentTriangles = new List<int>();
    public List<int> liquidTriangles = new List<int>();
    public List<Vector2> uvs = new List<Vector2>();

    public Chunk(string name, Vector3 position, Material[] material)
    {
        this.chunkObject = new GameObject(name);
        this.chunkObject.transform.position = position;
        this.blockMaterials = material;
        this.status = ChunkStatusEnum.GENERATED;
        GenerateChunk(10);
    }

    private void GenerateChunk(int chunkSize)
    {
        chunkBlocks = new Block[chunkSize, chunkSize, chunkSize];

        for (int z = 0; z < chunkSize; z++)
        {
            for (int x = 0; x < chunkSize; x++)
            {
                for (int y = 0; y < chunkSize; y++)
                {
                    bool oddX = x % 2 == 0;
                    float zOffset = oddX ? 0 : (float)(0.5 * Math.Sqrt(3) / 2);

                    float posX = x * 0.75f;
                    float posY = y * 0.5f;
                    float posZ = z - zOffset - z * (1f - (float)(0.5 * Math.Sqrt(3)));

                    float worldX = posX + chunkObject.transform.position.x;
                    float worldY = posY + chunkObject.transform.position.y;
                    float worldZ = posZ + chunkObject.transform.position.z;

                    Biome biome = BiomeUtils.SelectBiome(worldX, worldZ);

                    BlockType biomeBlock = biome.GenerateTerrain(worldX, worldY, worldZ);

                    chunkBlocks[x, y, z] = new Block(
                           biomeBlock,
                           this,
                           new Vector3(posX, posY, posZ),
                           new Vector3Int(x, y, z));
                }
            }  
        }

        status = ChunkStatusEnum.TO_DRAW;

    }
    public void RefreshChunk(string chunkName, Vector3 chunkPosition)
    {
        this.chunkObject = new GameObject(chunkName);
        this.chunkObject.transform.position = chunkPosition;

        status = ChunkStatusEnum.TO_DRAW;
    }

    public void DrawChunk(int chunkSize)
    {
        vertexIndex = 0;

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

        status = ChunkStatusEnum.DRAWN;
    }

    private void CombineSides()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = vertices.ToArray();
        mesh.subMeshCount = 2;
        mesh.SetTriangles(triangles.ToArray(), 0);
        mesh.SetTriangles(transparentTriangles.ToArray().Concat(liquidTriangles.ToArray()).ToArray(), 1);
        mesh.uv = uvs.ToArray();
        mesh.RecalculateNormals();

        var colliderMesh = new Mesh();
        colliderMesh.vertices = vertices.ToArray();
        colliderMesh.triangles = triangles.ToArray().Concat(transparentTriangles.ToArray()).ToArray();

        MeshFilter blockMeshFilter = (MeshFilter)chunkObject.AddComponent(typeof(MeshFilter));
        blockMeshFilter.mesh = mesh;

        MeshRenderer blockMeshRenderer = (MeshRenderer)chunkObject.AddComponent(typeof(MeshRenderer));
        blockMeshRenderer.materials = blockMaterials;

        MeshCollider blockMeshCollider = chunkObject.AddComponent(typeof(MeshCollider)) as MeshCollider;
        blockMeshCollider.sharedMesh = colliderMesh;

    }

}
