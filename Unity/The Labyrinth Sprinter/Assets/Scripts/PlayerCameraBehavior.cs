using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraBehavior : MonoBehaviour
{
    public float mouseSensitivity = 100f;

    public Transform playerBodyTransf;
    public Transform playerCameraTransf;

    float xRotation = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCameraTransf.localRotation = Quaternion.Euler(new Vector3(xRotation, 0.0f, 0.0f));
        playerBodyTransf.Rotate(Vector3.up, mouseX);
    }
}
