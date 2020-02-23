using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementBehavior : MonoBehaviour
{
    public float movAcceleration;
    public float maxSpeed;
    public Transform PlayerBodyTransf;
    public Rigidbody PlayerRigidBody;

    float sprintMod = 1.75f;

    // Start is called before the first frame update
    void Start()
    {
        
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

        // Create a "Desired" Movement Vector
        Vector3 movementIn = new Vector3(yPos && yNeg ? 0.0f : yPos ? 1.0f : yNeg ? -1.0f : 0.0f, 0.0f, xPos && xNeg ? 0.0f : xPos ? 1.0f : xNeg ? -1.0f : 0.0f);

        if (sprint)
        {
            movementIn = movAcceleration * sprintMod * Vector3.Normalize(movementIn);
            
            // Move based off those components
            if (Mathf.Abs(PlayerRigidBody.velocity.magnitude) < maxSpeed)
            {
                PlayerRigidBody.AddRelativeForce(movementIn, ForceMode.Acceleration);
            }
        }
        else
        {
            movementIn = movAcceleration * Vector3.Normalize(movementIn);
            
            // Move based off those components
            if (Mathf.Abs(PlayerRigidBody.velocity.magnitude) < sprintMod * maxSpeed)
            {
                PlayerRigidBody.AddRelativeForce(movementIn, ForceMode.Acceleration);
            }
        }

        if (Mathf.Abs(PlayerRigidBody.velocity.magnitude) > 1e-2f)
        {
            //Debug.Log("Moving at: " + Mathf.Abs(PlayerRigidBody.velocity.magnitude));
        }

        PlayerRigidBody.velocity = PlayerRigidBody.velocity * Mathf.Clamp(0.90f, 0.0f, 0.999f);        
    }
}
