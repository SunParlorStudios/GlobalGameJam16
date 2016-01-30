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
                return LaneKeyCode.A;
            case KeyCodes.B:
                return LaneKeyCode.B;
            case KeyCodes.X:
                return LaneKeyCode.X;
            case KeyCodes.Y:
                return LaneKeyCode.Y;
            default:
                return LaneKeyCode.A;
        }
    }

    public Transform GetPlayerSpawn(int player, int laneNumber)
    {
        Lane lane = lanes[laneNumber];

        Debug.Log("Lane id: " + player + " with child count: " + lane.transform.childCount);

        for (int i = 0; i < lane.transform.childCount; i++)
        {
            if (lane.transform.GetChild(i).tag.Contains((player + 1).ToString()))
            {
                Debug.Log("yes");
                return lane.transform.GetChild(i);
            }
        }

        return transform;
    }
}
