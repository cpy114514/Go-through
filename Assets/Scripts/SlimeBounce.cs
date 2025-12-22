using UnityEngine;

public class BouncePad : MonoBehaviour
{
    [Header("Bounce Settings")]
    public float bounceVelocity = 15f; // ⭐ 每次弹的“固定高度”
    public bool resetVerticalSpeed = true; // 是否清空下落速度

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
        if (rb == null) return;

        float x = rb.velocity.x;

        // 是否清空原来的下落速度
        if (resetVerticalSpeed)
            rb.velocity = new Vector2(x, bounceVelocity);
        else
            rb.velocity = new Vector2(x, rb.velocity.y + bounceVelocity);
    }
}
