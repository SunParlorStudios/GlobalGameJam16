using UnityEngine;
using System.Collections;

public class Particle : MonoBehaviour
{
    private float timer;
    public float destructTimer;

    public void Awake()
    {
        timer = 0.0f;
    }

    public void Update()
    {
        timer += Time.deltaTime;
        
        if (timer >= destructTimer)
        {
            Destroy(gameObject);
        }
    }
}
