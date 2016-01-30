using UnityEngine;
using System.Collections;

public class Sheep : MonoBehaviour
{
    public enum State
    {
        Walking,
        Attacking,
        Idle
    }

    public bool facingRight;
    public float collisionRange;

    public Animator animator;

    public Transform healthBarFill;
    public Transform healthBarBackground;
    public Transform explosionPoint;
    public float health;
    private float maxHealth;
    private float attackCooldown;

    public float speed;
    public float damage;

    private GameObject otherUnit;

    private float baseY;

    private State state;

    public void Awake()
    {
        baseY = transform.position.y;
        maxHealth = health;
        state = State.Walking;
        attackCooldown = 0.0f;
    }

    public void Update()
    {
        healthBarFill.localScale = new Vector3(health / maxHealth * 80.0f, 1.0f, 1.0f);

        attackCooldown -= Time.deltaTime;

        if (attackCooldown <= 0.0f && state != State.Attacking)
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, (facingRight ? Vector3.right : Vector3.left), collisionRange);
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider.tag == "Unit" && hit.collider.gameObject != gameObject)
                {
                    state = State.Attacking;
                    animator.SetTrigger("attack");
                    otherUnit = hit.collider.gameObject;
                    return;
                }
            }
        }

        switch (state)
        {
            case State.Walking:
                transform.Translate((facingRight ? Vector3.right : Vector3.left) * speed * Time.deltaTime);

                if (transform.position.x > -4.0f && transform.position.x < 4.0f)
                {
                    Vector3 newPos = new Vector3(transform.position.x, baseY, transform.position.z);
                    newPos.y = baseY + Mathf.Sin((newPos.x + 4.0f) / 8.0f * 3.0f) * 0.3f;

                    transform.position = newPos;
                }
                break;
            case State.Attacking:

                break;
            case State.Idle:
                if (attackCooldown <= 0.0f)
                {
                    state = State.Walking;
                }
                break;
        }
    }

    public void SetFacing(bool facingToRight)
    {
        if (facingRight != facingToRight)
        {
            transform.localScale = new Vector3(transform.localScale.x * -1, 1.0f, 1.0f);
            facingRight = facingToRight;
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        if (facingRight)
        {
            Gizmos.DrawLine(transform.position, transform.position + Vector3.right * collisionRange);
        }
        else
        {
            Gizmos.DrawLine(transform.position, transform.position + Vector3.left * collisionRange);
        }
    }

    public void OnHit()
    {
        Instantiate(Resources.Load("Particles/Explosion"), explosionPoint.position, Quaternion.identity);

        if (otherUnit.name.Contains("Sheep"))
        {
            otherUnit.GetComponent<Sheep>().Hit(damage);
        }
        
        if (otherUnit.name.Contains("Chicken"))
        {
            // checken
        }
    }

    public void Hit(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void OnEndAttack()
    {
        attackCooldown = 0.15f;
        state = State.Idle;
    }
}
