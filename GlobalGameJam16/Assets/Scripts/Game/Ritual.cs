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

    public struct RitualKey
    {
        public KeyCodes keyCode;
        public bool[] earnedByJoy;
    }

    public delegate void OnChange(int count);
    public event OnChange Changed;

    public delegate void PlayerReset(int joystick, float time, float magnitude);
    public event PlayerReset OnReset;

    public delegate void KeyPressed(int index);
    public event KeyPressed OnPress;

    private const float rumbleStrength_ = 2.0f;
    private const float rumbleTime_ = 0.5f;

    public List<RitualKey> ritual;
    private Reward reward;

    private KeyCodes lastKey;

    public void ConstructRitual(int length, Difficulty difficulty)
    {
        reward = Reward.GetReward(length);

        ritual = new List<RitualKey>();

        List<KeyCodes> keyCodesPool = new List<KeyCodes>();

        keyCodesPool.Add(KeyCodes.A);
        keyCodesPool.Add(KeyCodes.B);
        keyCodesPool.Add(KeyCodes.X);
        keyCodesPool.Add(KeyCodes.Y);

        if (difficulty == Difficulty.Medium)
        {
            keyCodesPool.Add(KeyCodes.RB);
            keyCodesPool.Add(KeyCodes.LB);
            keyCodesPool.Add(KeyCodes.RT);
            keyCodesPool.Add(KeyCodes.LT);
        }
        else if (difficulty == Difficulty.Hard)
        {
            keyCodesPool.Add(KeyCodes.RB);
            keyCodesPool.Add(KeyCodes.LB);
            keyCodesPool.Add(KeyCodes.RT);
            keyCodesPool.Add(KeyCodes.LT);
            keyCodesPool.Add(KeyCodes.Up);
            keyCodesPool.Add(KeyCodes.Down);
            keyCodesPool.Add(KeyCodes.Left);
            keyCodesPool.Add(KeyCodes.Right);
        }

        while (length >= 0)
        {
            length--;
            
            RitualKey ritualKey = new RitualKey();
            ritualKey.earnedByJoy = new bool[2];
            ritualKey.earnedByJoy[0] = false;
            ritualKey.earnedByJoy[1] = false;

            if (length == -1 && reward.IsLaneBound())
            {
                ritualKey.keyCode = KeyCodes.Last;
            }
            else
            {
                int idx = Random.Range(0, keyCodesPool.Count - 1);
                ritualKey.keyCode = keyCodesPool[idx];
            }

            ritual.Add(ritualKey);
        }

        string ritualKeys = "";

        for (int i = 0; i < ritual.Count; i++)
        {
            ritualKeys += ritual[i].keyCode.ToString() + " | ";
        }

        Debug.Log(ritualKeys);

        PostChangedEvent();
    }

    public void EnterInput(int joystick, KeyCodes keyCode)
    {
        int ritualIndex = GetCurrentIndex(joystick) + 1;

        if ((ritual[ritualIndex].keyCode == KeyCodes.Last && KeyMapping.IsLastKey(keyCode)) || ritual[ritualIndex].keyCode == keyCode)
        {
            lastKey = keyCode;
            ritual[ritualIndex].earnedByJoy[joystick] = true;

            if (OnPress != null)
            {
                OnPress(ritualIndex);
            }
        }
        else
        {
            Debug.Log("Penile dysfunction for player " + joystick);
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

    private int KeyCodeToLane(KeyCodes keyCode)
    {
        switch (keyCode)
        {
            case KeyCodes.A:
                return 0;

            case KeyCodes.B:
                return 1;

            case KeyCodes.X:
                return 2;

            case KeyCodes.Y:
                return 3;

            default:
                return -1;

        }
    }

    public void PostChangedEvent()
    {
        if (Changed != null)
        {
            Changed(ritual.Count);
        }
    }

    public void ExecuteReward()
    {
        if (IsComplete() == true)
        {
            for (int i = 0; i < 2; i++)
            {
                if (ritual[ritual.Count - 1].earnedByJoy[i] == true)
                {
                    reward.Execute(i, KeyCodeToLane(lastKey));
                    ConstructRitual(Random.Range(4, 9), Difficulty.Hard);
                    break;
                }
            }
        }
    }
}
