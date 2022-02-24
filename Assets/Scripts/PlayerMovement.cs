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

    [BoxGroup("References")]
    [SerializeField]
    private Camera cam;

    [BoxGroup("References")]
    [SerializeField]
    private new SpriteRenderer renderer;

    private new Rigidbody2D rigidbody;

    private bool facingRight = true;

    public bool playerInput { get; private set; }

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        playerInput = Mathf.Abs(Input.GetAxis("Vertical")) > 0 || Mathf.Abs(Input.GetAxis("Horizontal")) > 0;

        if (playerInput)
            Move();
        else
            StopMove();

        FlipTowardsMousePosition();
    }

    private void Move()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector2 movementVector = new Vector2(horizontal, vertical);

        rigidbody.velocity = movementVector * movementSpeed;
    }

    private void StopMove()
    {
        rigidbody.velocity = Vector2.zero;
    }

    private void FlipTowardsMousePosition()
    {
        Vector2 mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);

        if (mousePosition.x > transform.position.x && !facingRight)
        {
            Flip();
        }
        else if (mousePosition.x < transform.position.x && facingRight)
        {
            Flip();
        }
    }

    private void Flip()
    {
        if (renderer == null) return;

        // Switch the way the player is labelled as facing.
        facingRight = !facingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = renderer.transform.localScale;
        theScale.x *= -1;
        renderer.transform.localScale = theScale;
    }

}
