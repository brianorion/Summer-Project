using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowing : MonoBehaviour
{
    [SerializeField]
    private GameObject thirdPersonCamera;

    private PlayerController playerController;

    private void Awake()
    {
        thirdPersonCamera.SetActive(false);
    }

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        InThirdPerson();
        CameraFollow();
    }

    private void InThirdPerson()
    {
        if(!playerController.inFirstPerson)
        {
            thirdPersonCamera.SetActive(true);
        }
        else
        {
            thirdPersonCamera.SetActive(false);
        }
    }

    private void CameraFollow()
    {
        thirdPersonCamera.transform.position = transform.position;
    }
}