using UnityEngine;

public class SpikeObstacle : MonoBehaviour
{
    [SerializeField] int damage = 5;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Player player))
        {
            player.TakeDamage(damage);
        }
    }
}
