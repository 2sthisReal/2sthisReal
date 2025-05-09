using UnityEngine;

public abstract class ItemBase : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            UseItem(other.GetComponent<BaseCharacter>());
        }
    }

    abstract protected void UseItem(BaseCharacter player);
}
