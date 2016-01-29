using UnityEngine;
using System.Collections;
using System;

public class PowerupReward : Reward
{
    public enum PowerupType
    {
        ClearLane,
        Shockwave,
        Shield,
        Inferno
    }

    private PowerupType type;

    public PowerupReward(PowerupType type_)
    {
        type = type_;
    }

    public override void Execute(int player, int lane)
    {

    }

    public override bool IsLaneBound()
    {
        switch (type)
        {
            case PowerupType.ClearLane:
                return true;
                break;
            default:
                return false;
                break;
        }
    }
}
