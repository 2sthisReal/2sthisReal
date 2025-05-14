using System.Collections;
using UnityEngine;

public abstract class ItemBase : MonoBehaviour
{
    [SerializeField] protected float duration;
    [SerializeField] protected float value;
    [SerializeField] bool hasDuration = false;

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Player player))
        {
            UseItem(player);
        }
    }

    virtual protected void UseItem(Player player)
    {
        if (hasDuration)
            StartCoroutine(UseItemCoroutine(player));

        else
        {
            ApplyStat(player, value);
            Destroy(gameObject);
        }

        
    }

    abstract protected void ApplyStat(Player player, float value);

    virtual protected IEnumerator UseItemCoroutine(Player player)
    {
        GetComponentInChildren<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;

        ApplyStat(player, value);
        yield return new WaitForSeconds(duration);
        ApplyStat(player, -value);

        Destroy(gameObject);
    }
}
