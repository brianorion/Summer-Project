using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float speed = 1;

    private Rigidbody rb;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        Vector3 velocity = rb.velocity;

        velocity.x = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        velocity.z = Input.GetAxis("Vertical") * speed * Time.deltaTime;

        rb.velocity = velocity;
    }
}
