using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public class GameController : MonoBehaviour
{
    public GameObject ritualUIPrefab;
    public GameObject comboUIPrefab;
    public GameObject player1PortraitPrefab;
    public GameObject player2PortraitPrefab;

    public Ritual currentRitual;
    public GameObject lightningParticle;

    private bool[] joystickPressed;

    private struct RumbleValues
    {
        public float magnitude, time;
    }

    private RumbleValues[] rumble_;

    public void Awake()
    {
        rumble_ = new RumbleValues[2];

        rumble_[0].time = 0.0f;
        rumble_[1].time = 0.0f;

        joystickPressed = new bool[2]
        {
            false, false
        };

        KeyMapping.Initialise();

        currentRitual = new Ritual();
        currentRitual.OnReset += Rumble;

        Instantiate(ritualUIPrefab);
        Instantiate(player1PortraitPrefab);
        Instantiate(player2PortraitPrefab);
        Instantiate(comboUIPrefab);

        currentRitual.ConstructRitual(Random.Range(3, 9), Ritual.Difficulty.Easy);
    }

    private void Rumble(int joystick, float time, float magnitude)
    {
        rumble_[joystick].magnitude = magnitude;
        rumble_[joystick].time = time;
    }

    private void UpdateRumble()
    {
        for (int i = 0; i < 2; ++i)
        {
            rumble_[i].time = Mathf.Max(0.0f, rumble_[i].time);
            if (rumble_[i].time > 0.0f)
            {
                rumble_[i].time -= Time.deltaTime;
                float magnitude = rumble_[i].magnitude;
                GamePad.SetVibration((PlayerIndex)i, magnitude, magnitude);
            }
            else
            {
                GamePad.SetVibration((PlayerIndex)i, 0.0f, 0.0f);
            }
        }
    }

    public void Update()
    {
        System.Array keycodes = System.Enum.GetValues(typeof(KeyCodes));

        if (Input.GetKeyUp(KeyCode.Backspace) == true)
        {
            Application.LoadLevel(Application.loadedLevel);
            return;
        }

        UpdateRumble();

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

            for (int j = 0; j < keycodes.Length - 1; j++)
            {
                if (KeyMapping.IsAxisPressed((KeyCodes)j, Input.GetAxisRaw(KeyMapping.Get(i, (KeyCodes)j))))
                {
                    joystickPressed[i] = true;
                    currentRitual.EnterInput(i, (KeyCodes)j);

                    break;
                }
            }
        }

        if (currentRitual.IsComplete())
        {
            currentRitual.ExecuteReward(lightningParticle);
        }
    }
}
