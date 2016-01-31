using UnityEngine;
using System.Collections;

public class LaneSelector : MonoBehaviour
{
    public int joystick;
    public int selected;
    private bool pressed_;

	void Start()
    {
        pressed_ = false;
        selected = 0;
	}

	void Update()
    {
        float y = Input.GetAxis("Joystick" + (joystick + 1) + "ThumbY");

        if (Mathf.Abs(y) > 0.3f && pressed_ == false)
        {
            float direction = y / Mathf.Abs(y);

            selected += (int)direction;

            int old = selected;

            selected = Mathf.Clamp(selected, 0, 3);

            if (selected == old)
            {
                transform.Translate(0.0f, direction * -1.2f, 0.0f);
            }

            pressed_ = true;
        }
        else if (Mathf.Abs(y) < 0.3f)
        {
            pressed_ = false;
        }
	}
}
