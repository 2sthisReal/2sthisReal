using UnityEngine;

public class SpikeObstacle : MonoBehaviour
{
    [SerializeField] int damage = 5;
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            BaseCharacter player = other.GetComponent<BaseCharacter>();
            player.currentHealth -= damage;
        }
    }
}
