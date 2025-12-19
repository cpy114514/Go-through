using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerGameManager : MonoBehaviour
{
    [Header("UI & 关卡设置")]
    public GameObject winPanel;       // 胜利UI
    public GameObject losePanel;      // 失败UI
    public Transform respawnPoint;    // 玩家初始位置
    public string currentLevelName;   // 当前关卡名字
    public string nextLevelName;      // 下一关名字
    public MonoBehaviour playerControl; // 玩家控制脚本

    private Rigidbody2D rb;
    private Vector3 initialPosition;
    private bool canWin = true;
    private bool canLose = true;

    [Header("掉落虚空判定")]
    public float fallY = -10f; // 玩家低于此Y值即失败

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (respawnPoint != null)
            initialPosition = respawnPoint.position;
        else
            initialPosition = transform.position;
    }

    private void Update()
    {
        // 掉落虚空判定
        if (transform.position.y <= fallY && canLose)
        {
            TriggerLose();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 碰到障碍物失败
        if (canLose && collision.gameObject.CompareTag("Obstacle"))
        {
            TriggerLose();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 碰到终点胜利
        if (canWin && collision.CompareTag("Finish"))
        {
            canWin = false;

            if (winPanel != null)
                winPanel.SetActive(true);

            StopPlayer();

            Debug.Log("You Win!");
        }
    }

    private void TriggerLose()
    {
        canLose = false;

        if (losePanel != null)
            losePanel.SetActive(true);

        StopPlayer();

        Debug.Log("You Lose!");
    }

    private void StopPlayer()
    {
        // 停止 Rigidbody2D
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.simulated = false;
        }

        // 禁用玩家控制
        if (playerControl != null)
            playerControl.enabled = false;
    }

    // Retry 按钮
    public void RetryLevel()
    {
        // 恢复 Rigidbody2D
        if (rb != null)
        {
            rb.simulated = true;
            rb.velocity = Vector2.zero;
        }

        // 恢复控制
        if (playerControl != null)
            playerControl.enabled = true;

        // 重置位置
        transform.position = initialPosition;

        // 隐藏 UI
        if (winPanel != null)
            winPanel.SetActive(false);
        if (losePanel != null)
            losePanel.SetActive(false);

        // 重置触发状态
        canWin = true;
        canLose = true;

        Debug.Log("Player reset to start point!");
    }

    // Back 按钮 → 返回主菜单
    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    // Next 按钮 → 只有胜利才用
    public void NextLevel()
    {
        if (!string.IsNullOrEmpty(nextLevelName))
            SceneManager.LoadScene(nextLevelName);
    }
}
