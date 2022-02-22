using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [BoxGroup("Settings")]
    [Range(0.1f, 3.0f)]
    [SerializeField]
    private float movementSpeed = 1.0f;

    [BoxGroup("Settings")]
    [Range(1.0f, 10.0f)]
    [SerializeField]
    private float turnSpeed = 8.0f;

    [BoxGroup("References")]
    [SerializeField]
    private Camera cam;

    private new Rigidbody2D rigidbody;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Move();
        RotateTowardsMousePosition();
    }

    private void Move()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector2 movementVector = new Vector2(horizontal, vertical);

        rigidbody.velocity = movementVector * movementSpeed;
    }
    private void RotateTowardsMousePosition()
    {
        if (cam == null) return;

        Vector3 mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
        Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, mousePosition - transform.position);

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
    }
}
