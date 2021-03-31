using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    CharacterController characterController;
    [SerializeField] float speed=12f;
    float xMovement;
    float zMovement;
    [SerializeField] private AnimationCurve jumpCurve;
    [SerializeField] private float jumpMultiplier=300f;
    private bool isJumping=false;
    private float runMultiplier=2;

    // Start is called before the first frame update
    void Start()
    {
        characterController=GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        InputProcess();
    }

    private void InputProcess()
    {
        xMovement = Input.GetAxis("Horizontal");
        zMovement = Input.GetAxis("Vertical");
        MovePlayer();
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping) {
            isJumping = true;
            StartCoroutine(PlayerJump());
        }
    }

    private IEnumerator PlayerJump()
    {
        characterController.slopeLimit=90f;
        float timeInAir = .0f;
        do {
            float jumpVelocity = jumpCurve.Evaluate(timeInAir);
            characterController.Move(transform.up * jumpVelocity * jumpMultiplier * Time.deltaTime);
            timeInAir += Time.deltaTime;
            yield return null;
        } while (!characterController.isGrounded && characterController.collisionFlags!=CollisionFlags.Above);
        isJumping = false;
        characterController.slopeLimit=45f;
    }

    private void MovePlayer()
    {
        Vector3 move = transform.forward * zMovement + transform.right * xMovement;
        characterController.SimpleMove(move * speed); // SimpleMove is framerate independent
    }
}
