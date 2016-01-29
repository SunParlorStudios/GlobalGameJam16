using UnityEngine;
using System.Collections.Generic;

public class LaneManager : MonoBehaviour
{
    public List<Lane> lanes;

    public void Awake()
    {
        lanes = new List<Lane>();
    }
}
