using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip jumpSound;
    public AudioClip dashSound;
    public AudioClip boostSound;

    [Header("Move Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 12f;
    public float wallBounceDistance = 5f;
    public float dashForce = 15f;
    public float dashDuration = 0.15f;
    public float dashCooldown = 2f;
    public float boostMultiplier = 1.5f;
    public float wallSlideSpeed = 2f;
    public float rotationSpeed = 360f;

    [Header("Ice Settings")]
    [Range(0.001f, 0.2f)]
    public float iceFriction = 0.02f;   // 越小越滑

    [Header("Effects")]
    public ParticleSystem iceEffect;    // 冰滑粒子

    [Header("Jump Settings")]
    public int maxJumps = 2;
    private int jumpsUsed = 0;

    [Header("Wall Jump Settings")]
    public float wallJumpDuration = 0.2f;
    private bool isWallJumping = false;
    private float wallJumpTimer = 0f;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private bool isFacingRight = true;
    private bool isOnWall = false;
    private bool isWallSliding = false;
    private Vector2 wallNormal;
    private bool isRotating = false;

    private bool isDashing = false;
    private float dashTimerCurrent = 0f;
    private float dashCooldownTimer = 0f;
    private float originalGravity;

    private bool isOnIce = false;

    // 过滤 Player 层，避免 Raycast 打到自己
    private int groundMask;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        sr = GetComponent<SpriteRenderer>();
        originalGravity = rb.gravityScale;

        groundMask = ~(1 << LayerMask.NameToLayer("Player"));

        if (iceEffect != null)
            iceEffect.Stop();
    }

    void Update()
    {
        // 检测是否踩在冰面上
        isOnIce = CheckIce();

        HandleDash();
        HandleMovement();
        PreventEnteringWall();
        HandleJump();
        HandleWallSlide();
        HandleRotation();

        if (dashCooldownTimer > 0f)
            dashCooldownTimer -= Time.deltaTime;

        if (isWallJumping)
        {
            wallJumpTimer -= Time.deltaTime;
            if (wallJumpTimer <= 0f)
                isWallJumping = false;
        }

        UpdateIceParticle();   // 控制冰粒子
        PreventStuckInWall();  // 防止卡墙
    }

    // ★★★ ICE 检测（脚底往下射线）★★★
    bool CheckIce()
    {
        Vector2 origin = (Vector2)transform.position + Vector2.down * 0.4f;
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, 1f, groundMask);
        Debug.DrawRay(origin, Vector2.down * 1f, Color.blue);

        return hit.collider != null && hit.collider.CompareTag("Ice");
    }

    // 冰粒子控制（只在冰上 & 移动时喷，自动跟随朝向）
    void UpdateIceParticle()
    {
        if (iceEffect == null) return;

        bool isMoving = Mathf.Abs(rb.velocity.x) > 0.1f;

        if (isOnIce && isMoving)
        {
            if (!iceEffect.isPlaying)
                iceEffect.Play();
        }
        else
        {
            if (iceEffect.isPlaying)
                iceEffect.Stop();
        }

        // 使用 Velocity over Lifetime 改方向，而不是 rotation
        var vel = iceEffect.velocityOverLifetime;

        if (isFacingRight)
        {
            vel.x = new ParticleSystem.MinMaxCurve(-4f, -8f);
            vel.y = new ParticleSystem.MinMaxCurve(4f, 12f);
        }
        else
        {
            vel.x = new ParticleSystem.MinMaxCurve(4f, 8f);  // 朝左喷
            vel.y = new ParticleSystem.MinMaxCurve(4f, 12f); // 一样往上
        }
    }


    void HandleMovement()
    {
        if (isDashing) return;
        if (isWallJumping) return;

        float move = 0f;
        if (Input.GetKey(KeyCode.A)) move = -moveSpeed;
        if (Input.GetKey(KeyCode.D)) move = moveSpeed;

        // 加速键 J
        if (Input.GetKey(KeyCode.J)) move *= boostMultiplier;

        if (isOnIce)
        {
            // 冰面：缓慢改变速度 → 滑行
            float smoothX = Mathf.Lerp(rb.velocity.x, move, iceFriction);
            rb.velocity = new Vector2(smoothX, rb.velocity.y);
        }
        else
        {
            // 正常地面：立即响应
            rb.velocity = new Vector2(move, rb.velocity.y);
        }

        // 朝向翻转
        if (move > 0 && !isFacingRight) Flip();
        if (move < 0 && isFacingRight) Flip();
    }

    void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isWallSliding)
            {
                float dir = wallNormal.x > 0 ? 1 : -1;
                rb.velocity = new Vector2(dir * wallBounceDistance, jumpForce);

                isWallSliding = false;
                isWallJumping = true;
                wallJumpTimer = wallJumpDuration;
                jumpsUsed = 1;
                isRotating = true;

                if ((dir > 0 && !isFacingRight) || (dir < 0 && isFacingRight))
                    Flip();
            }
            else if (jumpsUsed < maxJumps)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                jumpsUsed++;
                isRotating = true;
            }
        }
    }

    void HandleDash()
    {
        if (Input.GetKeyDown(KeyCode.K) && dashCooldownTimer <= 0f)
        {
            float dir = isFacingRight ? 1 : -1;

            rb.velocity = new Vector2(dir * dashForce, 0f);
            rb.gravityScale = 0f;

            isDashing = true;
            dashTimerCurrent = dashDuration;
            dashCooldownTimer = dashCooldown;

            isWallJumping = false;
            wallJumpTimer = 0f;
        }

        if (isDashing)
        {
            dashTimerCurrent -= Time.deltaTime;

            if (dashTimerCurrent <= 0f)
            {
                isDashing = false;
                rb.gravityScale = originalGravity;
            }
        }
    }

    void HandleWallSlide()
    {
        if (isOnWall && !IsGrounded() && rb.velocity.y < 0)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, -wallSlideSpeed);
        }
        else
        {
            isWallSliding = false;
        }
    }

    void HandleRotation()
    {
        if (isRotating)
        {
            float dir = isFacingRight ? -1 : 1;
            transform.Rotate(0, 0, rotationSpeed * Time.deltaTime * dir);
        }
    }

    // Ground 检测（脚底射线）
    bool IsGrounded()
    {
        Vector2 origin = (Vector2)transform.position + Vector2.down * 0.4f;
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, 1f, groundMask);
        Debug.DrawRay(origin, Vector2.down * 1f, Color.red);

        if (hit.collider == null) return false;

        return hit.collider.CompareTag("Ground")
            || hit.collider.CompareTag("Block")
            || hit.collider.CompareTag("Ice");
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Wall") || collision.collider.CompareTag("Block"))
        {
            Vector2 normal = collision.contacts[0].normal;

            if (Mathf.Abs(normal.x) > 0.5f)
            {
                isOnWall = true;
                wallNormal = normal;
            }
        }

        if (collision.collider.CompareTag("Ground")
            || collision.collider.CompareTag("Block")
            || collision.collider.CompareTag("Ice"))
        {
            jumpsUsed = 0;
            isRotating = false;
            transform.rotation = Quaternion.identity;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Wall") || collision.collider.CompareTag("Block"))
            isOnWall = false;
    }

    void PreventEnteringWall()
    {
        float dist = 0.1f;
        Vector2 origin = (Vector2)transform.position + Vector2.down * 0.3f;

        RaycastHit2D r = Physics2D.Raycast(origin, Vector2.right, dist, groundMask);
        if (r.collider != null && !r.collider.isTrigger && rb.velocity.x > 0)
            rb.velocity = new Vector2(0, rb.velocity.y);

        RaycastHit2D l = Physics2D.Raycast(origin, Vector2.left, dist, groundMask);
        if (l.collider != null && !l.collider.isTrigger && rb.velocity.x < 0)
            rb.velocity = new Vector2(0, rb.velocity.y);
    }

    void PreventStuckInWall()
    {
        float dist = 0.08f;
        float push = 0.08f;
        Vector2 origin = (Vector2)transform.position + Vector2.down * 0.3f;

        RaycastHit2D r = Physics2D.Raycast(origin, Vector2.right, dist, groundMask);
        if (r.collider != null && !r.collider.isTrigger)
            transform.position += new Vector3(-push, 0, 0);

        RaycastHit2D l = Physics2D.Raycast(origin, Vector2.left, dist, groundMask);
        if (l.collider != null && !l.collider.isTrigger)
            transform.position += new Vector3(push, 0, 0);
    }


    void Flip()
    {
        isFacingRight = !isFacingRight;
        sr.flipX = !sr.flipX;
    }
}
