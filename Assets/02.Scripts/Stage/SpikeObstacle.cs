using UnityEngine;

public class SpikeObstacle : MonoBehaviour
{
    [SerializeField] int damage = 5;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            player.Damaged(damage);
        }
    }
}
