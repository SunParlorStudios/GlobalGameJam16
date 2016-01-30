using UnityEngine;
using System.Collections.Generic;

public class LaneManager : MonoBehaviour
{
    public List<Lane> lanes;

    public enum LaneKeyCode : int
    {
        A,
        B,
        X,
        Y
    }

    public static LaneKeyCode KeyCodeToLane(KeyCodes keyCode)
    {
        switch (keyCode)
        {
            case KeyCodes.A:
                return LaneKeyCode.Y;
            case KeyCodes.B:
                return LaneKeyCode.X;
            case KeyCodes.X:
                return LaneKeyCode.B;
            case KeyCodes.Y:
                return LaneKeyCode.A;
            default:
                return LaneKeyCode.A;
        }
    }

    public Transform GetPlayerSpawn(int player, int laneNumber)
    {
        Lane lane = lanes[laneNumber];

        for (int i = 0; i < lane.transform.childCount; i++)
        {
            if (lane.transform.GetChild(i).tag.Contains((player + 1).ToString()))
            {
                return lane.transform.GetChild(i);
            }
        }

        return transform;
    }
}
