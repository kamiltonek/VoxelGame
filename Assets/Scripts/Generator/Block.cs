using Assets.Scripts.Enums;
using System;
using UnityEngine;

public class Block
{
    private BlockTypeEnum blockType;
    private bool isTransparent;
    private GameObject blockParent;
    private Vector3 blockPosition;
    private Material blockMaterial;

    Vector3[] vertices = new Vector3[14]
    {
        new Vector3( -0.5f,  0.25f,  0),
        new Vector3(-0.25f,  0.25f,  (float)(-0.5 * Math.Sqrt(3) / 2)),
        new Vector3( 0.25f,  0.25f,  (float)(-0.5 * Math.Sqrt(3) / 2)),
        new Vector3(  0.5f,  0.25f,  0f),
        new Vector3( 0.25f,  0.25f,  (float)(0.5 * Math.Sqrt(3) / 2)),
        new Vector3(-0.25f,  0.25f,  (float)(0.5 * Math.Sqrt(3) / 2)),
        new Vector3( -0.5f, -0.25f,  0),
        new Vector3(-0.25f, -0.25f,  (float)(-0.5 * Math.Sqrt(3) / 2)),
        new Vector3( 0.25f, -0.25f,  (float)(-0.5 * Math.Sqrt(3) / 2)),
        new Vector3(  0.5f, -0.25f,  0f),
        new Vector3( 0.25f, -0.25f,  (float)(0.5 * Math.Sqrt(3) / 2)),
        new Vector3(-0.25f, -0.25f,  (float)(0.5 * Math.Sqrt(3) / 2)),
        new Vector3(0, 0.25f,  0),
        new Vector3(0, -0.25f,  0)
    };
    Vector2[] uv = new Vector2[11]
    {
        new Vector2(0.25f, 0.25f),
        new Vector2(0.75f, 0.25f),
        new Vector2(0.25f, 0.75f),
        new Vector2(0.75f, 0.75f),
        new Vector2(0.25f, 1),
        new Vector2(0.75f, 1),
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
        Material blockMaterial)
    {
        this.blockType = blockType;
        this.blockParent = blockParent;
        this.blockPosition = blockPosition;
        this.blockMaterial = blockMaterial;

        isTransparent = 
            blockType == BlockTypeEnum.AIR || 
            blockType == BlockTypeEnum.WATER 
            ? true : false;
    }

    public void CreateBlock()
    {
        CreateBlockSide(BlockSideEnum.FRONT);
        CreateBlockSide(BlockSideEnum.BACK);
        CreateBlockSide(BlockSideEnum.LEFT_BACK);
        CreateBlockSide(BlockSideEnum.LEFT_FRONT);
        CreateBlockSide(BlockSideEnum.RIGHT_BACK);
        CreateBlockSide(BlockSideEnum.RIGHT_FRONT);
        CreateBlockSide(BlockSideEnum.TOP);
        CreateBlockSide(BlockSideEnum.BOTTOM);
    }

    private void CreateBlockSide(BlockSideEnum side)
    {
        Mesh mesh = new Mesh();
        mesh = GenerateBlockSide(mesh, side);

        GameObject blockSide = new GameObject(side.ToString());
        blockSide.transform.position = blockPosition;
        blockSide.transform.parent = blockParent.transform;

        MeshFilter meshFilter = (MeshFilter)blockSide.AddComponent(typeof(MeshFilter));
        meshFilter.mesh = mesh;
    }

    private Mesh GenerateBlockSide(Mesh mesh, BlockSideEnum side)
    {
        switch (side)
        {
            case BlockSideEnum.FRONT:
                mesh.vertices = new Vector3[] { vertices[5], vertices[4], vertices[10], vertices[11] };
                mesh.normals = new Vector3[] { Vector3.forward, Vector3.forward, Vector3.forward, Vector3.forward };
                mesh.uv = new Vector2[] { uv[3], uv[2], uv[0], uv[1] };
                mesh.triangles = new int[] { 3, 1, 0, 3, 2, 1 };
                break;
            case BlockSideEnum.BACK:
                mesh.vertices = new Vector3[] { vertices[2], vertices[1], vertices[7], vertices[8]};
                mesh.normals = new Vector3[] { Vector3.back, Vector3.back, Vector3.back, Vector3.back };
                mesh.uv = new Vector2[] { uv[3], uv[2], uv[0], uv[1] };
                mesh.triangles = new int[] { 3, 1, 0, 3, 2, 1 };
                break;
            case BlockSideEnum.LEFT_BACK:
                mesh.vertices = new Vector3[] { vertices[1], vertices[0], vertices[6], vertices[7] };
                mesh.normals = new Vector3[] { vectorLeftBack, vectorLeftBack, vectorLeftBack, vectorLeftBack };
                mesh.uv = new Vector2[] { uv[3], uv[2], uv[0], uv[1] };
                mesh.triangles = new int[] { 3, 1, 0, 3, 2, 1 };
                break;
            case BlockSideEnum.LEFT_FRONT:
                mesh.vertices = new Vector3[] { vertices[0], vertices[5], vertices[11], vertices[6] };
                mesh.normals = new Vector3[] { vectorLeftFront, vectorLeftFront, vectorLeftFront, vectorLeftFront };
                mesh.uv = new Vector2[] { uv[3], uv[2], uv[0], uv[1] };
                mesh.triangles = new int[] { 3, 1, 0, 3, 2, 1 };
                break;
            case BlockSideEnum.RIGHT_BACK:
                mesh.vertices = new Vector3[] { vertices[3], vertices[2], vertices[8], vertices[9] };
                mesh.normals = new Vector3[] { vectorRightBack, vectorRightBack, vectorRightBack, vectorRightBack };
                mesh.uv = new Vector2[] { uv[3], uv[2], uv[0], uv[1] };
                mesh.triangles = new int[] { 3, 1, 0, 3, 2, 1 };
                break;
            case BlockSideEnum.RIGHT_FRONT:
                mesh.vertices = new Vector3[] { vertices[4], vertices[3], vertices[9], vertices[10] };
                mesh.normals = new Vector3[] { vectorRightFront, vectorRightFront, vectorRightFront, vectorRightFront };
                mesh.uv = new Vector2[] { uv[3], uv[2], uv[0], uv[1] };
                mesh.triangles = new int[] { 3, 1, 0, 3, 2, 1 };
                break;
            case BlockSideEnum.TOP:
                mesh.vertices = new Vector3[] { vertices[0], vertices[1], vertices[2], vertices[3], vertices[4], vertices[5], vertices[12] };
                mesh.normals = new Vector3[] { Vector3.up, Vector3.up, Vector3.up, Vector3.up, Vector3.up, Vector3.up, Vector3.up };
                mesh.uv = new Vector2[] { uv[5], uv[4], uv[6], uv[8], uv[9], uv[7], uv[10] };
                mesh.triangles = new int[] { 1, 0, 6, 2, 1, 6, 3, 2, 6, 4, 3, 6, 5, 4, 6, 0, 5, 6 };
                break;
            case BlockSideEnum.BOTTOM:
                mesh.vertices = new Vector3[] { vertices[6], vertices[7], vertices[8], vertices[9], vertices[10], vertices[11], vertices[13] };
                mesh.normals = new Vector3[] { Vector3.down, Vector3.down, Vector3.down, Vector3.down, Vector3.down, Vector3.down, Vector3.down };
                mesh.uv = new Vector2[] { uv[5], uv[4], uv[6], uv[8], uv[9], uv[7], uv[10] };
                mesh.triangles = new int[] { 0, 1, 6, 1, 2, 6, 2, 3, 6, 3, 4, 6, 4, 5, 6, 5, 0, 6 };
                break;

        }

        return mesh;
    }

}
