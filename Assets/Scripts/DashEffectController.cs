using UnityEngine;

public class DashEffectController : MonoBehaviour
{
    public ParticleSystem dashEffect;  // 拖入粒子系统
    public Transform player;           // 拖入玩家对象
    public KeyCode dashKey = KeyCode.K;
    public float dashCooldown = 1f;    // 冲刺冷却时间

    private bool canDash = true;
    private bool isFacingRight = true;

    void Update()
    {
        if (player != null)
        {
            // 根据玩家方向更新朝向
            float scaleX = player.localScale.x;
            if (scaleX > 0 && !isFacingRight)
            {
                isFacingRight = true;
                dashEffect.transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
            else if (scaleX < 0 && isFacingRight)
            {
                isFacingRight = false;
                dashEffect.transform.localRotation = Quaternion.Euler(0, 180, 0);
            }
        }

        // 按下冲刺键播放特效（不改你原本的冲刺逻辑）
        if (Input.GetKeyDown(dashKey) && canDash)
        {
            dashEffect.Play();
            StartCoroutine(DashCooldown());
        }
    }

    private System.Collections.IEnumerator DashCooldown()
    {
        canDash = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
}
