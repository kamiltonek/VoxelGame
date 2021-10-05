using Assets.Scripts;
using Assets.Scripts.Enums;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Block
{
    private BlockType blockType;
    private Chunk chunkParent;
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
        this.blockPosition = blockPosition;
        this.cubeBlockPosition = cubeBlockPosition;
    }

    public void CreateBlock()
    {
        if (blockType.IsTransparent)
            return;

        if(HasTransparentNeighbourTest(BlockSideEnum.FRONT))
            GenerateBlockSide(BlockSideEnum.FRONT);
        if (HasTransparentNeighbourTest(BlockSideEnum.BACK))
            GenerateBlockSide(BlockSideEnum.BACK);
        if (HasTransparentNeighbourTest(BlockSideEnum.LEFT_BACK))
            GenerateBlockSide(BlockSideEnum.LEFT_BACK);
        if (HasTransparentNeighbourTest(BlockSideEnum.LEFT_FRONT))
            GenerateBlockSide(BlockSideEnum.LEFT_FRONT);
        if (HasTransparentNeighbourTest(BlockSideEnum.RIGHT_BACK))
            GenerateBlockSide(BlockSideEnum.RIGHT_BACK);
        if (HasTransparentNeighbourTest(BlockSideEnum.RIGHT_FRONT))
            GenerateBlockSide(BlockSideEnum.RIGHT_FRONT);
        if (HasTransparentNeighbourTest(BlockSideEnum.TOP))
            GenerateBlockSide(BlockSideEnum.TOP);
        //if (HasTransparentNeighbour(BlockSideEnum.BOTTOM))
           //GenerateBlockSide(BlockSideEnum.BOTTOM);
    }

    private bool HasTransparentNeighbour(BlockSideEnum blockSide)
    {
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

        Block[,,] chunkBlocks = chunkParent.chunkBlocks;

        if(neighbourPosition.x < 0 || neighbourPosition.x >= World.chunkSize ||
           neighbourPosition.y < 0 || neighbourPosition.y >= World.chunkSize ||
           neighbourPosition.z < 0 || neighbourPosition.z >= World.chunkSize)
        {


            Vector3 neighbourChunkPosition = this.chunkParent.chunkObject.transform.position;
            neighbourChunkPosition.x += (neighbourPosition.x - blockPosition.x) * World.chunkSize;
            neighbourChunkPosition.y += (neighbourPosition.y - blockPosition.y) * World.chunkSize;
            neighbourChunkPosition.z += (neighbourPosition.z - blockPosition.z) * World.chunkSize;

            neighbourChunkPosition.x = neighbourChunkPosition.x / 10 / 0.75f;
            neighbourChunkPosition.y = neighbourChunkPosition.y / 10 / 0.5f;
            neighbourChunkPosition.z = neighbourChunkPosition.z / 10 / (float)(Math.Sqrt(3)) / 2;

            string neighbourChunkName = World.GetChunkName((int)neighbourChunkPosition.x, (int)neighbourChunkPosition.y, (int)neighbourChunkPosition.z);

            if(World.chunks.TryGetValue(neighbourChunkName, out Chunk neighbourChunk))
            {
                chunkBlocks = neighbourChunk.chunkBlocks;
            }
            else
            {
                return false;
            }
        }

        if (neighbourPosition.x < 0) neighbourPosition.x = World.chunkSize - 1;
        if (neighbourPosition.y < 0) neighbourPosition.y = World.chunkSize - 1;
        if (neighbourPosition.z < 0) neighbourPosition.z = World.chunkSize - 1;
        if (neighbourPosition.x >= World.chunkSize) neighbourPosition.x = 0;
        if (neighbourPosition.y >= World.chunkSize) neighbourPosition.y = 0;
        if (neighbourPosition.z >= World.chunkSize) neighbourPosition.z = 0;

        var neighbourBlockType = chunkBlocks[(int)neighbourPosition.x, (int)neighbourPosition.y, (int)neighbourPosition.z].blockType;

        if(neighbourBlockType.IsTranslucent && !neighbourBlockType.IsTransparent && this.blockType.IsTranslucent)
        {
            return false;
        }

        return neighbourBlockType.IsTransparent || neighbourBlockType.IsTranslucent;

    }

    private bool HasTransparentNeighbourTest(BlockSideEnum blockSide)
    {
        if(blockType.IsLiquid && blockSide != BlockSideEnum.TOP)
        {
            return false;
        }

        Vector3Int neighbourCubePosition;
        Vector3 neighbourPosition = this.blockPosition + chunkParent.chunkObject.transform.position;

        if (blockSide == BlockSideEnum.FRONT)
        {
            neighbourCubePosition = new Vector3Int(cubeBlockPosition.x, cubeBlockPosition.y, cubeBlockPosition.z - 1);

            if(neighbourCubePosition.z == -1)
            {
                neighbourPosition.z -= (float)(Math.Sqrt(3)) / 2;
            }
        }
        else if (blockSide == BlockSideEnum.BACK)
        {
            neighbourCubePosition = new Vector3Int(cubeBlockPosition.x, cubeBlockPosition.y, cubeBlockPosition.z + 1);

            if (neighbourCubePosition.z == World.chunkSize)
            {
                neighbourPosition.z += (float)(Math.Sqrt(3)) / 2;
            }
        }
        else if (blockSide == BlockSideEnum.LEFT_BACK)
        {
            neighbourCubePosition = cubeBlockPosition.x % 2 == 0
                ? new Vector3Int(cubeBlockPosition.x - 1, cubeBlockPosition.y, cubeBlockPosition.z + 1)
                : new Vector3Int(cubeBlockPosition.x - 1, cubeBlockPosition.y, cubeBlockPosition.z);

            if (neighbourCubePosition.x == -1 || neighbourCubePosition.z == World.chunkSize)
            {
                neighbourPosition.z += 0.5f * (float)(Math.Sqrt(3)) / 2;
                neighbourPosition.x -= 0.75f;
            }
        }
        else if (blockSide == BlockSideEnum.LEFT_FRONT)
        {
            neighbourCubePosition = cubeBlockPosition.x % 2 == 0
                ? new Vector3Int(cubeBlockPosition.x - 1, cubeBlockPosition.y, cubeBlockPosition.z)
                : new Vector3Int(cubeBlockPosition.x - 1, cubeBlockPosition.y, cubeBlockPosition.z - 1);

            if(neighbourCubePosition.x == -1 || neighbourCubePosition.z == -1)
            {
                neighbourPosition.z -= 0.5f * (float)(Math.Sqrt(3)) / 2;
                neighbourPosition.x -= 0.75f;
            }
        }

        else if (blockSide == BlockSideEnum.RIGHT_BACK)
        {
            neighbourCubePosition = cubeBlockPosition.x % 2 == 0
                ? new Vector3Int(cubeBlockPosition.x + 1, cubeBlockPosition.y, cubeBlockPosition.z + 1)
                : new Vector3Int(cubeBlockPosition.x + 1, cubeBlockPosition.y, cubeBlockPosition.z);

            if (neighbourCubePosition.x == World.chunkSize || neighbourCubePosition.z == World.chunkSize)
            {
                neighbourPosition.z += 0.5f * (float)(Math.Sqrt(3)) / 2;
                neighbourPosition.x += 0.75f;
            }
        }
        else if (blockSide == BlockSideEnum.RIGHT_FRONT)
        {
            neighbourCubePosition = cubeBlockPosition.x % 2 == 0
                ? new Vector3Int(cubeBlockPosition.x + 1, cubeBlockPosition.y, cubeBlockPosition.z)
                : new Vector3Int(cubeBlockPosition.x + 1, cubeBlockPosition.y, cubeBlockPosition.z - 1);

            if (neighbourCubePosition.x == World.chunkSize || neighbourCubePosition.z == -1)
            {
                neighbourPosition.z -= 0.5f * (float)(Math.Sqrt(3)) / 2;
                neighbourPosition.x += 0.75f;
            }
        }
        else if (blockSide == BlockSideEnum.TOP)
        {
            neighbourCubePosition = new Vector3Int(cubeBlockPosition.x, cubeBlockPosition.y + 1, cubeBlockPosition.z);
        }
        else
        {
            neighbourCubePosition = new Vector3Int(cubeBlockPosition.x, cubeBlockPosition.y - 1, cubeBlockPosition.z);
        }

        

        Block[,,] chunkBlocks = chunkParent.chunkBlocks;

        if (neighbourCubePosition.x < 0 || neighbourCubePosition.x >= World.chunkSize ||
           neighbourCubePosition.y < 0 || neighbourCubePosition.y >= World.chunkSize ||
           neighbourCubePosition.z < 0 || neighbourCubePosition.z >= World.chunkSize)
        {
            Biome biome = BiomeUtils.SelectBiome(neighbourPosition.x, neighbourPosition.z);
            BlockType biomeBlock = biome.GenerateTerrain(neighbourPosition.x, neighbourPosition.y, neighbourPosition.z);

            return biomeBlock.IsTranslucent || biomeBlock.IsTranslucent;

            /*Vector3 neighbourChunkPosition = this.chunkParent.chunkObject.transform.position;
            neighbourChunkPosition.x += (neighbourCubePosition.x - blockPosition.x) * World.chunkSize;
            neighbourChunkPosition.y += (neighbourCubePosition.y - blockPosition.y) * World.chunkSize;
            neighbourChunkPosition.z += (neighbourCubePosition.z - blockPosition.z) * World.chunkSize;

            neighbourChunkPosition.x = neighbourChunkPosition.x / 10 / 0.75f;
            neighbourChunkPosition.y = neighbourChunkPosition.y / 10 / 0.5f;
            neighbourChunkPosition.z = neighbourChunkPosition.z / 10 / (float)(Math.Sqrt(3)) / 2;

            string neighbourChunkName = World.GetChunkName((int)neighbourChunkPosition.x, (int)neighbourChunkPosition.y, (int)neighbourChunkPosition.z);

            if (World.chunks.TryGetValue(neighbourChunkName, out Chunk neighbourChunk))
            {
                chunkBlocks = neighbourChunk.chunkBlocks;
            }
            else
            {
                return false;
            }*/
        }

        if (neighbourCubePosition.x < 0) neighbourCubePosition.x = World.chunkSize - 1;
        if (neighbourCubePosition.y < 0) neighbourCubePosition.y = World.chunkSize - 1;
        if (neighbourCubePosition.z < 0) neighbourCubePosition.z = World.chunkSize - 1;
        if (neighbourCubePosition.x >= World.chunkSize) neighbourCubePosition.x = 0;
        if (neighbourCubePosition.y >= World.chunkSize) neighbourCubePosition.y = 0;
        if (neighbourCubePosition.z >= World.chunkSize) neighbourCubePosition.z = 0;

        var neighbourBlockType = chunkBlocks[(int)neighbourCubePosition.x, (int)neighbourCubePosition.y, (int)neighbourCubePosition.z].blockType;

        if (neighbourBlockType.IsTranslucent && !neighbourBlockType.IsTransparent && this.blockType.IsTranslucent)
        {
            return false;
        }

        return neighbourBlockType.IsTransparent || neighbourBlockType.IsTranslucent;

    }

    private void GenerateBlockSide(BlockSideEnum side)
    {
        switch (side)
        {
            case BlockSideEnum.FRONT:
                foreach(Vector3 vertex in verticesFront)
                {
                    chunkParent.vertices.Add(blockPosition + vertex);
                }
                break;
            case BlockSideEnum.BACK:
                foreach (Vector3 vertex in verticesBack)
                {
                    chunkParent.vertices.Add(blockPosition + vertex);
                }
                break;
            case BlockSideEnum.LEFT_BACK:
                foreach (Vector3 vertex in verticesLeftBack)
                {
                    chunkParent.vertices.Add(blockPosition + vertex);
                }
                break;
            case BlockSideEnum.LEFT_FRONT:
                foreach (Vector3 vertex in verticesLeftFront)
                {
                    chunkParent.vertices.Add(blockPosition + vertex);
                }
                break;
            case BlockSideEnum.RIGHT_BACK:
                foreach (Vector3 vertex in verticesRightBack)
                {
                    chunkParent.vertices.Add(blockPosition + vertex);
                }
                break;
            case BlockSideEnum.RIGHT_FRONT:
                foreach (Vector3 vertex in verticesRightFront)
                {
                    chunkParent.vertices.Add(blockPosition + vertex);
                }
                break;
            case BlockSideEnum.TOP:
                foreach (Vector3 vertex in verticesTop)
                {
                    chunkParent.vertices.Add(blockPosition + vertex);
                }
                break;
            case BlockSideEnum.BOTTOM:
                foreach (Vector3 vertex in verticesBottom)
                {
                    chunkParent.vertices.Add(blockPosition + vertex);
                }
                break;

        }

        foreach(Vector2 blockUV in blockType.GetUv(side))
        {
            chunkParent.uvs.Add(blockUV);
        }

        if(side == BlockSideEnum.TOP || side == BlockSideEnum.BOTTOM)
        {
            foreach(int triangle in trianglesBase)
            {
                if (this.blockType.IsLiquid)
                {
                    chunkParent.liquidTriangles.Add(chunkParent.vertexIndex + triangle);
                }
                else if(this.blockType.IsTransparent || this.blockType.IsTranslucent)
                {
                    chunkParent.transparentTriangles.Add(chunkParent.vertexIndex + triangle);
                }
                else
                {
                    chunkParent.triangles.Add(chunkParent.vertexIndex + triangle);
                }
            }

            chunkParent.vertexIndex += 7;
        }
        else
        {
            foreach(int triangle in trianglesSide)
            {
                if (this.blockType.IsLiquid)
                {
                    chunkParent.liquidTriangles.Add(chunkParent.vertexIndex + triangle);
                }
                else if (this.blockType.IsTransparent || this.blockType.IsTranslucent)
                {
                    chunkParent.transparentTriangles.Add(chunkParent.vertexIndex + triangle);
                }
                else
                {
                    chunkParent.triangles.Add(chunkParent.vertexIndex + triangle);
                }
            }

            chunkParent.vertexIndex += 4;
        }


    }

}
