using UnityEngine;
using System.Collections.Generic;

public class Ritual
{
    public enum Difficulty
    {
        Easy,
        Medium,
        Hard
    }

    public class RitualKey
    {
        public RitualKey(KeyCodes code = KeyCodes.None)
        {
            keyCode = code;
            earnedByJoy = new bool[2];
            earnedByJoy[0] = false;
            earnedByJoy[1] = false;
        }

        public KeyCodes keyCode;
        public bool[] earnedByJoy;
    }

    public delegate void Completed(int joystick);
    public event Completed OnComplete;

    public delegate void OnChange(int count);
    public event OnChange Changed;

    public delegate void PlayerReset(int joystick, float time, float magnitude);
    public event PlayerReset OnReset;

    public delegate void KeyPressed(int joystick, int index);
    public event KeyPressed OnPress;

    private const float rumbleStrength_ = 2.0f;
    private const float rumbleTime_ = 0.5f;

    public List<RitualKey> ritual;
    private Reward reward;

    public void ConstructRitual(int length, Difficulty difficulty)
    {
        reward = Reward.GetReward(length);

        ritual = new List<RitualKey>();

        List<KeyCodes> keyCodesPool = new List<KeyCodes>()
        {
            KeyCodes.A, KeyCodes.B, KeyCodes.X, KeyCodes.Y
        };

        if (difficulty == Difficulty.Medium)
        {
            keyCodesPool.AddRange(new KeyCodes[4]{ KeyCodes.Left, KeyCodes.Right, KeyCodes.Up, KeyCodes.Down });
        }

        if (difficulty == Difficulty.Hard)
        {
            keyCodesPool.AddRange(new KeyCodes[4] { KeyCodes.LT, KeyCodes.RT, KeyCodes.LB, KeyCodes.RB });
        }

        for (int i = 0; i < length; ++i)
        {
            RitualKey ritualKey = new RitualKey(keyCodesPool[Random.Range(0, keyCodesPool.Count)]);

            ritual.Add(ritualKey);
        }

        PostChangedEvent();
    }

    public void EnterInput(int joystick, KeyCodes keyCode)
    {
        int ritualIndex = GetCurrentIndex(joystick) + 1;

        if (ritual[ritualIndex].keyCode == keyCode)
        {
            ritual[ritualIndex].earnedByJoy[joystick] = true;

            if (OnPress != null)
            {
                OnPress(joystick, ritualIndex);
            }
        }
        else
        {
            Reset(joystick);
        }
    }

    private void Reset(int joystick)
    {
        for (int i = 0; i < ritual.Count; i++)
        {
            ritual[i].earnedByJoy[joystick] = false;
        }

        if (OnReset != null)
        {
            OnReset(joystick, rumbleTime_, rumbleStrength_);
        }
    }

    public int GetCurrentIndex(int joystick)
    {
        for (int i = ritual.Count - 1; i >= 0; i--)
        {
            if (ritual[i].earnedByJoy[joystick] == true)
            {
                return i;
            }
        }

        return -1;
    }

    public bool IsComplete()
    {
        for (int i = 0; i < 2; i++)
        {
            if (ritual[ritual.Count - 1].earnedByJoy[i] == true)
            {
                return true;
            }
        }

        return false;
    }

    public void PostChangedEvent()
    {
        if (Changed != null)
        {
            Changed(ritual.Count);
        }
    }

    public void ExecuteReward(GameObject particle)
    {
        if (IsComplete() == true)
        {
            for (int i = 0; i < 2; i++)
            {
                if (ritual[ritual.Count - 1].earnedByJoy[i] == true)
                {
                    int lane = 0;
                    GameObject laneSelector = GameObject.FindGameObjectWithTag("Lane" + (i + 1));
                    if (laneSelector != null)
                    {
                        lane = laneSelector.GetComponent<LaneSelector>().selected;
                        GameObject.Instantiate(particle, laneSelector.transform.position, Quaternion.Euler(0.0f, 0.0f, 0.0f));
                    }

                    reward.Execute(i, lane);
                    ConstructRitual(Random.Range(3, 9), (Ritual.Difficulty)PlayerPrefs.GetInt("difficulty"));

                    if (OnComplete != null)
                    {
                        OnComplete(i);
                    }
                    break;
                }
            }
        }
    }
}
