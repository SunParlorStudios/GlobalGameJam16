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
        LaneManager laneManager = GameObject.FindGameObjectWithTag("LaneManager").GetComponent<LaneManager>();

        GameObject unit = (GameObject)GameObject.Instantiate(Resources.Load("Units/Sheep"), laneManager.GetPlayerSpawn(player, lane).position, Quaternion.identity);
        unit.GetComponent<Sheep>().facingRight = (player == 0 ? true : false);
    }

    public override bool IsLaneBound()
    {
        return true;
    }
}
