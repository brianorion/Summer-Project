using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float speed = 1f;
    [SerializeField]
    private float rotationSensitivity = 1f;
    [SerializeField]
    private GameObject firstPersonCamera;

    private Rigidbody rb;

    private bool changeState = false;
    
    public bool inFirstPerson = true;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        firstPersonCamera.SetActive(inFirstPerson);
    }

    private void Update()
    {
        // we move no matter what state the character is in.
        Move();

        // this checks if we are in first person or third person. 
        if (Input.GetKeyDown(KeyCode.C) && !changeState)
        {
            changeState = true;
            inFirstPerson = false;
        }
        else if (Input.GetKeyDown(KeyCode.C) && changeState)
        {
            changeState = false;
            inFirstPerson = true;
        }
        IsFirstPerson(inFirstPerson);
    }


    private void IsFirstPerson(bool firstPerson)
    {
        if (firstPerson)
        {
            PlayerRotate();
            firstPersonCamera.SetActive(firstPerson);
        }
        else
        {
            ThirdPersonMode();
            firstPersonCamera.SetActive(firstPerson);
        }
    }

    private void Move()
    {
        if (inFirstPerson)
        {
            // this ensures that the player will face towards and walk towards the forward or right direction of the camera. 
            Vector3 camF = firstPersonCamera.transform.forward;
            Vector3 camR = firstPersonCamera.transform.right;

            camF.y = 0;
            camR.y = 0f;
            camF = camF.normalized;
            camR = camR.normalized;

            Vector3 velocity = camF * Input.GetAxis("Vertical") + camR * Input.GetAxis("Horizontal");
            velocity = velocity.normalized * speed;

            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
        }
        else
        {
            // in third person, we only need to move in terms of the world axis. We don't need to worry about the position of the camera. 
            Vector3 velocity = Vector3.zero;
            velocity.x = Input.GetAxis("Horizontal");
            velocity.z = Input.GetAxis("Vertical");

            velocity = velocity.normalized * speed;

            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
        }
    }

    private void PlayerRotate()
    {
        float yRot = Input.GetAxis("Mouse X");

        Vector3 rotation = new Vector3(0f, yRot, 0f) * rotationSensitivity;

        rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));
        if (firstPersonCamera != null)
        {
            CameraRotate();
        }
    }
    
    private void CameraRotate()
    {
        float xRot = Input.GetAxis("Mouse Y");

        Vector3 rotation = new Vector3(xRot, 0f, 0f) * rotationSensitivity;

        firstPersonCamera.transform.Rotate(-rotation);
    }

    private void ThirdPersonMode()
    {

    }
}
