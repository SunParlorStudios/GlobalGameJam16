using UnityEngine;
using System.Collections;

public class Sheep : Unit
{
    public bool facingRight;
    public float collisionRange;

    public float speed;

    private float baseY;

    public void Awake()
    {
        baseY = transform.position.y;
    }

    public void Update()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, (facingRight ? Vector3.right : Vector3.left), collisionRange);
        foreach(RaycastHit2D hit in hits)
        {
            if (hit.collider.tag == "Unit" && hit.collider.gameObject != gameObject)
            {
                return;
            }
        }

        transform.Translate((facingRight ? Vector3.right : Vector3.left) * speed * Time.deltaTime);

        if (transform.position.x > -4.0f && transform.position.x < 4.0f)
        {
            Vector3 newPos = new Vector3(transform.position.x, baseY, transform.position.z);
            newPos.y = baseY + Mathf.Sin((newPos.x + 4.0f) / 8.0f * 3.0f) * 0.3f;

            transform.position = newPos;
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
}
