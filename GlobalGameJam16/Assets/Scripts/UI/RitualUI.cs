using UnityEngine;
using System.Collections;

public class RitualUI : MonoBehaviour
{
    public Sprite aButtonSprite;
    public Sprite bButtonSprite;
    public Sprite xButtonSprite;
    public Sprite yButtonSprite;
    public Sprite leftButtonSprite;
    public Sprite rightButtonSprite;
    public Sprite upButtonSprite;
    public Sprite downButtonSprite;

    private GameController gameController_;

	void Awake()
    {
        GameObject gameController = GameObject.FindGameObjectWithTag("GameController");

        if (gameController == null)
        {
            Debug.LogError("Could not find a GameController object tagged with 'GameController'");
            return;
        }

        gameController_ = gameController.GetComponent<GameController>();
	}
	
	void Update()
    {
        Debug.Log(Input.GetAxisRaw("Joystick1Trigger") + " | " + Input.GetAxisRaw("Joystick2Trigger"));
	}
}
