using UnityEngine;
using System.Collections.Generic;

public class Lane : MonoBehaviour
{
    private struct LaneTile
    {
        public bool inUse;
        public GameObject unit;
    }

    public int numTiles;
    public float tileSize;

    public void Awake()
    {
        
    }

    public void OnDrawGizmos()
    {
        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
        float totalWidth = boxCollider.bounds.size.x;
        float tileWidth = totalWidth / numTiles;

        for (int i = 0; i < numTiles; i++)
        {
            Gizmos.color = new Color(1.0f, 1.0f, 1.0f, 0.2f);
            Gizmos.DrawWireCube(transform.position + Vector3.left * (totalWidth / 2) + Vector3.right * (tileWidth / 2) + Vector3.right * tileWidth * i, new Vector3(boxCollider.bounds.size.y, boxCollider.bounds.size.y, boxCollider.bounds.size.y));
        }
    }
}
