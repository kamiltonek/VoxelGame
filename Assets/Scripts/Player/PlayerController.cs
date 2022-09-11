using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CharacterController controller;

    public float speed = 10f;
    public float gravity = -9.81f;

    Vector3 velocity;
    void Start()
    {

    }

    void Update()
    {
        Move();
        CheckClick();
    }

    private void CheckClick()
    {
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
            {
                Chunk chunkToEdit;
                string chunkToEditName = World.GenerateChunkName(hit.transform.position);
                if (World.chunks.TryGetValue(chunkToEditName, out chunkToEdit))
                {
                    Vector3Int blockPosition = Vector3Int.FloorToInt(hit.transform.InverseTransformPoint(hit.point) + new Vector3(0.5f, 0.5f, 0.5f) - hit.normal / 2);

                    chunkToEdit.RemoveBlock(blockPosition);
                }
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
            {
                Chunk chunkToEdit;
                string chunkToEditName = World.GenerateChunkName(hit.transform.position);
                if (World.chunks.TryGetValue(chunkToEditName, out chunkToEdit))
                {
                    Vector3Int blockPosition = Vector3Int.FloorToInt(hit.transform.InverseTransformPoint(hit.point) + new Vector3(0.5f, 0.5f, 0.5f) + hit.normal / 2);
                    //Vector3Int blockPosition = new Vector3Int(1, 5, 1);

                    chunkToEdit.AddBlock(blockPosition);
                }
            }
        }
    }

    private void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(x, 0f, z);
        controller.Move(move * speed * Time.deltaTime);
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
