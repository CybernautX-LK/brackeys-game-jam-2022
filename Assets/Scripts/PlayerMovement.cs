using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    public enum SpeedMode { Normal, Fast, Slow }

    [BoxGroup("Settings")]
    [SerializeField]
    private SpeedMode speedMode;

    [BoxGroup("Settings")]
    [Range(0.1f, 3.0f)]
    [SerializeField]
    private float forwardMovementSpeedFactor = 1.0f;

    [BoxGroup("Settings")]
    [Range(0.1f, 3.0f)]
    [SerializeField]
    private float backwardsMovementSpeedFactor = 0.5f;

    [BoxGroup("Settings")]
    [Range(0.1f, 3.0f)]
    [SerializeField]
    private float normalMovementSpeed = 1.0f;

    [BoxGroup("Settings")]
    [Range(0.1f, 3.0f)]
    [SerializeField]
    private float fastMovementSpeed = 1.0f;

    [BoxGroup("Settings")]
    [Range(0.1f, 3.0f)]
    [SerializeField]
    private float slowMovementSpeed = 1.0f;

    [BoxGroup("Audio")]
    [SerializeField]
    private AudioObject OnPlayerMoveSFX;
    
    [BoxGroup("References")]
    [SerializeField]
    private new SpriteRenderer renderer;

    private Camera cam;
    private new Rigidbody2D rigidbody;
    private Animator animator;

    private bool facingRight = true;
    private Vector2 movement;
    Vector2 mousePosition;

    public bool isRunning;
    public bool isMovingVertical;
    public bool isMovingBackwards;

    private float movementSpeed;

    public bool playerInput { get; private set; }

    private void Awake()
    {
        cam = Camera.main;
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();

        SetSpeedMode(SpeedMode.Normal);
    }

    private void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        movement = new Vector2(horizontal, vertical);
        mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);

        float verticalAnimatorValue = (mousePosition - (Vector2)this.transform.position).normalized.y;

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", Mathf.Clamp(verticalAnimatorValue, -1, 1));
        animator.SetFloat("Speed", movement.sqrMagnitude);

    }

    private void FixedUpdate()
    {
        playerInput = (movement != Vector2.zero);//Mathf.Abs(Input.GetAxis("Vertical")) > 0 || Mathf.Abs(Input.GetAxis("Horizontal")) > 0;

        if (playerInput)
            Move();

        FlipTowardsMousePosition();
    }

    public void SetSpeedMode(SpeedMode speedMode)
    {
        switch (speedMode)
        {
            case SpeedMode.Normal:
                movementSpeed = normalMovementSpeed;
                break;
            case SpeedMode.Fast:
                movementSpeed = fastMovementSpeed;
                break;
            case SpeedMode.Slow:
                movementSpeed = slowMovementSpeed;
                break;
            default:
                break;
        }

        this.speedMode = speedMode;
    }

    private void Move()
    {       
        isMovingVertical = movement.x != 0 && movement.y != 0;
        isMovingBackwards = movement.x > 0 && !facingRight || movement.x < 0 && facingRight || movement.y > 0 && mousePosition.y < transform.position.y || movement.y < 0 && mousePosition.y > transform.position.y;

        float movementMultiplier = (isMovingBackwards) ? backwardsMovementSpeedFactor : forwardMovementSpeedFactor;
        float verticalMovementMultiplier = isMovingVertical ? 0.75f : 1.0f;

        //movementSpeed = (isRunning && !isMovingBackwards) ? runMovementSpeed : movementSpeed;            

        rigidbody.MovePosition(rigidbody.position + movement * movementSpeed * movementMultiplier * verticalMovementMultiplier * Time.fixedDeltaTime);

        //if (OnPlayerMoveSFX != null)
        //    AudioManager2.Instance.PlayOneShot(OnPlayerMoveSFX);
    }

    private void FlipTowardsMousePosition()
    {       
        if (mousePosition.x >= transform.position.x && !facingRight)
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
