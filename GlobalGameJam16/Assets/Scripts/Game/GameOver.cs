using UnityEngine;
using System.Collections;

public class GameOver : MonoBehaviour
{
    new private SpriteRenderer renderer;
    public SpriteRenderer vignette;

    private float baseY;

    private bool animating;
    private float animationTimer;

    public void Awake()
    {
        baseY = transform.position.y;
        renderer = GetComponent<SpriteRenderer>();

        GameController gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

        GameObject[] gos = GameObject.FindGameObjectsWithTag("Combo");
        foreach (GameObject go in gos)
        {
            Destroy(go);
        }

        gos = GameObject.FindGameObjectsWithTag("RitualUI");
        foreach (GameObject go in gos)
        {
            Destroy(go);
        }

        switch (gc.WhoHasWon())
        {
            case 0:
                renderer.sprite = Resources.Load<Sprite>("GameOver/God");
                transform.FindChild("smoke_particle_devil").gameObject.SetActive(false);
                break;
            case 1:
                renderer.sprite = Resources.Load<Sprite>("GameOver/Devil");
                transform.FindChild("smoke_particle_god").gameObject.SetActive(false);
                break;
        }

        animating = true;
        animationTimer = 0.0f;

        vignette.transform.parent = null;
    }

    public void Update()
    {
        if (animating)
        {
            animationTimer += Time.deltaTime;
            Color newColor = new Color(1.0f, 1.0f, 1.0f, Mathf.Lerp(0.0f, 1.0f, animationTimer));
            renderer.color = newColor;
            vignette.color = newColor;

            if (animationTimer >= 1.0f)
            {
                animating = false;
            }
        }

        transform.position = new Vector3(transform.position.x, baseY + Mathf.Sin(Time.time) * 0.4f, transform.position.z);

        if (Input.GetAxisRaw("Joystick1A") >= 0.3f || Input.GetAxisRaw("Joystick1B") >= 0.3f)
        {
            Application.LoadLevel(0);
        }
    }
}
