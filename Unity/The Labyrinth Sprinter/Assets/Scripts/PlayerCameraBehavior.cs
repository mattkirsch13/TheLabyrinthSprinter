using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraBehavior : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public const float FOV = 70f;
    float currentFOV;
    public Transform playerBodyTransf;
    public Transform playerCameraTransf;
    public Rigidbody playerRigidBody;

    float xRotation = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        currentFOV = FOV;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        bool sprint = Input.GetButton("Sprint");

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCameraTransf.localRotation = Quaternion.Euler(new Vector3(xRotation, 0.0f, 0.0f));
        playerBodyTransf.Rotate(Vector3.up, mouseX);

        bool moving = playerRigidBody.velocity.sqrMagnitude >= 1e-4f;

        if (sprint && moving)
        {
            if ((currentFOV < FOV * 1.1f) )
            {
                currentFOV += 0.5f;
            }
        }
        else
        {
            if ((currentFOV > FOV))
            {
                currentFOV -= 0.5f;
            }
        }
        Debug.Log(currentFOV);
        
        GetComponent<Camera>().fieldOfView = currentFOV;
    }
}
