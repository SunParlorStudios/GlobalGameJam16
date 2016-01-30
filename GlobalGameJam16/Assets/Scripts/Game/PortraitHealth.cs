using UnityEngine;
using System.Collections;

public class PortraitHealth : MonoBehaviour
{
    public float health = 100.0f;
    private float maxHealth = 100.0f;

    public delegate void OnPortraitDeathDelegate();
    public event OnPortraitDeathDelegate OnPortraitDeath;

    public delegate void OnPortraitHitDelegate();
    public event OnPortraitHitDelegate OnPortraitHit;

    public GameObject healthBarFill;
    public GameObject healthBarBackground;

    public void Awake()
    {
        maxHealth = health;
    }

    public void Update()
    {
        healthBarFill.transform.localScale = new Vector3(health / maxHealth * 300.0f, 1.0f, 1.0f);
    }

    public void Hit(float damage)
    {
        health -= damage;

        if (OnPortraitHit != null)
        {
            OnPortraitHit();
        }

        if (health <= 0)
        {
            if (OnPortraitDeath != null)
            {
                OnPortraitDeath();
            }

            Destroy(gameObject);
        }
    }
}
