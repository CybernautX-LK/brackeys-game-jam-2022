using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [BoxGroup("Settings")]
    [Range(0.1f, 3.0f)]
    [SerializeField]
    private float movementSpeed = 1.0f;

    private new Rigidbody rigidbody;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 movementVector = new Vector3(horizontal, 0.0f, vertical);

        rigidbody.velocity = movementVector * movementSpeed;
    }
}
