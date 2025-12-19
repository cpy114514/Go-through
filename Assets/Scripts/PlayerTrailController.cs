using UnityEngine;

public class PlayerTrailController : MonoBehaviour
{
    public TrailRenderer trail;       // 拖尾组件
    public GameObject player;         // 玩家对象
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode accelKey = KeyCode.J;

    private bool isJumpTrail = false;

    void Start()
    {
        if (trail == null)
            trail = GetComponent<TrailRenderer>();
        trail.emitting = false;
    }

    void Update()
    {
        // 跳跃：按一次空格触发短拖尾
        if (Input.GetKeyDown(jumpKey))
        {
            if (!isJumpTrail)
                StartCoroutine(JumpTrail());
        }

        // 加速：按住J键持续发出拖尾
        if (Input.GetKey(accelKey))
        {
            trail.emitting = true;
        }
        else if (!isJumpTrail)
        {
            trail.emitting = false;
        }
    }

    System.Collections.IEnumerator JumpTrail()
    {
        isJumpTrail = true;
        trail.emitting = true;
        yield return new WaitForSeconds(0.25f);
        trail.emitting = false;
        isJumpTrail = false;
    }
}
