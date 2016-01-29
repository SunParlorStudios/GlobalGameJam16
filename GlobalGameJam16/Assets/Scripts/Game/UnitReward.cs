using UnityEngine;
using System.Collections;
using System;

public class UnitReward : Reward
{
    public enum UnitType
    {
        Small,
        Medium,
        Big
    }

    public UnitReward(UnitType type)
    {

    }

    public override void Execute(int player, int lane)
    {

    }

    public override bool IsLaneBound()
    {
        return true;
    }
}
