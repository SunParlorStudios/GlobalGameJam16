using UnityEngine;
using System.Collections.Generic;

public class Shockwave : MonoBehaviour
{
    private List<GameObject> pushedObjects;

    public int belongsToPlayer;
    public bool goingRight;
    public float speed;
    public float knockbackForce;

    public AudioSource shockwaveSound;

    public void Awake()
    {
        shockwaveSound.Play();
        pushedObjects = new List<GameObject>();

        RippleEffect rip = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<RippleEffect>();

        if (belongsToPlayer == 0)
        {
            rip.Emit(new Vector2(0.0f, 0.5f));
            transform.position = new Vector3(-9.5f, 0.0f, 0.0f);
        }
        else
        {
            rip.Emit(new Vector2(1.0f, 0.5f));
            transform.position = new Vector3(9.5f, 0.0f, 0.0f);
        }
    }

    public void Update()
    {
        transform.Translate((goingRight ? Vector3.right : Vector3.left) * speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, Vector3.zero) > 30)
        {
            Destroy(gameObject);
        }
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        for (int i = 0; i < pushedObjects.Count; i++)
        {
            if (pushedObjects[i] == collider.gameObject)
            {
                return;
            }
        }

        if (collider.GetComponent<Unit>())
        {
            if (collider.GetComponent<Unit>().belongsToPlayer != belongsToPlayer)
            {
                pushedObjects.Add(collider.gameObject);
                collider.GetComponent<Unit>().Knockback(collider.transform.position + (goingRight ? Vector3.right : Vector3.left) * knockbackForce);
            }
        }
    }
}
