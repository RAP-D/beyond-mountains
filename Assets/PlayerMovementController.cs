using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    CharacterController characterController;
    [SerializeField] float walkSpeed=7f;
    [SerializeField] float runSpeed= 12f;
    [SerializeField] float crouchSpeed = 3f;
    [SerializeField] float proneSpeed = 1f;
    float playerHeight;
    float playerRadius;
    float xMovement;
    float zMovement;
    [SerializeField] private AnimationCurve jumpCurve;
    [SerializeField] private float jumpMultiplier=300f;
    private bool isJumping=false;

    // Start is called before the first frame update
    void Start()
    {
        characterController=GetComponent<CharacterController>();
        playerHeight = characterController.height;
        playerRadius = characterController.radius;
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
        //Run
        if (Input.GetKey(KeyCode.LeftShift))
        {
            PlayerStand();
            MovePlayer(runSpeed);
        }
        else if (Input.GetKey(KeyCode.C))
        {
            PlayerCrouch();
            MovePlayer(crouchSpeed);
        }
        else if(Input.GetKey(KeyCode.Z)){
            PlayerProne();
            MovePlayer(proneSpeed);

        }else{
            PlayerStand();
            MovePlayer(walkSpeed);
        }
        //Jump
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping) {
            print("aaa");
            isJumping = true;
            StartCoroutine(PlayerJump());
        }
    }

    private void PlayerProne()
    {
        characterController.height = playerHeight * 0.1f;
        characterController.radius = playerRadius * 0.1f;
        transform.position = new Vector3(transform.position.x, transform.position.y + ((characterController.height - playerHeight) / 2), transform.position.z);
    }

    private void PlayerStand()
    {
        characterController.radius = playerRadius;
        characterController.height = playerHeight;
        transform.position =new Vector3( transform.position.x, transform.position.y+((characterController.height - playerHeight) / 2),transform.position.z);
    }

    private void PlayerCrouch()
    {
        characterController.radius = playerRadius * 0.5f;
        characterController.height = playerHeight * 0.5f;
        transform.position = new Vector3(transform.position.x, transform.position.y + ((characterController.height - playerHeight) / 2), transform.position.z);
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

    private void MovePlayer(float speed)
    {
        Vector3 move = transform.forward * zMovement + transform.right * xMovement;
        characterController.SimpleMove(move * speed); // SimpleMove is framerate independent
    }
}
