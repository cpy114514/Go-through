using UnityEngine;

public class PlayerShatter : MonoBehaviour
{
    public GameObject shardPrefab;
    public int shardCount = 8;
    private GameObject shardParent;

    public void Shatter()
    {
        shardParent = new GameObject("PlayerShards");

        for (int i = 0; i < shardCount; i++)
        {
            GameObject shard = Instantiate(
                shardPrefab,
                transform.position,
                Quaternion.identity,
                shardParent.transform
            );

            Rigidbody2D rb = shard.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.AddForce(Random.insideUnitCircle * 6f, ForceMode2D.Impulse);
            }
        }
    }

    public void ClearShards()
    {
        if (shardParent != null)
            Destroy(shardParent);
    }
}