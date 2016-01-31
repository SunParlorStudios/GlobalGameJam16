using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour
{
    public GameObject selector;
    public GameObject keyArt;
    public GameObject logo;

    private float animationTime;

    private Ritual.Difficulty difficulty;

    private bool resetted;

    public void Awake()
    {
        KeyMapping.Initialise();
    }

    public void Update()
    {
        keyArt.transform.position = Vector3.Lerp(keyArt.transform.position, new Vector3(-3.47f, -0.15f, 1.0f), Time.deltaTime * 3.0f);
        logo.transform.position = Vector3.Lerp(logo.transform.position, new Vector3(-2.25f, -3.24f, 1.0f), Time.deltaTime * 3.0f);

        if (Input.GetAxisRaw(KeyMapping.Get(0, KeyCodes.A)) >= 0.3f)
        {
            PlayerPrefs.SetInt("difficulty", (int)difficulty);
            Application.LoadLevel(1);
        }

        if (Input.GetAxisRaw("Joystick1ThumbY") >= 0.3f && resetted)
        {
            switch (difficulty)
            {
                case Ritual.Difficulty.Easy:
                    selector.transform.Translate(Vector3.down * 0.67f);
                    difficulty = Ritual.Difficulty.Medium;
                    break;
                case Ritual.Difficulty.Medium:
                    selector.transform.Translate(Vector3.down * 0.67f);
                    difficulty = Ritual.Difficulty.Hard;
                    break;
                case Ritual.Difficulty.Hard:
                    break;
            }
            resetted = false;
        }

        if (Input.GetAxisRaw("Joystick1ThumbY") <= -0.3f && resetted)
        {
            switch (difficulty)
            {
                case Ritual.Difficulty.Easy:
                    // nope
                    break;
                case Ritual.Difficulty.Medium:
                    selector.transform.Translate(Vector3.up * 0.67f);
                    difficulty = Ritual.Difficulty.Easy;
                    break;
                case Ritual.Difficulty.Hard:
                    selector.transform.Translate(Vector3.up * 0.67f);
                    difficulty = Ritual.Difficulty.Medium;
                    break;
            }
            
            resetted = false;
        }

        if (!resetted && Mathf.Abs(Input.GetAxisRaw("Joystick1ThumbY")) <= 0.3f)
        {
            resetted = true;
        }
    }
}
