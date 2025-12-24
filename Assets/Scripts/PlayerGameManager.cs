using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PlayerGameManager : MonoBehaviour
{
    [Header("UI & 关卡设置")]
    public GameObject winPanel;           // 胜利UI
    public GameObject losePanel;          // 失败UI
    public Transform respawnPoint;        // 玩家初始位置
    public string currentLevelName;       // 当前关卡名字
    public string nextLevelName;          // 下一关名字
    public MonoBehaviour playerControl;   // 玩家控制脚本

    [Header("Death Animation")]
    public float deathAnimTime = 0.6f;    // 死亡动画/碎裂持续时间

    [Header("掉落虚空判定")]
    public float fallY = -10f;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator animator;
    private Vector3 initialPosition;
    private bool canWin = true;
    private bool canLose = true;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        if (respawnPoint != null)
            initialPosition = respawnPoint.position;
        else
            initialPosition = transform.position;
    }

    private void Update()
    {
        // 掉落虚空失败
        if (canLose && transform.position.y <= fallY)
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

            StopPlayer();

            if (winPanel != null)
                winPanel.SetActive(true);

            Debug.Log("You Win!");
        }
    }

    // ========================
    // 失败流程（核心）
    // ========================
    private void TriggerLose()
    {
        if (!canLose) return;
        canLose = false;

        StopPlayer();

        // 角色碎裂
        PlayerShatter shatter = GetComponent<PlayerShatter>();
        if (shatter != null)
            shatter.Shatter();

        // 隐藏原角色
        GetComponent<SpriteRenderer>().enabled = false;

        if (losePanel != null)
            losePanel.SetActive(true);

        // ⭐⭐⭐ 关键：清除 UI 选中状态 ⭐⭐⭐
        EventSystem.current.SetSelectedGameObject(null);

        Debug.Log("You Lose!");
    }



    private void ShowLosePanel()
    {
        if (losePanel != null)
            losePanel.SetActive(true);
    }

    private void StopPlayer()
    {
        // 停止物理
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.simulated = false;
        }

        // 禁用玩家控制
        if (playerControl != null)
            playerControl.enabled = false;
    }

    // ========================
    // Retry
    // ========================
    public void RetryLevel()
    {
        // 恢复物理
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

        // 恢复显示
        if (sr != null)
            sr.enabled = true;

        // 清除碎片
        PlayerShatter shatter = GetComponent<PlayerShatter>();
        if (shatter != null)
        {
            shatter.ClearShards();
        }


        // 重置动画状态
        if (animator != null)
        {
            animator.Rebind();
            animator.Update(0f);
        }

        // 隐藏 UI
        if (winPanel != null) winPanel.SetActive(false);
        if (losePanel != null) losePanel.SetActive(false);

        // 重置状态
        canWin = true;
        canLose = true;

        Debug.Log("Player reset to start point!");
    }

    // ========================
    // UI 按钮
    // ========================
    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void NextLevel()
    {
        if (!string.IsNullOrEmpty(nextLevelName))
            SceneManager.LoadScene(nextLevelName);
    }
}
