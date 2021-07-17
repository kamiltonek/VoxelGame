using Assets.Scripts.Enums;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Block
{
    private int textureSize = 128;
    private BlockTypeEnum blockType;
    private bool isTransparent;
    private GameObject blockParent;
    private Vector3 blockPosition;
    private Vector3Int cubeBlockPosition;
    private Dictionary<string, Rect> blockUVCoordinates;

    Vector3[] vertices = new Vector3[14]
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
    Vector2[] uv = new Vector2[11]
    {
        new Vector2(0.25f, 0.25f),
        new Vector2(0.75f, 0.25f),
        new Vector2(0.25f, 0.75f),
        new Vector2(0.75f, 0.75f),
        new Vector2(0.25f, 1), // 1
        new Vector2(0.75f, 1), // 2
        new Vector2(0, 0.5f),
        new Vector2(1, 0.5f),
        new Vector2(0.25f, 0),
        new Vector2(0.75f, 0),
        new Vector2(0.5f, 0.5f)
    };

    Vector3 vectorLeftFront = new Vector3(-0.75f, 0, (float)(0.5 * Math.Sqrt(3) / 2));
    Vector3 vectorLeftBack = new Vector3(-0.75f, 0, (float)(-0.5 * Math.Sqrt(3) / 2));
    Vector3 vectorRightFront = new Vector3(0.75f, 0, (float)(-0.5 * Math.Sqrt(3) / 2));
    Vector3 vectorRightBack = new Vector3(0.75f, 0, (float)(0.5 * Math.Sqrt(3) / 2));

    public Block(
        BlockTypeEnum blockType, 
        GameObject blockParent, 
        Vector3 blockPosition,
        Vector3Int cubeBlockPosition,
        Dictionary<string, Rect> blockUVCoordinates)
    {
        this.blockType = blockType;
        this.blockParent = blockParent;
        this.blockPosition = blockPosition;
        this.cubeBlockPosition = cubeBlockPosition;
        this.blockUVCoordinates = blockUVCoordinates;

        isTransparent = 
            blockType == BlockTypeEnum.AIR
            ? true : false;
    }

    public void CreateBlock()
    {
        if (blockType == BlockTypeEnum.AIR)
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
        Block[,,] chunkBlocks = blockParent.GetComponent<Chunk>().chunkBlocks;
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
        Vector2[] uvs = GetBlockSideUVs(side);

        Mesh mesh = new Mesh();
        mesh = GenerateBlockSide(mesh, side, uvs);

        GameObject blockSide = new GameObject(side.ToString());
        blockSide.transform.position = blockPosition;
        blockSide.transform.parent = blockParent.transform;

        MeshFilter meshFilter = (MeshFilter)blockSide.AddComponent(typeof(MeshFilter));
        meshFilter.mesh = mesh;
    }

    private Vector2[] GetBlockSideUVs(BlockSideEnum side)
    {
        Vector2[] uvs;

        if(blockType == BlockTypeEnum.AIR)
        {
            uvs = new Vector2[4]
            {
                new Vector2(0.25f, 0.25f),
                new Vector2(0.75f, 0.25f),
                new Vector2(0.25f, 0.75f),
                new Vector2(0.75f, 0.75f)
            };
        }
        else if (blockType == BlockTypeEnum.GRASS && side == BlockSideEnum.BOTTOM)
        {
            uvs = new Vector2[7]
            {
                new Vector2(
                    blockUVCoordinates[BlockTypeEnum.DIRT.ToString().ToLower()].x + (blockUVCoordinates[BlockTypeEnum.DIRT.ToString().ToLower()].width * 0.25f),
                    blockUVCoordinates[BlockTypeEnum.DIRT.ToString().ToLower()].y),
                new Vector2(
                    blockUVCoordinates[BlockTypeEnum.DIRT.ToString().ToLower()].x + (blockUVCoordinates[BlockTypeEnum.DIRT.ToString().ToLower()].width * 0.75f),
                    blockUVCoordinates[BlockTypeEnum.DIRT.ToString().ToLower()].y),
                new Vector2(
                    blockUVCoordinates[BlockTypeEnum.DIRT.ToString().ToLower()].x,
                    blockUVCoordinates[BlockTypeEnum.DIRT.ToString().ToLower()].y + (blockUVCoordinates[BlockTypeEnum.DIRT.ToString().ToLower()].height * 0.5f)),
                new Vector2(
                    blockUVCoordinates[BlockTypeEnum.DIRT.ToString().ToLower()].x + (blockUVCoordinates[BlockTypeEnum.DIRT.ToString().ToLower()].width),
                    blockUVCoordinates[BlockTypeEnum.DIRT.ToString().ToLower()].y + (blockUVCoordinates[BlockTypeEnum.DIRT.ToString().ToLower()].height * 0.5f)),
                new Vector2(
                    blockUVCoordinates[BlockTypeEnum.DIRT.ToString().ToLower()].x + (blockUVCoordinates[BlockTypeEnum.DIRT.ToString().ToLower()].width * 0.25f),
                    blockUVCoordinates[BlockTypeEnum.DIRT.ToString().ToLower()].y + (blockUVCoordinates[BlockTypeEnum.DIRT.ToString().ToLower()].height)),
                new Vector2(
                    blockUVCoordinates[BlockTypeEnum.DIRT.ToString().ToLower()].x + (blockUVCoordinates[BlockTypeEnum.DIRT.ToString().ToLower()].width * 0.75f),
                    blockUVCoordinates[BlockTypeEnum.DIRT.ToString().ToLower()].y + (blockUVCoordinates[BlockTypeEnum.DIRT.ToString().ToLower()].height)),
                new Vector2(
                    blockUVCoordinates[BlockTypeEnum.DIRT.ToString().ToLower()].x + (blockUVCoordinates[BlockTypeEnum.DIRT.ToString().ToLower()].width * 0.5f),
                    blockUVCoordinates[BlockTypeEnum.DIRT.ToString().ToLower()].y + (blockUVCoordinates[BlockTypeEnum.DIRT.ToString().ToLower()].height * 0.5f))
            };
        }
        else if (blockType == BlockTypeEnum.GRASS && side != BlockSideEnum.TOP)
        {
            uvs = new Vector2[4]
            {
                new Vector2(
                    blockUVCoordinates["grass_side"].x + (blockUVCoordinates["grass_side"].width * 0.25f),
                    blockUVCoordinates["grass_side"].y + (blockUVCoordinates["grass_side"].height * 0.25f)),
                new Vector2(
                    blockUVCoordinates["grass_side"].x + (blockUVCoordinates["grass_side"].width * 0.75f),
                    blockUVCoordinates["grass_side"].y + (blockUVCoordinates["grass_side"].height * 0.25f)),
                new Vector2(
                    blockUVCoordinates["grass_side"].x + (blockUVCoordinates["grass_side"].width * 0.25f),
                    blockUVCoordinates["grass_side"].y + (blockUVCoordinates["grass_side"].height * 0.75f)),
                new Vector2(
                    blockUVCoordinates["grass_side"].x + (blockUVCoordinates["grass_side"].width * 0.75f),
                    blockUVCoordinates["grass_side"].y + (blockUVCoordinates["grass_side"].height * 0.75f)),
            };
        }
        else if (side == BlockSideEnum.TOP || side == BlockSideEnum.BOTTOM)
        {
            uvs = new Vector2[7]
            {
                new Vector2(
                    blockUVCoordinates[blockType.ToString().ToLower()].x + (blockUVCoordinates[blockType.ToString().ToLower()].width * 0.25f), 
                    blockUVCoordinates[blockType.ToString().ToLower()].y),
                new Vector2(
                    blockUVCoordinates[blockType.ToString().ToLower()].x + (blockUVCoordinates[blockType.ToString().ToLower()].width * 0.75f), 
                    blockUVCoordinates[blockType.ToString().ToLower()].y),
                new Vector2(
                    blockUVCoordinates[blockType.ToString().ToLower()].x, 
                    blockUVCoordinates[blockType.ToString().ToLower()].y + (blockUVCoordinates[blockType.ToString().ToLower()].height * 0.5f)),
                new Vector2(
                    blockUVCoordinates[blockType.ToString().ToLower()].x + (blockUVCoordinates[blockType.ToString().ToLower()].width), 
                    blockUVCoordinates[blockType.ToString().ToLower()].y + (blockUVCoordinates[blockType.ToString().ToLower()].height * 0.5f)),
                new Vector2(
                    blockUVCoordinates[blockType.ToString().ToLower()].x + (blockUVCoordinates[blockType.ToString().ToLower()].width * 0.25f), 
                    blockUVCoordinates[blockType.ToString().ToLower()].y + (blockUVCoordinates[blockType.ToString().ToLower()].height)),
                new Vector2(
                    blockUVCoordinates[blockType.ToString().ToLower()].x + (blockUVCoordinates[blockType.ToString().ToLower()].width * 0.75f), 
                    blockUVCoordinates[blockType.ToString().ToLower()].y + (blockUVCoordinates[blockType.ToString().ToLower()].height)),
                new Vector2(
                    blockUVCoordinates[blockType.ToString().ToLower()].x + (blockUVCoordinates[blockType.ToString().ToLower()].width * 0.5f),
                    blockUVCoordinates[blockType.ToString().ToLower()].y + (blockUVCoordinates[blockType.ToString().ToLower()].height * 0.5f))
            };
        }
        else
        {
            uvs = new Vector2[4]
            {
                new Vector2(
                    blockUVCoordinates[blockType.ToString().ToLower()].x + (blockUVCoordinates[blockType.ToString().ToLower()].width * 0.25f), 
                    blockUVCoordinates[blockType.ToString().ToLower()].y + (blockUVCoordinates[blockType.ToString().ToLower()].height * 0.25f)),
                new Vector2(
                    blockUVCoordinates[blockType.ToString().ToLower()].x + (blockUVCoordinates[blockType.ToString().ToLower()].width * 0.75f), 
                    blockUVCoordinates[blockType.ToString().ToLower()].y + (blockUVCoordinates[blockType.ToString().ToLower()].height * 0.25f)),
                new Vector2(
                    blockUVCoordinates[blockType.ToString().ToLower()].x + (blockUVCoordinates[blockType.ToString().ToLower()].width * 0.25f), 
                    blockUVCoordinates[blockType.ToString().ToLower()].y + (blockUVCoordinates[blockType.ToString().ToLower()].height * 0.75f)),
                new Vector2(
                    blockUVCoordinates[blockType.ToString().ToLower()].x + (blockUVCoordinates[blockType.ToString().ToLower()].width * 0.75f), 
                    blockUVCoordinates[blockType.ToString().ToLower()].y + (blockUVCoordinates[blockType.ToString().ToLower()].height * 0.75f)),
            };
        }

        return uvs;
    }

    private Mesh GenerateBlockSide(Mesh mesh, BlockSideEnum side, Vector2[] uvs)
    {
        switch (side)
        {
            case BlockSideEnum.FRONT:
                mesh.vertices = new Vector3[] { vertices[7], vertices[6], vertices[1], vertices[0] }; // 1 0 7 6
                mesh.normals = new Vector3[] { Vector3.forward, Vector3.forward, Vector3.forward, Vector3.forward };
                mesh.uv = uvs;
                mesh.triangles = new int[] { 0, 1, 2, 2, 1, 3 };
                break;
            case BlockSideEnum.BACK:
                mesh.vertices = new Vector3[] { vertices[9], vertices[10], vertices[3], vertices[4]};
                mesh.normals = new Vector3[] { Vector3.back, Vector3.back, Vector3.back, Vector3.back };
                mesh.uv = uvs;
                mesh.triangles = new int[] { 3, 1, 0, 0, 2, 3 };
                break;
            case BlockSideEnum.LEFT_BACK:
                mesh.vertices = new Vector3[] { vertices[10], vertices[11], vertices[4], vertices[5] };
                mesh.normals = new Vector3[] { vectorLeftBack, vectorLeftBack, vectorLeftBack, vectorLeftBack };
                mesh.uv = uvs;
                mesh.triangles = new int[] { 3, 1, 0, 0, 2, 3 };
                break;
            case BlockSideEnum.LEFT_FRONT:
                mesh.vertices = new Vector3[] { vertices[11], vertices[6], vertices[5], vertices[0] };
                mesh.normals = new Vector3[] { vectorLeftFront, vectorLeftFront, vectorLeftFront, vectorLeftFront };
                mesh.uv = uvs;
                mesh.triangles = new int[] { 3, 1, 0, 0, 2, 3 };
                break;
            case BlockSideEnum.RIGHT_BACK:
                mesh.vertices = new Vector3[] { vertices[8], vertices[9], vertices[2], vertices[3] };
                mesh.normals = new Vector3[] { vectorRightBack, vectorRightBack, vectorRightBack, vectorRightBack };
                mesh.uv = uvs;
                mesh.triangles = new int[] { 3, 1, 0, 0, 2, 3 };
                break;
            case BlockSideEnum.RIGHT_FRONT:
                mesh.vertices = new Vector3[] { vertices[7], vertices[8], vertices[1], vertices[2] };
                mesh.normals = new Vector3[] { vectorRightFront, vectorRightFront, vectorRightFront, vectorRightFront };
                mesh.uv = uvs; 
                mesh.triangles = new int[] { 3, 1, 0, 0, 2, 3 };
                break;
            case BlockSideEnum.TOP:
                mesh.vertices = new Vector3[] { vertices[1], vertices[0], vertices[2], vertices[5], vertices[3], vertices[4], vertices[12] };
                mesh.normals = new Vector3[] { Vector3.up, Vector3.up, Vector3.up, Vector3.up, Vector3.up, Vector3.up, Vector3.up };
                mesh.uv = uvs; // 5 4 6 8 9 7 10
                mesh.triangles = new int[] { 0, 1, 6, 1, 3, 6, 3, 5, 6, 5, 4, 6, 4, 2, 6, 2, 0, 6 };
                break;
            case BlockSideEnum.BOTTOM:
                mesh.vertices = new Vector3[] { vertices[6], vertices[7], vertices[11], vertices[8], vertices[10], vertices[9], vertices[13] };
                mesh.normals = new Vector3[] { Vector3.down, Vector3.down, Vector3.down, Vector3.down, Vector3.down, Vector3.down, Vector3.down };
                mesh.uv = uvs;
                mesh.triangles = new int[] { 0, 1, 6, 1, 3, 6, 3, 5, 6, 5, 4, 6, 4, 2, 6, 2, 0, 6 };
                break;

        }

        return mesh;
    }

}
