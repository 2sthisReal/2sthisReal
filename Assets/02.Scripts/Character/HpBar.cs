using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    [SerializeField] private Image hpbar;
    [SerializeField] BaseCharacter character;

    void Awake()
    {
        character = GetComponent<BaseCharacter>();

        if (character != null)
            character.OnChangedHp += SetHpBar;
    }
    void SetHpBar(float maxHealth, float currentHealth)
    {
        hpbar.fillAmount = currentHealth / maxHealth;
    }
}
