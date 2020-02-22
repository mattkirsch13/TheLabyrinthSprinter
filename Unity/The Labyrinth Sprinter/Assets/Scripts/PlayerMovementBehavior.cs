using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementBehavior : MonoBehaviour
{
    public float maxSpeed = 0.5f;
    public Transform PlayerBodyTransf;
    //CapsuleCollider PlayerBodyCollider;

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

        // Create a "Desired" Movement Vector
        Vector3 movementIn = new Vector3(yPos && yNeg ? 0.0f : yPos ? 1.0f : yNeg ? -1.0f : 0.0f, 0.0f, xPos && xNeg ? 0.0f : xPos ? 1.0f : xNeg ? -1.0f : 0.0f);
        movementIn = maxSpeed * Time.deltaTime * Vector3.Normalize(movementIn);
        Debug.Log("Moving x: " + movementIn.x + " z: " + movementIn.z);


        // Check what components are viable

        // Move based off those components
        PlayerBodyTransf.Translate(movementIn);
    }
}
