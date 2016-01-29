using UnityEngine;
using System.Collections;

public abstract class Reward
{
    public abstract void Execute(int player, int lane = -1);

    public abstract bool IsLaneBound();

    public static Reward GetReward(int length)
    {
        if (Random.Range(0, 1) == 0)
        {
            return new UnitReward((UnitReward.UnitType)Mathf.Floor(length / 3));
        }
        else
        {
            return new PowerupReward((PowerupReward.PowerupType)Random.Range(0, System.Enum.GetValues(typeof(PowerupReward.PowerupType)).Length - 1));
        }
    }
}
