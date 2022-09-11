using Assets.Scripts;
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

                    this.status = ChunkStatusEnum.TO_DRAW;

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

        MeshFilter blockMeshFilter = (MeshFilter)chunkObject.GetComponent(typeof(MeshFilter));

        if (blockMeshFilter == null)
        {
            blockMeshFilter = (MeshFilter)chunkObject.AddComponent(typeof(MeshFilter));
        }
        blockMeshFilter.mesh = mesh;

        MeshRenderer blockMeshRenderer = (MeshRenderer)chunkObject.GetComponent(typeof(MeshRenderer));
        if (blockMeshRenderer == null)
        {
            blockMeshRenderer = (MeshRenderer)chunkObject.AddComponent(typeof(MeshRenderer));
        }
        blockMeshRenderer.materials = blockMaterials;

        MeshCollider blockMeshCollider = chunkObject.GetComponent(typeof(MeshCollider)) as MeshCollider;
        if (blockMeshCollider == null)
        {
            blockMeshCollider = chunkObject.AddComponent(typeof(MeshCollider)) as MeshCollider;
        }
        blockMeshCollider.sharedMesh = colliderMesh;

    }

    public void AddBlock(Vector3Int blockPos)
    {
        bool oddX = blockPos.x % 2 == 0;
        float zOffset = oddX ? 0 : (float)(0.5 * Math.Sqrt(3) / 2);

        float posX = blockPos.x * 0.75f;
        float posY = blockPos.y * 0.5f;
        float posZ = blockPos.z - zOffset - blockPos.z * (1f - (float)(0.5 * Math.Sqrt(3)));

        chunkBlocks[blockPos.x, blockPos.y, blockPos.z] = new Block(
            World.blockTypes[BlockName.DESERT_BLOCK],
            this,
            new Vector3(posX, posY, posZ),
            new Vector3Int(blockPos.x, blockPos.y, blockPos.z));

        this.status = ChunkStatusEnum.TO_DRAW;
        RefreshChunk();

        /*Chunk neigbourChunk;
        //Refresh suraround chunks
        if (blockPos.x == 0)
        {
            string chunkNeighbourPosition = World.GenerateChunkName(chunkObject.transform.position + Vector3.left * World.chunkSize);
            if (World.chunks.TryGetValue(chunkNeighbourPosition, out neigbourChunk))
            {
                neigbourChunk.RefreshChunk();
            }
        }
        if (blockPos.x == World.chunkSize - 1)
        {
            string chunkNeighbourPosition = World.GenerateChunkName(chunkObject.transform.position + Vector3.right * World.chunkSize);
            if (World.chunks.TryGetValue(chunkNeighbourPosition, out neigbourChunk))
            {
                neigbourChunk.RefreshChunk();
            }
        }
        if (blockPos.y == 0)
        {
            string chunkNeighbourPosition = World.GenerateChunkName(chunkObject.transform.position + Vector3.down * World.chunkSize);
            if (World.chunks.TryGetValue(chunkNeighbourPosition, out neigbourChunk))
            {
                neigbourChunk.RefreshChunk();
            }
        }
        if (blockPos.y == World.chunkSize - 1)
        {
            string chunkNeighbourPosition = World.GenerateChunkName(chunkObject.transform.position + Vector3.up * World.chunkSize);
            if (World.chunks.TryGetValue(chunkNeighbourPosition, out neigbourChunk))
            {
                neigbourChunk.RefreshChunk();
            }
        }
        if (blockPos.z == 0)
        {
            string chunkNeighbourPosition = World.GenerateChunkName(chunkObject.transform.position + Vector3.back * World.chunkSize);
            if (World.chunks.TryGetValue(chunkNeighbourPosition, out neigbourChunk))
            {
                neigbourChunk.RefreshChunk();
            }
        }
        if (blockPos.z == World.chunkSize - 1)
        {
            string chunkNeighbourPosition = World.GenerateChunkName(chunkObject.transform.position + Vector3.forward * World.chunkSize);
            if (World.chunks.TryGetValue(chunkNeighbourPosition, out neigbourChunk))
            {
                neigbourChunk.RefreshChunk();
            }
        }*/
    }
    public void RemoveBlock(Vector3Int blockPos)
    {
        chunkBlocks[blockPos.x, blockPos.y, blockPos.z] = new Block(World.blockTypes[BlockName.SAND],
            this,
            chunkObject.transform.position,
            new Vector3Int(blockPos.x, blockPos.y, blockPos.z));
        RefreshChunk();

        Chunk neigbourChunk;
        //Refresh suraround chunks
        if (blockPos.x == 0)
        {
            string chunkNeighbourPosition = World.GenerateChunkName(chunkObject.transform.position + Vector3.left * World.chunkSize);
            if (World.chunks.TryGetValue(chunkNeighbourPosition, out neigbourChunk))
            {
                neigbourChunk.RefreshChunk();
            }
        }
        if (blockPos.x == World.chunkSize - 1)
        {
            string chunkNeighbourPosition = World.GenerateChunkName(chunkObject.transform.position + Vector3.right * World.chunkSize);
            if (World.chunks.TryGetValue(chunkNeighbourPosition, out neigbourChunk))
            {
                neigbourChunk.RefreshChunk();
            }
        }
        if (blockPos.y == 0)
        {
            string chunkNeighbourPosition = World.GenerateChunkName(chunkObject.transform.position + Vector3.down * World.chunkSize);
            if (World.chunks.TryGetValue(chunkNeighbourPosition, out neigbourChunk))
            {
                neigbourChunk.RefreshChunk();
            }
        }
        if (blockPos.y == World.chunkSize - 1)
        {
            string chunkNeighbourPosition = World.GenerateChunkName(chunkObject.transform.position + Vector3.up * World.chunkSize);
            if (World.chunks.TryGetValue(chunkNeighbourPosition, out neigbourChunk))
            {
                neigbourChunk.RefreshChunk();
            }
        }
        if (blockPos.z == 0)
        {
            string chunkNeighbourPosition = World.GenerateChunkName(chunkObject.transform.position + Vector3.back * World.chunkSize);
            if (World.chunks.TryGetValue(chunkNeighbourPosition, out neigbourChunk))
            {
                neigbourChunk.RefreshChunk();
            }
        }
        if (blockPos.z == World.chunkSize - 1)
        {
            string chunkNeighbourPosition = World.GenerateChunkName(chunkObject.transform.position + Vector3.forward * World.chunkSize);
            if (World.chunks.TryGetValue(chunkNeighbourPosition, out neigbourChunk))
            {
                neigbourChunk.RefreshChunk();
            }
        }

    }

    public void RefreshChunk()
    {
        ClearChunkMesh();
        DrawChunk(World.chunkSize);
    }

    private void ClearChunkMesh()
    {
        vertexIndex = 0;
        vertices.Clear();
        triangles.Clear();
        transparentTriangles.Clear();
        liquidTriangles.Clear();
        uvs.Clear();
    }
}
