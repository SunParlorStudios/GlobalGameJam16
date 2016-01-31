using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour
{
    public enum State
    {
        Walking,
        Attacking,
        Idle,
        KnockedBack
    }

    public int belongsToPlayer;
    public float health;
    protected float maxHealth;
    public int lane;

    public float speed;
    public float damage;

    public bool facingRight;
    public float collisionRange;

    public Animator animator;

    public GameObject shield;
    public Transform healthBarFill;
    public Transform healthBarBackground;
    public Transform explosionPoint;
    private float attackCooldown;
    private bool specialAttack;

    private float hobble;

    private GameObject otherUnit;

    private float baseY;

    private State state;

    private float shieldDuration;
    private float shieldTimer;
    private bool shieldDecaying;
    private float shieldDecayTimer;

    private Vector3 knockBackStart;
    private Vector3 knockBackEnd;
    private float knockBackTimer;

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

        shieldTimer -= Time.deltaTime;

        if (state == State.KnockedBack)
        {
            knockBackTimer += Time.deltaTime * 5.0f;

            transform.position = Vector3.Lerp(knockBackStart, knockBackEnd, knockBackTimer);

            if (knockBackTimer >= 1.0f)
            {
                state = State.Walking;
            }
            else
            {
                return;
            }
        }

        if (shieldTimer <= 0.0f && shield.activeInHierarchy == true && !shieldDecaying)
        {
            shieldDecaying = true;
            shieldDecayTimer = 0.0f;
        }

        if (shieldDecaying)
        {
            shieldDecayTimer += Time.deltaTime * 3.0f;

            shield.GetComponent<SpriteRenderer>().color = Color.Lerp(Color.white, new Color(1.0f, 1.0f, 1.0f, 0.0f), shieldDecayTimer);

            if (shieldDecayTimer >= 1.0f)
            {
                DisableShield();
            }
        }

        attackCooldown -= Time.deltaTime;

        if (attackCooldown <= 0.0f && state != State.Attacking)
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, (facingRight ? Vector3.right : Vector3.left), collisionRange);
            foreach (RaycastHit2D hit in hits)
            {
                
                if (hit.collider.tag == "Unit" && hit.collider.gameObject != gameObject && hit.collider.GetComponent<Unit>().belongsToPlayer != belongsToPlayer)
                {
                    state = State.Attacking;
                    animator.SetTrigger("attack");
                    otherUnit = hit.collider.gameObject;

                    specialAttack = false;
                    return;
                }
                
                if ((hit.collider.tag == "GodPortrait" && belongsToPlayer == 1) || (hit.collider.tag == "DevilPortrait" && belongsToPlayer == 0))
                {
                    state = State.Attacking;
                    animator.SetTrigger("attack");
                    specialAttack = true;
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

                    hobble = Mathf.Sin(Time.time * 12.0f) * 0.1f;

                    newPos.y += hobble;
                    transform.position = newPos;
                }
                else
                {
                    Vector3 newPos2 = new Vector3(transform.position.x, transform.position.y - hobble, transform.position.z);

                    hobble = Mathf.Sin(Time.time * 12.0f) * 0.1f;

                    newPos2.y += hobble;

                    transform.position = newPos2;
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
        if (state == State.Attacking)
        {
            Instantiate(Resources.Load("Particles/Explosion"), explosionPoint.position, Quaternion.identity);

            if (!specialAttack)
            {
                otherUnit.GetComponent<Unit>().Hit(damage);
            }
            else
            {
                otherUnit.GetComponent<PortraitHealth>().Hit(damage);
            }
        }
    }

    public void Hit(float damage)
    {
        if (!shield.activeInHierarchy)
        {
            health -= damage;

            if (health <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    public void OnEndAttack()
    {
        if (state == State.Attacking)
        {
            attackCooldown = 0.15f;
            state = State.Idle;
        }
    }

    public void EnableShield(float duration)
    {
        if (shield.activeInHierarchy != true)
        {
            shield.SetActive(true);
            shield.GetComponent<SpriteRenderer>().color = Color.white;
            shieldTimer = duration;
            shieldDecaying = false;
            shieldDecayTimer = 0.0f;
        }
    }

    public void DisableShield()
    {
        shield.SetActive(false);
        shieldTimer = -0.1f;
        shieldDecaying = false;
        shieldDecayTimer = 0.0f;
    }

    public void Knockback(Vector3 to)
    {
        knockBackStart = transform.position;
        knockBackEnd = to;
        knockBackTimer = 0.0f;
        state = State.KnockedBack;
    }
}
