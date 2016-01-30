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
        GameObject[] units = GameObject.FindGameObjectsWithTag("Unit");

        switch (type)
        {
            case PowerupType.ClearLane:

                break;
            case PowerupType.Shockwave:

                break;
            case PowerupType.Shield:
                Unit unit;
                for (int i = 0; i < units.Length; i++)
                {
                    unit = units[i].GetComponent<Unit>();
                    if (unit.belongsToPlayer == player)
                    {
                        unit.EnableShield(3.0f);
                    }
                }
                break;
            case PowerupType.Inferno:

                break;
        }
    }

    public override bool IsLaneBound()
    {
        switch (type)
        {
            case PowerupType.ClearLane:
                return true;
            default:
                return false;
        }
    }
}
