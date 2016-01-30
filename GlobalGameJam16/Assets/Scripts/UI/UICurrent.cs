using UnityEngine;
using System.Collections;

public class UICurrent : MonoBehaviour
{
    public float clockAnimationTime;

    private float current_;
    private float from_;
    private float to_;

    private float clockTimer_;
    private SpriteRenderer renderer_;

    private GameObject parent_;
    private SpriteRenderer parentRenderer_;

    private bool state_;

    void Awake()
    {
        renderer_ = GetComponent<SpriteRenderer>();
        from_ = current_ = 0.0f;

        parent_ = transform.parent.gameObject;
        parentRenderer_ = parent_.GetComponent<SpriteRenderer>();

        state_ = false;

        Set(state_);
    }

    public void Set(bool value)
    {
        if (state_ == value)
        {
            return;
        }

        state_ = value;

        from_ = current_;
        to_ = state_ == true ? 1.0f : 0.0f;
        clockTimer_ = 0.0f;
    }

    void Update()
    {
        clockTimer_ += Time.deltaTime;
        clockTimer_ = Mathf.Clamp(clockTimer_, 0.0f, clockAnimationTime);

        current_ = Mathf.SmoothStep(from_, to_, clockTimer_ / clockAnimationTime);

        renderer_.material.SetFloat("_Ratio", current_);

        Color col = renderer_.color;
        col.a = parentRenderer_.color.a;

        renderer_.color = col;
    }
}
