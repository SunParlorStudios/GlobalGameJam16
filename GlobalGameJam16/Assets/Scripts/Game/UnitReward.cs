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

    private UnitType type;

    public UnitReward(UnitType type_)
    {
        type = type_;
    }

    public override void Execute(int player, int lane)
    {
        LaneManager laneManager = GameObject.FindGameObjectWithTag("LaneManager").GetComponent<LaneManager>();

        string toLoad = "";

        switch (type)
        {
            case UnitType.Small:
                if (player == 0)
                    toLoad = "Units/Sheep";
                else
                    toLoad = "Units/Chick";
                break;
            case UnitType.Medium:
                if (player == 0)
                    toLoad = "Units/BlackSheep";
                else
                    toLoad = "Units/Chicken";
                break;
            case UnitType.Big:
                if (player == 0)
                    toLoad = "Units/Ram";
                else
                    toLoad = "Units/Turkey";
                break;
        }

        GameObject unit = (GameObject)GameObject.Instantiate(Resources.Load(toLoad), laneManager.GetPlayerSpawn(player, lane).position, Quaternion.identity);
        unit.GetComponent<Unit>().SetFacing(player == 0 ? true : false);
        unit.GetComponent<Unit>().belongsToPlayer = player;
        unit.GetComponent<Unit>().lane = lane;
    }

    public override bool IsLaneBound()
    {
        return true;
    }
}
