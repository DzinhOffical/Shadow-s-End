using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Tốc độ di chuyển thông thường
    public float sprintSpeed = 10f; // Tốc độ chạy nhanh khi dùng kỹ năng
    private float currentSpeed; // Tốc độ hiện tại của người chơi
    public float jumpForce = 10f; // Lực nhảy
    private Rigidbody2D rb;
    private bool isGrounded;
    public Transform groundCheck;
    public LayerMask groundLayer;
    private bool facingRight = true; // Biến để theo dõi hướng mà nhân vật đang đối diện
    private Animator anim; // Tham chiếu tới Animator
    private bool isSprinting = false; // Biến để theo dõi trạng thái chạy nhanh
    public float sprintDuration = 5f; // Thời gian chạy nhanh

    // Mana
    //private PlayerMana playerMana;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        //playerMana = GetComponent<PlayerMana>();

        // Kiểm tra xem Animator có tồn tại không
        if (anim == null)
        {
            Debug.LogError("Animator not found on the Player object.");
        }
        //if (playerMana == null)
        {
            Debug.LogError("PlayerMana component not found on the Player object.");
        }

        currentSpeed = moveSpeed; // Đặt tốc độ ban đầu thành tốc độ di chuyển thông thường
    }

    void Update()
    {
        // Lấy input từ người chơi
        float moveHorizontal = Input.GetAxis("Horizontal");

        // Di chuyển đối tượng theo input và tốc độ hiện tại
        Vector2 movement = new Vector2(moveHorizontal * currentSpeed, rb.velocity.y);
        rb.velocity = movement;

        // Kiểm tra và quay đầu nhân vật
        if (moveHorizontal > 0 && !facingRight)
        {
            Flip();
        }
        else if (moveHorizontal < 0 && facingRight)
        {
            Flip();
        }

        // Kiểm tra xem player có đang đứng trên mặt đất không
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

        // Nếu player đang đứng trên mặt đất và người chơi nhấn phím Space
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            // Áp dụng lực nhảy
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            // Áp dụng animation nhảy
            anim.SetTrigger("Jump");
        }

        // Nếu player không đứng trên mặt đất và không nhấn phím nhảy, chuyển sang animation Fall
        if (!isGrounded && rb.velocity.y < 0)
        {
            anim.SetBool("Roll", true);
        }
        else
        {
            anim.SetBool("Roll", false);
        }

        // Cập nhật trạng thái của Animator
        anim.SetFloat("Speed", Mathf.Abs(moveHorizontal));
        anim.SetBool("Grounded", isGrounded);

        // Kích hoạt chạy nhanh khi nhấn phím E
        if (Input.GetKeyDown(KeyCode.E) && !isSprinting)
        {
            //if (playerMana.UseMana(5))
            {
                StartCoroutine(Sprint());
            }
        }

        anim.SetFloat("Move", Mathf.Abs(moveHorizontal));
    }

    // Phương thức để quay đầu nhân vật
    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    // Coroutine để xử lý chạy nhanh
    IEnumerator Sprint()
    {
        isSprinting = true;
        currentSpeed = sprintSpeed; // Tăng tốc độ di chuyển lên sprintSpeed
        yield return new WaitForSeconds(sprintDuration); // Chờ trong thời gian chạy nhanh
        currentSpeed = moveSpeed; // Trở về tốc độ bình thường
        isSprinting = false;
    }

    // Vẽ vòng tròn kiểm tra mặt đất trong Scene view để kiểm tra
    void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, 0.2f);
        }
    }
}
