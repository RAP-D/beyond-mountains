using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [SerializeField] public float mouseSensitivity = 5f;
    Camera playerCamera; 
    private float xRotation=0;
    // Start is called before the first frame update
    void Start()
    {
        LockCursor();
        playerCamera = GetComponentInChildren<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        RoatateCamera();
    }

    private void RoatateCamera()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation,-90f,90f);
        playerCamera.transform.localRotation=Quaternion.Euler(xRotation,0f,0f);
        transform.Rotate(transform.up*mouseX);
    }

    void LockCursor() {
        Cursor.lockState = CursorLockMode.Locked;
    }
}
