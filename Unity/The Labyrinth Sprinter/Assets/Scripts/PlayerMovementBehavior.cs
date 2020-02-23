using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementBehavior : MonoBehaviour
{
    public float movAcceleration;
    public float maxSpeed;
    public float jumpConst;
    public Transform PlayerBodyTransf;
    public Rigidbody PlayerRigidBody;
    public GameObject maze;

    float sprintMod = 1.75f;
    bool grounded = false;
    bool jumping = false;
    // Start is called before the first frame update
    void Start()
    {
        
        float cellWidth = maze.GetComponent<Maze>().cellWidth;
        float totalWidth = maze.GetComponent<Maze>().mazeWidth * cellWidth;
        Vector3 startPos = Vector3.zero + Vector3.up * 2;
        PlayerRigidBody.position = startPos;
    }

    // Update is called once per frame
    void Update()
    {
        // Get player input
        bool xPos = Input.GetButton("Forward");
        bool xNeg = Input.GetButton("Backward");
        bool yPos = Input.GetButton("Right");
        bool yNeg = Input.GetButton("Left");

        bool sprint = Input.GetButton("Sprint");
        bool jumpInput = Input.GetButton("Jump");
        float jumpVelocity = 0.0f;
        CapsuleCollider collider = GetComponent<CapsuleCollider>();
        
        


        if (jumpInput && grounded && !jumping)
        { 
             jumpVelocity = jumpConst;
        }
        // Create a "Desired" Movement Vector
        Vector2 movementIn = new Vector2(yPos && yNeg ? 0.0f : yPos ? 1.0f : yNeg ? -1.0f : 0.0f, xPos && xNeg ? 0.0f : xPos ? 1.0f : xNeg ? -1.0f : 0.0f);
        Vector2 horizontal = new Vector2(PlayerRigidBody.velocity.x, PlayerRigidBody.velocity.z);
        if (sprint)
        {
            movementIn = movAcceleration * sprintMod * movementIn.normalized;

            // Move based off those components
            if (Mathf.Abs(horizontal.magnitude) >= maxSpeed)
            {
                movementIn = Vector2.zero;
            }
        }
        else
        {
            movementIn = movAcceleration * movementIn.normalized;

            // Move based off those components
            if (Mathf.Abs(horizontal.magnitude) >= sprintMod * maxSpeed)
            {
                movementIn = Vector2.zero;
            }
        }

        PlayerRigidBody.AddRelativeForce(new Vector3(movementIn.x, 0.0f, movementIn.y), ForceMode.Acceleration);
        PlayerRigidBody.velocity += new Vector3(0.0f, jumpVelocity, 0.0f);
        /*
        if (Mathf.Abs(PlayerRigidBody.velocity.magnitude) > 1e-2f)
        {
            Debug.Log("Moving at: " + Mathf.Abs(PlayerRigidBody.velocity.magnitude));
        }
        */

        // Decay the X and Z velocity without modifying Y
        PlayerRigidBody.velocity = new Vector3(PlayerRigidBody.velocity.x * 0.82f, PlayerRigidBody.velocity.y, PlayerRigidBody.velocity.z * 0.82f);

        // Finally, check if the jump button has been pressed or released
        if (Input.GetButtonDown("Jump"))
        {
            jumping = true;
        }

        if (Input.GetButtonUp("Jump"))
        {
            jumping = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "floor")
        {
            grounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.name == "floor")
        {
            grounded = false;
        }
    }
}
