using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    // Declaring variables relying on input
    [SerializeField] private string horizontalInput;    // Gets horizontal axis of an object
    [SerializeField] private string verticalInput;      // Gets vertical axis of an object
    [SerializeField] private float movementSpeed;       // User-changable movement speed
    [SerializeField] private AnimationCurve jumpCurve;  // Jump trajectory curve
    [SerializeField] private float jumpHeight;          // Y axis movement multiplier
    [SerializeField] private KeyCode jumpKey;           // Key to trigger jump action

    private CharacterController charController;         // Character controller to manipulate the object
    private bool isJumping;                             // Boolean to store jumping status

	// Use this for initialization
	void Awake()
    {
        charController = GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update ()
    { 
        MovePlayer();   // MovePlayer function called once per frame to decide whether the player should move
    }

    private void MovePlayer()
    {
        // Calculating the amount of movement to perform
        float horizontalMovement = Input.GetAxis(horizontalInput) * movementSpeed * Time.deltaTime; // Horizontal movement
        float verticalMovement = Input.GetAxis(verticalInput) * movementSpeed * Time.deltaTime;     // Vertical movement

        // Tranforming the amount of movement into 3 dimensional vector
        Vector3 forwardMovement = transform.forward * verticalMovement; // transform.forward = 1(forward) or -1(backward)
        Vector3 rightMovement = transform.right * horizontalMovement;   // transform.forward = 1(right) or -1(left)

        if(isJumping)   // Reduce movement speed when in air
        {
            forwardMovement /= 100;
            rightMovement /= 100;
        }
        
        // Performing the movement
        charController.SimpleMove(forwardMovement + rightMovement); // Combining the vectors to get 1 directional vector

        Jump(); // Decides if player can jump or is already jumping
    }

    private void Jump()
    {
        if (Input.GetKeyDown(jumpKey) && !isJumping)    // If player pressed the jumpKey and is not already jumping, preform jump
        {
            isJumping = true;
            StartCoroutine(JumpAction());    // Starts a routine that can be stopped with a yield statement
        }
    }

    private IEnumerator JumpAction()    // Performes the jump. Must return IEnumerator in order for the iteration to work
    {
        float timeInAir = 0.0f;

        do
        {
            float jumpForce = jumpCurve.Evaluate(timeInAir);                            // Evaluates how much force should be applied throughout the jump based on the jump curve
            charController.Move(Vector3.up * jumpForce * jumpHeight * Time.deltaTime);  // Vector3.up = +1 on Y axis
            timeInAir += Time.deltaTime;                                                // Time flow

            yield return null;  // Terminates the coroutine
        }

        while (!charController.isGrounded && charController.collisionFlags != CollisionFlags.Above);    // Checks collisions and ends jumping procedure
            isJumping = false;
    }
}
