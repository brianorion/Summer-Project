using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Basic Player stats")]
    [SerializeField]
    private float speed = 1f;
    [SerializeField]
    private float rotationSensitivity = 1f;
    [SerializeField]
    private float jumpForce = 1f;
    [SerializeField]
    private float maximumCameraRotation = 90f;
    [SerializeField]
    private float pickUpDistance = 5f;

    [Header("Associated Objects")]
    [SerializeField]
    private GameObject firstPersonCamera;
    [SerializeField]
    private Transform guide;

    private Rigidbody rb;

    // these are all of the item pick up
    private Rigidbody itemRb;
    private Transform itemTransform;

    private bool changeState = false;
    private bool canPickUp = false;
    private Vector3 jumpVector;

    public bool inFirstPerson = true;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        firstPersonCamera.SetActive(inFirstPerson);
        jumpVector = new Vector3(0f, jumpForce, 0f);
        Cursor.lockState = CursorLockMode.Locked;
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
            Cursor.lockState = CursorLockMode.Locked;
            PlayerRotate();
            ClickAndDrag();
            firstPersonCamera.SetActive(firstPerson);
        }
        else
        {
            // TODO maybe in third person mode there might be something different. 
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

        // the jumping functionality doesn't depend on whether or not the player is in first person or in third person. Thus, I am doing it outside of the if statement. 
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // At the moment, a person can have infinite jumps. Too lazy to fix it so I will do it later. 
            rb.AddForce(jumpVector, ForceMode.Impulse);
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
        float currentX = firstPersonCamera.transform.localEulerAngles.x;
        currentX = Mathf.Clamp(currentX, -270f, maximumCameraRotation);

        firstPersonCamera.transform.localEulerAngles = new Vector3(currentX, 0f, 0f);
    }

    private void ThirdPersonMode()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    private void ClickAndDrag()
    {
        RaycastHit hit;
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = firstPersonCamera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.distance <= pickUpDistance)
                {
                    if (hit.transform.tag != "Ground")
                    {
                        Debug.Log("I pick up something");
                        canPickUp = true;
                        itemRb = hit.rigidbody;
                        itemTransform = hit.transform;


                        itemRb.useGravity = false;
                        itemRb.isKinematic = true;
                        itemTransform.position = guide.position;
                        itemTransform.rotation = guide.rotation;
                        itemTransform.parent = guide;
                    }
                }
            }
        }
        else if(Input.GetMouseButtonUp(0) && canPickUp)
        {
            if (itemRb != null)
            {
                canPickUp = false;
                itemRb.useGravity = true;
                itemRb.isKinematic = false;
                itemTransform.position = guide.position;
                itemTransform.parent = null;
            }
        }
    }
}
