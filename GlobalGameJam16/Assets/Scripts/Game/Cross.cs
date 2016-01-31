using UnityEngine;
using System.Collections;

public class Cross : MonoBehaviour
{
    private enum State
    {
        HackyDelay,
        Dropping,
        Delay,
        Speeding
    }

    public float hackyDelayDuration;
    private float hackyDelayTimer;

    public float speed;
    public bool goingRight;
    public int lane;

    private float dropTimer;
    private Vector3 dropFrom;
    public Vector3 dropTo;

    public float delayDuration;
    private float delayTimer;

    private State state;

    private ParticleSystem particles;
    private Rigidbody2D rigidbody2d;

    public void Awake()
    {
        state = State.HackyDelay;

        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    public void Update()
    {
        switch (state)
        {
            case State.HackyDelay:
                hackyDelayTimer += Time.deltaTime;

                if (hackyDelayTimer >= hackyDelayDuration)
                {
                    state = State.Dropping;
                    dropFrom = transform.position;
                    dropTimer = 0.0f;

                    particles = GetComponentInChildren<ParticleSystem>();
                    particles.Stop();
                }
                break;
            case State.Dropping:
                dropTimer += Time.deltaTime * 4.0f;
                transform.position = Vector3.Lerp(dropFrom, dropTo, dropTimer);

                if (dropTimer >= 1.0f)
                {
                    state = State.Delay;
                    delayTimer = delayDuration;
                }
                break;
            case State.Delay:
                delayTimer -= Time.deltaTime;

                if (delayTimer <= 0.0f)
                {
                    state = State.Speeding;
                    particles.Play();
                }
                break;
            case State.Speeding:
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0.0f, 0.0f, goingRight ? 50.0f : -50.0f), Time.deltaTime * 10.0f);
                break;
        }
    }

    public void FixedUpdate()
    {
        switch (state)
        {
            case State.Speeding:
                rigidbody2d.AddForce((goingRight ? Vector3.right : Vector3.left) * speed);

                if (Vector3.Distance(Vector3.zero, transform.position) > 30.0f)
                {
                    Destroy(gameObject);
                }
                break;
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Unit")
        {
            if (collision.gameObject.GetComponent<Unit>().lane == lane)
            {
                Instantiate(Resources.Load("Particles/Explosion"), collision.transform.position, Quaternion.identity);
                collision.gameObject.GetComponent<Unit>().Hit(Mathf.Infinity);
            }
        }
    }
}
