using UnityEngine;

public class StageDoor : MonoBehaviour
{
   [SerializeField] private ParticleSystem particle;
    [SerializeField] private Animator anim;
    [SerializeField] private Collider2D trigger;
    [SerializeField] private Collider2D col;

    void Awake()
    {
        anim.GetComponent<Animator>();
        particle = GetComponentInChildren<ParticleSystem>();
        trigger = GetComponent<Collider2D>();
    }
    public void SetDoor(bool isClear)
    {
        anim.SetBool("IsClear", isClear);
        trigger.enabled = isClear;
        col.enabled = !isClear;
        
        if (isClear)
            particle.Play();
        else
            particle.Stop();
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.MoveToNextStage();
        }
    }
}
