using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 12f;
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;

    [Header("Animation")]
    public Animator animator;
    private Rigidbody2D rb;
    private float moveX;
    private float moveY;
    private bool isGrounded;
    private bool isFacingRight = true;
    bool isAttack = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }

    void Update()
    {   // Lấy data x và y
        moveX = Input.GetAxisRaw("Horizontal");
        moveY = Input.GetAxisRaw("Vertical");

        bool isMoving = Mathf.Abs(moveX) > 0.1f || Mathf.Abs(moveY) > 0.1f;
        animator.SetBool("isRun", isMoving);

        if (Input.GetKeyDown(KeyCode.J))
        {
            animator.SetBool("isAttack", true);
        }
        if (Input.GetKeyUp(KeyCode.J))
        {
            animator.SetBool("isAttack", false);
        }

        // Nhảy
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        // Lật mặt theo chiều ngang
        if (moveX > 0 && !isFacingRight)
            Flip();
        else if (moveX < 0 && isFacingRight)
            Flip();
    }

    void FixedUpdate()
    {
        // Di chuyển tự do 2 chiều
        rb.velocity = new Vector2(moveX * moveSpeed, moveY * moveSpeed);

        // Giới hạn tốc độ rơi khi không có input dọc
        if (moveY == 0 && !isGrounded)
        {
            //rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);
        }

        // Kiểm tra tiếp đất
        //isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (animator != null)
        {
            animator.SetBool("isGrounded", isGrounded);
            animator.SetFloat("VerticalVelocity", rb.velocity.y);
        }
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }
}
