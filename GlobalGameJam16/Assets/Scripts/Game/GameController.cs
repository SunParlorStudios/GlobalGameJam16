using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
    public Ritual currentRitual;

    private bool[] joystickPressed;

    public void Awake()
    {
        joystickPressed = new bool[2];
        joystickPressed[0] = false;
        joystickPressed[1] = false;

        KeyMapping.Initialise();

        currentRitual = new Ritual(Random.Range(3, 4), Ritual.Difficulty.Easy);
    }

    public void Update()
    {
        for (int i = 0; i < 2; i++)
        {
            if (joystickPressed[i] == true)
            {
                if (KeyMapping.NoKeysPressed(i))
                {
                    joystickPressed[i] = false;
                }
                else
                {
                    continue;
                }
            }

            System.Array something = System.Enum.GetValues(typeof(KeyCodes));
            
            for (int j = 0; j < something.Length; j++)
            {
                if ((KeyCodes)j != KeyCodes.Last && KeyMapping.IsAxisPressed((KeyCodes)j, Input.GetAxisRaw(KeyMapping.Get(i, (KeyCodes)j))))
                {
                    joystickPressed[i] = true;
                    currentRitual.EnterInput(i, (KeyCodes)j);

                    break;
                }
            }
        }

        if (currentRitual.IsComplete())
        {
            currentRitual.ExecuteReward();
        }
    }
}
