using UnityEngine;

public class Player2 : MonoBehaviour
{
    [SerializeField] private Player2Input gameInput;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpSpeed = 5f;
    [SerializeField] private Rigidbody2D rigidbody2D;
    [SerializeField] private Animator animator;

    private Vector2 inputVector;

    [Header("Jump Settings")]
    [SerializeField] private int maxJumps = 2; // 1 = single, 2 = double jump
    private int jumpCount = 0;

    [Header("Slope Settings")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private float slopeCheckDistance = 0.5f;
    [SerializeField] private float maxSlopeAngle = 45f;
    [SerializeField] private PhysicsMaterial2D noFriction;
    [SerializeField] private PhysicsMaterial2D fullFriction;

    private bool Grounded;

    private float slopeDownAngle;
    private float slopeDownAngleOld;
    private float slopeSideAngle;
    private Vector2 slopeNormalPerpendicular;

    private bool isOnSlope;
    private bool canWalkOnSlope;

    private void Awake()
    {
        gameInput.OnPlayer2Jump += GameInput_OnPlayer2Jump;
    }

    private void Update()
    {
        inputVector = gameInput.GetMovement2VectorNormalized();

        var effects = GetComponent<Player2Effects>();
        if (effects != null)
            inputVector = effects.ProcessInput(inputVector);

        if (inputVector != Vector2.zero)
        {
            animator.SetBool("IsRunning", true);
        }
        else
        {
            animator.SetBool("IsRunning", false);
        }

        if (inputVector.x > 0.1f)
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        else if (inputVector.x < -0.1f)
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }

    private void FixedUpdate()
    {

        // Reset jump count when grounded
        if (IsGrounded())
        {
            jumpCount = 0;

            if (isOnSlope && inputVector.y == 0f)
                rigidbody2D.sharedMaterial = fullFriction;
            else
                rigidbody2D.sharedMaterial = noFriction;
        }

        SlopeCheck();

        // Movement logic
        if (IsGrounded() && !isOnSlope)
        {
            // Flat ground
            rigidbody2D.linearVelocity = new Vector2(inputVector.x * moveSpeed, rigidbody2D.linearVelocityY);
        }
        else if (IsGrounded() && isOnSlope && canWalkOnSlope)
        {
            // On slope surface
            Vector2 slopeVelocity = new Vector2(
                moveSpeed * slopeNormalPerpendicular.x * (-inputVector.x),
                moveSpeed * slopeNormalPerpendicular.y * (-inputVector.x)
            );
            rigidbody2D.linearVelocity = slopeVelocity;
        }
        else
        {
            // Airborne
            rigidbody2D.linearVelocity = new Vector2(inputVector.x * moveSpeed, rigidbody2D.linearVelocityY);
        }
    }

    private void GameInput_OnPlayer2Jump(object sender, System.EventArgs e)
    {
        // Jump if grounded or within allowed jump count
        if (IsGrounded() || jumpCount < maxJumps - 1)
        {
            animator.Play("Jump");
            rigidbody2D.linearVelocityY = jumpSpeed;
            jumpCount++;
        }
    }

    // ---------------- SLOPE DETECTION ---------------- //

    private void SlopeCheck()
    {
        SlopeCheckHorizontal(groundCheck.position);
        SlopeCheckVertical(groundCheck.position);
    }

    private void SlopeCheckHorizontal(Vector2 checkPos)
    {
        RaycastHit2D slopeHitFront = Physics2D.Raycast(checkPos, transform.right, slopeCheckDistance, whatIsGround);
        RaycastHit2D slopeHitBack = Physics2D.Raycast(checkPos, -transform.right, slopeCheckDistance, whatIsGround);

        if (slopeHitFront)
        {
            isOnSlope = true;
            slopeSideAngle = Vector2.Angle(slopeHitFront.normal, Vector2.up);
        }
        else if (slopeHitBack)
        {
            isOnSlope = true;
            slopeSideAngle = Vector2.Angle(slopeHitBack.normal, Vector2.up);
        }
        else
        {
            slopeSideAngle = 0.0f;
            isOnSlope = false;
        }
    }

    private void SlopeCheckVertical(Vector2 checkPos)
    {
        RaycastHit2D hit = Physics2D.Raycast(checkPos, Vector2.down, slopeCheckDistance, whatIsGround);

        if (hit)
        {
            slopeNormalPerpendicular = Vector2.Perpendicular(hit.normal).normalized;
            slopeDownAngle = Vector2.Angle(hit.normal, Vector2.up);

            if (slopeDownAngle != slopeDownAngleOld)
            {
                isOnSlope = true;
            }

            slopeDownAngleOld = slopeDownAngle;

            // Debug slope rays
            Debug.DrawRay(hit.point, slopeNormalPerpendicular, Color.red);
            Debug.DrawRay(hit.point, hit.normal, Color.green);
        }

        // Determine if slope is walkable
        if (slopeDownAngle > maxSlopeAngle || slopeSideAngle > maxSlopeAngle)
        {
            canWalkOnSlope = false;
        }
        else
        {
            canWalkOnSlope = true;
        }

        // Friction logic (updated variable name)
        if (isOnSlope && inputVector.x == 0.0f && canWalkOnSlope)
        {
            rigidbody2D.sharedMaterial = fullFriction;
        }
        else
        {
            rigidbody2D.sharedMaterial = noFriction;
        }
    }

    private bool IsGrounded()
    {
        Grounded = Physics2D.Raycast(groundCheck.position, Vector2.down, slopeCheckDistance, whatIsGround);
        return Grounded;
    }

    public float GetJumpSpeed2()
    {
        return jumpSpeed;
    }

    public float GetMoveSpeed2()
    {
        return moveSpeed;
    }

    public void SetJumpSpeed2(float jumpSpeed)
    {
        this.jumpSpeed = jumpSpeed;
    }

    public void SetMoveSpeed2(float moveSpeed)
    {
        this.moveSpeed = moveSpeed;
    }

    private void OnDestroy()
    {
        gameInput.OnPlayer2Jump -= GameInput_OnPlayer2Jump;
    }
}
