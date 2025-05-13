using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetBomb : MonoBehaviour
{
    private float explodeTimer;

    GameObject bombSprite;
    GameObject explosionEffect;

    // Start is called before the first frame update
    private void Awake()
    {
        bombSprite = transform.Find("Sprite").GetComponent<GameObject>();
        explosionEffect = transform.Find("Explode").GetComponent<GameObject>();
    }
    void Start()
    {
        explodeTimer = 2f;
    }

    // Update is called once per frame
    void Update()
    {
        explodeTimer -= Time.deltaTime;
        if (explodeTimer < 0)
            Explosion();
    }
    public void Explosion()
    {
        bombSprite.SetActive(false);
        explosionEffect.SetActive(true);
    }
}
