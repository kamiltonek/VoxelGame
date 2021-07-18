using Assets.Scripts;
using Assets.Scripts.Enums;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Block
{
    private BlockType blockType;
    private bool isTransparent;
    private Chunk chunkParent;
    private GameObject blockParent;
    private Vector3 blockPosition;
    private Vector3Int cubeBlockPosition;

    static Vector3[] vertices = new Vector3[14]
    {
        new Vector3(-0.25f,  0.25f,  (float)(-0.5 * Math.Sqrt(3) / 2)),
        new Vector3( 0.25f,  0.25f,  (float)(-0.5 * Math.Sqrt(3) / 2)),
        new Vector3(  0.5f,  0.25f,  0f),
        new Vector3( 0.25f,  0.25f,  (float)(0.5 * Math.Sqrt(3) / 2)),
        new Vector3(-0.25f,  0.25f,  (float)(0.5 * Math.Sqrt(3) / 2)),
        new Vector3( -0.5f,  0.25f,  0),
        new Vector3(-0.25f, -0.25f,  (float)(-0.5 * Math.Sqrt(3) / 2)),
        new Vector3( 0.25f, -0.25f,  (float)(-0.5 * Math.Sqrt(3) / 2)),
        new Vector3(  0.5f, -0.25f,  0f),
        new Vector3( 0.25f, -0.25f,  (float)(0.5 * Math.Sqrt(3) / 2)),
        new Vector3(-0.25f, -0.25f,  (float)(0.5 * Math.Sqrt(3) / 2)),
        new Vector3( -0.5f, -0.25f,  0),
        new Vector3(0, 0.25f,  0),
        new Vector3(0, -0.25f,  0)
    };

    static Vector3 vectorLeftFront = new Vector3(-0.75f, 0, (float)(0.5 * Math.Sqrt(3) / 2));
    static Vector3 vectorLeftBack = new Vector3(-0.75f, 0, (float)(-0.5 * Math.Sqrt(3) / 2));
    static Vector3 vectorRightFront = new Vector3(0.75f, 0, (float)(-0.5 * Math.Sqrt(3) / 2));
    static Vector3 vectorRightBack = new Vector3(0.75f, 0, (float)(0.5 * Math.Sqrt(3) / 2));

    static Vector3[] verticesFront = new Vector3[] { vertices[6], vertices[7], vertices[0], vertices[1] };
    static Vector3[] verticesBack = new Vector3[] { vertices[9], vertices[10], vertices[3], vertices[4] };
    static Vector3[] verticesLeftBack = new Vector3[] { vertices[10], vertices[11], vertices[4], vertices[5] };
    static Vector3[] verticesLeftFront = new Vector3[] { vertices[11], vertices[6], vertices[5], vertices[0] };
    static Vector3[] verticesRightBack = new Vector3[] { vertices[8], vertices[9], vertices[2], vertices[3] };
    static Vector3[] verticesRightFront = new Vector3[] { vertices[7], vertices[8], vertices[1], vertices[2] };
    static Vector3[] verticesTop = new Vector3[] { vertices[1], vertices[0], vertices[2], vertices[5], vertices[3], vertices[4], vertices[12] };
    static Vector3[] verticesBottom = new Vector3[] { vertices[6], vertices[7], vertices[11], vertices[8], vertices[10], vertices[9], vertices[13] };

    static Vector3[] normalsFront = new Vector3[] { Vector3.forward, Vector3.forward, Vector3.forward, Vector3.forward };
    static Vector3[] normalsBack = new Vector3[] { Vector3.back, Vector3.back, Vector3.back, Vector3.back };
    static Vector3[] normalsLeftBack = new Vector3[] { vectorLeftBack, vectorLeftBack, vectorLeftBack, vectorLeftBack };
    static Vector3[] normalsLeftFront = new Vector3[] { vectorLeftFront, vectorLeftFront, vectorLeftFront, vectorLeftFront };
    static Vector3[] normalsRightBack = new Vector3[] { vectorRightBack, vectorRightBack, vectorRightBack, vectorRightBack };
    static Vector3[] normalsRightFront = new Vector3[] { vectorRightFront, vectorRightFront, vectorRightFront, vectorRightFront };
    static Vector3[] normalsTop = new Vector3[] { Vector3.up, Vector3.up, Vector3.up, Vector3.up, Vector3.up, Vector3.up, Vector3.up };
    static Vector3[] normalsBottom = new Vector3[] { Vector3.down, Vector3.down, Vector3.down, Vector3.down, Vector3.down, Vector3.down, Vector3.down };

    static int[] trianglesSide = new int[] { 3, 1, 0, 0, 2, 3 };
    static int[] trianglesBase = new int[] { 0, 1, 6, 1, 3, 6, 3, 5, 6, 5, 4, 6, 4, 2, 6, 2, 0, 6 };

    public Block(
        BlockType blockType, 
        Chunk chunkParent, 
        Vector3 blockPosition,
        Vector3Int cubeBlockPosition)
    {
        this.blockType = blockType;
        this.chunkParent = chunkParent;
        this.blockParent = chunkParent.chunkObject;
        this.blockPosition = blockPosition;
        this.cubeBlockPosition = cubeBlockPosition;

        isTransparent = blockType.IsTransparent ? true : false;
    }

    public void CreateBlock()
    {
        if (blockType.IsTransparent)
            return;

        if(HasTransparentNeighbour(BlockSideEnum.FRONT))
            CreateBlockSide(BlockSideEnum.FRONT);
        if (HasTransparentNeighbour(BlockSideEnum.BACK))
            CreateBlockSide(BlockSideEnum.BACK);
        if (HasTransparentNeighbour(BlockSideEnum.LEFT_BACK))
            CreateBlockSide(BlockSideEnum.LEFT_BACK);
        if (HasTransparentNeighbour(BlockSideEnum.LEFT_FRONT))
            CreateBlockSide(BlockSideEnum.LEFT_FRONT);
        if (HasTransparentNeighbour(BlockSideEnum.RIGHT_BACK))
            CreateBlockSide(BlockSideEnum.RIGHT_BACK);
        if (HasTransparentNeighbour(BlockSideEnum.RIGHT_FRONT))
            CreateBlockSide(BlockSideEnum.RIGHT_FRONT);
        if (HasTransparentNeighbour(BlockSideEnum.TOP))
            CreateBlockSide(BlockSideEnum.TOP);
        if (HasTransparentNeighbour(BlockSideEnum.BOTTOM))
            CreateBlockSide(BlockSideEnum.BOTTOM);
    }

    private bool HasTransparentNeighbour(BlockSideEnum blockSide)
    {
        Block[,,] chunkBlocks = chunkParent.chunkBlocks;
        Vector3Int neighbourPosition;

        if (blockSide == BlockSideEnum.FRONT)
            neighbourPosition = new Vector3Int(cubeBlockPosition.x, cubeBlockPosition.y, cubeBlockPosition.z - 1);
        else if (blockSide == BlockSideEnum.BACK)
            neighbourPosition = new Vector3Int(cubeBlockPosition.x, cubeBlockPosition.y, cubeBlockPosition.z + 1);
        else if(blockSide == BlockSideEnum.LEFT_BACK)
        {
            neighbourPosition = cubeBlockPosition.x % 2 == 0
                ? new Vector3Int(cubeBlockPosition.x - 1, cubeBlockPosition.y, cubeBlockPosition.z + 1)
                : new Vector3Int(cubeBlockPosition.x - 1, cubeBlockPosition.y, cubeBlockPosition.z);
        }
        else if(blockSide == BlockSideEnum.LEFT_FRONT)
        {
            neighbourPosition = cubeBlockPosition.x % 2 == 0 
                ? new Vector3Int(cubeBlockPosition.x - 1, cubeBlockPosition.y, cubeBlockPosition.z) 
                : new Vector3Int(cubeBlockPosition.x - 1, cubeBlockPosition.y, cubeBlockPosition.z - 1);
        }
            
        else if(blockSide == BlockSideEnum.RIGHT_BACK)
        {
            neighbourPosition = cubeBlockPosition.x % 2 == 0
                ? new Vector3Int(cubeBlockPosition.x + 1, cubeBlockPosition.y, cubeBlockPosition.z + 1)
                : new Vector3Int(cubeBlockPosition.x + 1, cubeBlockPosition.y, cubeBlockPosition.z);
        }
        else if(blockSide == BlockSideEnum.RIGHT_FRONT)
        {
            neighbourPosition = cubeBlockPosition.x % 2 == 0
                ? new Vector3Int(cubeBlockPosition.x + 1, cubeBlockPosition.y, cubeBlockPosition.z)
                : new Vector3Int(cubeBlockPosition.x + 1, cubeBlockPosition.y, cubeBlockPosition.z - 1);
        }
        else if (blockSide == BlockSideEnum.TOP)
            neighbourPosition = new Vector3Int(cubeBlockPosition.x, cubeBlockPosition.y + 1, cubeBlockPosition.z);
        else
            neighbourPosition = new Vector3Int(cubeBlockPosition.x, cubeBlockPosition.y - 1, cubeBlockPosition.z);

        if (neighbourPosition.x >= 0 && neighbourPosition.x < chunkBlocks.GetLength(0) &&
            neighbourPosition.y >= 0 && neighbourPosition.y < chunkBlocks.GetLength(1) &&
            neighbourPosition.z >= 0 && neighbourPosition.z < chunkBlocks.GetLength(2))
        {
            return chunkBlocks[neighbourPosition.x, neighbourPosition.y, neighbourPosition.z].isTransparent;
        }

        return true;

    }

    private void CreateBlockSide(BlockSideEnum side)
    {
        Vector2[] uvs = blockType.GetUv(side);

        Mesh mesh = new Mesh();
        mesh = GenerateBlockSide(mesh, side, uvs);

        GameObject blockSide = new GameObject();
        blockSide.transform.position = blockPosition;
        blockSide.transform.parent = blockParent.transform;

        MeshFilter meshFilter = (MeshFilter)blockSide.AddComponent(typeof(MeshFilter));
        meshFilter.mesh = mesh;
    }

    private Mesh GenerateBlockSide(Mesh mesh, BlockSideEnum side, Vector2[] uvs)
    {
        switch (side)
        {
            case BlockSideEnum.FRONT:
                mesh.vertices = verticesFront;
                mesh.normals = normalsFront;
                mesh.uv = uvs;
                mesh.triangles = trianglesSide;
                break;
            case BlockSideEnum.BACK:
                mesh.vertices = verticesBack;
                mesh.normals = normalsBack;
                mesh.uv = uvs;
                mesh.triangles = trianglesSide;
                break;
            case BlockSideEnum.LEFT_BACK:
                mesh.vertices = verticesLeftBack;
                mesh.normals = normalsLeftBack;
                mesh.uv = uvs;
                mesh.triangles = trianglesSide;
                break;
            case BlockSideEnum.LEFT_FRONT:
                mesh.vertices = verticesLeftFront;
                mesh.normals = normalsLeftFront;
                mesh.uv = uvs;
                mesh.triangles = trianglesSide;
                break;
            case BlockSideEnum.RIGHT_BACK:
                mesh.vertices = verticesRightBack;
                mesh.normals = normalsRightBack;
                mesh.uv = uvs;
                mesh.triangles = trianglesSide;
                break;
            case BlockSideEnum.RIGHT_FRONT:
                mesh.vertices = verticesRightFront;
                mesh.normals = normalsRightFront;
                mesh.uv = uvs; 
                mesh.triangles = trianglesSide;
                break;
            case BlockSideEnum.TOP:
                mesh.vertices = verticesTop;
                mesh.normals = normalsTop;
                mesh.uv = uvs;
                mesh.triangles = trianglesBase;
                break;
            case BlockSideEnum.BOTTOM:
                mesh.vertices = verticesBottom;
                mesh.normals = normalsBottom;
                mesh.uv = uvs;
                mesh.triangles = trianglesBase;
                break;

        }

        return mesh;
    }

}
