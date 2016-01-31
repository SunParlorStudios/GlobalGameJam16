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

        LaneManager laneManager = GameObject.FindGameObjectWithTag("LaneManager").GetComponent<LaneManager>();

        switch (type)
        {
            case PowerupType.ClearLane:
                if (player == 0)
                {
                    GameObject obj = (GameObject)GameObject.Instantiate(Resources.Load("Powerups/PowerupLight"));
                    Cross cross = obj.GetComponent<Cross>();
                    cross.lane = lane;
                    cross.goingRight = true;
                    cross.dropTo = laneManager.GetPlayerSpawn(0, lane).position + Vector3.up * 0.5f;
                }
                else
                {
                    GameObject obj = (GameObject)GameObject.Instantiate(Resources.Load("Powerups/PowerupFire"));
                    Cross cross = obj.GetComponent<Cross>();
                    cross.lane = lane;
                    cross.goingRight = false;
                    cross.dropTo = laneManager.GetPlayerSpawn(1, lane).position + Vector3.up * 0.5f;
                }
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
