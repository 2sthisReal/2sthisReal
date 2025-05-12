using System.Collections;
using UnityEngine;

public abstract class ItemBase : MonoBehaviour
{
    [SerializeField] protected float duration;
    [SerializeField] protected float value;
    [SerializeField] bool hasDuration = false;

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            UseItem(other.GetComponent<BaseCharacter>());
        }
    }

    virtual protected void UseItem(BaseCharacter player)
    {
        if (hasDuration)
            StartCoroutine(UseItemCoroutine(player));

        else
            ApplyStat(player, value);

        Destroy(gameObject);
    }

    abstract protected void ApplyStat(BaseCharacter player, float value);

    virtual protected IEnumerator UseItemCoroutine(BaseCharacter player)
    {
        ApplyStat(player, value);
        yield return new WaitForSeconds(duration);
        ApplyStat(player, -value);
    
    }
}
