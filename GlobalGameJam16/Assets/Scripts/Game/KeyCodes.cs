using UnityEngine;
using System.Collections.Generic;

public enum KeyCodes : int
{
    A,
    B,
    X,
    Y,
    Up,
    Down,
    Left,
    Right,
    RB,
    LB,
    RT,
    LT,
    Last
}

public class KeyMapping
{
    private static Dictionary<KeyCodes, string>[] map_joy;

    public static void Initialise()
    {
        map_joy = new Dictionary<KeyCodes, string>[2];
        map_joy[0] = new Dictionary<KeyCodes, string>();
        map_joy[1] = new Dictionary<KeyCodes, string>();

        map_joy[0].Add(KeyCodes.A, "Joystick1A");
        map_joy[0].Add(KeyCodes.B, "Joystick1B");
        map_joy[0].Add(KeyCodes.X, "Joystick1X");
        map_joy[0].Add(KeyCodes.Y, "Joystick1Y");
        map_joy[0].Add(KeyCodes.Up, "Joystick1DPY");
        map_joy[0].Add(KeyCodes.Down, "Joystick1DPY");
        map_joy[0].Add(KeyCodes.Left, "Joystick1DPX");
        map_joy[0].Add(KeyCodes.Right, "Joystick1DPX");
        map_joy[0].Add(KeyCodes.RB, "Joystick1RB");
        map_joy[0].Add(KeyCodes.LB, "Joystick1LB");
        map_joy[0].Add(KeyCodes.RT, "Joystick1Trigger");
        map_joy[0].Add(KeyCodes.LT, "Joystick1Trigger");

        map_joy[1].Add(KeyCodes.A, "Joystick2A");
        map_joy[1].Add(KeyCodes.B, "Joystick2B");
        map_joy[1].Add(KeyCodes.X, "Joystick2X");
        map_joy[1].Add(KeyCodes.Y, "Joystick2Y");
        map_joy[1].Add(KeyCodes.Up, "Joystick2DPY");
        map_joy[1].Add(KeyCodes.Down, "Joystick2DPY");
        map_joy[1].Add(KeyCodes.Left, "Joystick2DPX");
        map_joy[1].Add(KeyCodes.Right, "Joystick2DPX");
        map_joy[1].Add(KeyCodes.RB, "Joystick2RB");
        map_joy[1].Add(KeyCodes.LB, "Joystick2LB");
        map_joy[1].Add(KeyCodes.RT, "Joystick2Trigger");
        map_joy[1].Add(KeyCodes.LT, "Joystick2Trigger");
    }

    public static string Get(int joystick, KeyCodes kc)
    {
        return map_joy[joystick][kc];
    }

    public static bool IsLastKey(KeyCodes kc)
    {
        return kc == KeyCodes.A || kc == KeyCodes.B || kc == KeyCodes.X || kc == KeyCodes.Y;
    }

    public static bool IsAxisPressed(KeyCodes keyCode, float value)
    {
        switch (keyCode)
        {
            case KeyCodes.LT:
            case KeyCodes.Down:
            case KeyCodes.Left:
                if (value <= -0.3f)
                {
                    return true;
                }
                break;
            default:
                return value >= 0.3f;
                break;
        }

        return false;
    }

    public static bool NoKeysPressed(int joystick)
    {
        System.Array array = System.Enum.GetValues(typeof(KeyCodes));

        for (int j = 0; j < array.Length; j++)
        {
            if ((KeyCodes)j != KeyCodes.Last && IsAxisPressed((KeyCodes)j, Input.GetAxisRaw(KeyMapping.Get(joystick, (KeyCodes)j))))
            {
                return false;
            }
        }

        return true;
    }
}