using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player {
    public class PlayerMovementController : MonoBehaviour
    {
        CharacterController characterController;
        [SerializeField] float walkSpeed = 7f;
        [SerializeField] float runSpeed = 12f;
        [SerializeField] float crouchSpeed = 3f;
        [SerializeField] float proneSpeed = 1f;
        float playerHeight;
        float playerRadius;
        float xMovement;
        float zMovement;
        [SerializeField] private AnimationCurve jumpCurve;
        [SerializeField] private float jumpMultiplier = 300f;
        private bool isJumping = false;
        private bool isFalling = false;
        public float FallingThreshold = -10f;
        PlayerHealth playerHealth;
      
        // Start is called before the first frame update
        void Start()
        {
            characterController = GetComponent<CharacterController>();
            playerHealth = GetComponent<PlayerHealth>();
            playerHeight = characterController.height;
            playerRadius = characterController.radius;
        }

        // Update is called once per frame
        void Update()
        {
            InputProcess();
            CheckFalling();
            if (isFalling) {
                playerHealth.TakeDamage(50*Time.deltaTime);    
            }
        }

        private void CheckFalling()
        {
            if (characterController.velocity.y < FallingThreshold)
            {
                isFalling = true;
            }
            else {
                isFalling = false;
            }
        }

        private void InputProcess()
        {

            xMovement = Input.GetAxis("Horizontal");
            zMovement = Input.GetAxis("Vertical");
            bool isrun = GetComponent<Animator>().GetBool("run");
            bool forward = Input.GetKey("w");
            bool walk = GetComponent<Animator>().GetBool("walk");
            bool run = Input.GetKey("left shift");

            //Run
            if (Input.GetKey(KeyCode.LeftShift))
            {
                StandPlayer();
                MovePlayer(runSpeed);
            }
            else if (Input.GetKey(KeyCode.C))
            {
                CrouchPlayer();
                MovePlayer(crouchSpeed);
                return;
            }
            else if (Input.GetKey(KeyCode.Z))
            {
                PronePlayer();
                MovePlayer(proneSpeed);
                return;
            }
            else
            {
                StandPlayer();
                MovePlayer(walkSpeed);
            }
            //Jump
            if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
            {
                isJumping = true;
                StartCoroutine(JumpPlayer());
            }
            if (!walk && forward)
            {
                GetComponent<Animator>().SetBool("walk", true);
                //animt.SetBool("walk", true);
            }
            if (walk && !forward)
            {
                //animt.SetBool("walk", false);
                GetComponent<Animator>().SetBool("walk", false);
            }
            if (!isrun && (forward && run))
            {
               //animt.SetBool("run", true);
                GetComponent<Animator>().SetBool("run", true);

            }
            if (isrun && (!forward || !run))
            {
                //animt.SetBool("run", false);
                GetComponent<Animator>().SetBool("run", false);
            }
        }

        private void PronePlayer()
        {
            characterController.height = playerHeight * 0.1f;
            characterController.radius = playerRadius * 0.1f;
            transform.position = new Vector3(transform.position.x, transform.position.y + ((characterController.height - playerHeight) / 2), transform.position.z);
        }

        private void StandPlayer()
        {
            characterController.radius = playerRadius;
            characterController.height = playerHeight;
            transform.position = new Vector3(transform.position.x, transform.position.y + ((characterController.height - playerHeight) / 2), transform.position.z);
        }

        private void CrouchPlayer()
        {
            characterController.radius = playerRadius * 0.5f;
            characterController.height = playerHeight * 0.5f;
            transform.position = new Vector3(transform.position.x, transform.position.y + ((characterController.height - playerHeight) / 2), transform.position.z);
        }

        private IEnumerator JumpPlayer()
        {
            characterController.slopeLimit = 90f;
            float timeInAir = .0f;
            do
            {
                float jumpVelocity = jumpCurve.Evaluate(timeInAir);
                characterController.Move(transform.up * jumpVelocity * jumpMultiplier * Time.deltaTime);
                timeInAir += Time.deltaTime;
                yield return null;
            } while (!characterController.isGrounded && characterController.collisionFlags != CollisionFlags.Above);
            isJumping = false;
            characterController.slopeLimit = 45f;
        }

        private void MovePlayer(float speed)
        {
            Vector3 move = transform.forward * zMovement + transform.right * xMovement;
            characterController.SimpleMove(move * speed); // SimpleMove is framerate independent
        }
    }
}

