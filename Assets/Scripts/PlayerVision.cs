using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVision : MonoBehaviour {

    // Serializable variables
    [SerializeField] private string inputX, inputY;     // X and Y axis of the camera to be rotated
    [SerializeField] private float mouseSensitivity;    // Changable sensitivity
    [SerializeField] private Transform playerBody;      // Transform object of a parent of the camera object

    private float xClamp;   // POV limit value

	// Use this for initialization
	private void Awake()
    {
        LockCursor();   // Enables infinite view movement by locking the cursor in place
        xClamp = 0.0f;  // Initializes the variable
    }

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;   
    }

	// Update is called once per frame
	void Update ()
    {
        CameraRotation();   // Starts the camera rotation control routine
	}

    private void CameraRotation()
    {
        float viewSpeedX = Input.GetAxis(inputX) * mouseSensitivity * Time.deltaTime;
        float viewSpeedY = Input.GetAxis(inputY) * mouseSensitivity * Time.deltaTime;

        xClamp += viewSpeedY;           // Stores the degree on Y axis where the movement should be performed

        if(xClamp > 90.0f)              // If the degree should be bigger than 90°(straight up), the movement can't be performed
        {
            xClamp = 90.0f;
            viewSpeedY = 0.0f;          // Stops the movement
            SetXRotation(270.0f);       // Prevents exceeding the view limit(up)
        }

        else if(xClamp < -90.0f)        // If the degree should be smaller than -90°(straight down), the movement can't be performed
        {
            xClamp = -90.0f;
            viewSpeedY = 0.0f;          // Stops the movement
            SetXRotation(90.0f);        // Prevents exceeding the view limit(down)
        }

        transform.Rotate(Vector3.left * viewSpeedY);    // Rotates the camera to look up or down
        playerBody.Rotate(Vector3.up * viewSpeedX);     // Rotates the camera parent to look in horizontal directions
    }

    private void SetXRotation(float degree)  // Sets the value of X/Y axis angle degree 
    {
        Vector3 axisDegree = transform.eulerAngles; // Angles between axes
        axisDegree.x = degree;                      // X/Y degree
        transform.eulerAngles = axisDegree;         // Save 
    }



}
