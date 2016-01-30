using UnityEngine;
using System.Collections;

public class ParticleOneShot : MonoBehaviour
{
    private ParticleSystem particleSystem_;

	void Awake()
    {
        particleSystem_ = GetComponent<ParticleSystem>();
	}
	
	void Update()
    {
        if (particleSystem_.IsAlive() == false)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
                return;
            }

            Destroy(gameObject);
        }
	}
}
