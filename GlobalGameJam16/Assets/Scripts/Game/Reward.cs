﻿using UnityEngine;
using System.Collections;

public abstract class Reward
{
    public abstract void Execute(int player, int lane = -1);

    public abstract bool IsLaneBound();

    public static bool used = false;

    public static Reward GetReward(int length)
    {
        int chance = Random.Range(0, 100);
        if (chance <= 75)
        {
            UnitReward.UnitType type;

            if (length >= 8)
            {
                type = UnitReward.UnitType.Big;
            }
            else if (length >= 6)
            {
                type = UnitReward.UnitType.Medium;
            }
            else
            {
                type = UnitReward.UnitType.Small;
            }
            used = true;
            return new UnitReward(type);
        }
        else
        {
            return new PowerupReward((PowerupReward.PowerupType)Random.Range(0, System.Enum.GetValues(typeof(PowerupReward.PowerupType)).Length - 1));
        }
    }
}
