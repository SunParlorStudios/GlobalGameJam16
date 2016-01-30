using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Portrait : MonoBehaviour
{
    public Sprite neutralSprite;
    public Sprite angrySprite;
    public Sprite happySprite;
    public Sprite sadSprite;
    public Sprite hitSprite;

    public int player;

    private SpriteRenderer renderer_;
    private State state_;

    public float returnSpeed;
    public float showTime;
    public float neutralWobble;
    public float angryWobble;
    public float happyWobble;
    public float sadnessLower;
    public float hitHeight;

    public float wobbleSpeedNeutral;
    public float wobbleSpeedAngry;
    public float wobbleSpeedHappy;

    private float animationTimer_;
    private float wobbleTimer_;

    private float last_;

    private Dictionary<State, Sprite> spriteMapping_;

    private Ritual currentRitual_;
    private PortraitHealth portraitHealth_;

    public enum State
    {
        Neutral,
        Angry,
        Happy,
        Sad,
        Hit
    }

    void OnAcquire(int joystick)
    {
        if (joystick != player)
        {
            Show(State.Angry);
            return;
        }

        Show(State.Happy);
    }

    void OnMiss(int joystick, float time, float magnitude)
    {
        if (joystick == player)
        {
            Show(State.Sad);
        }
    }

    void OnHit()
    {
        Show(State.Hit);
    }

    void Show(State state)
    {
        state_ = state;
        animationTimer_ = 0.0f;
        renderer_.sprite = spriteMapping_[state];
        last_ = transform.localPosition.y;
    }

	void Awake()
    {
        spriteMapping_ = new Dictionary<State, Sprite>()
        {
            { State.Neutral, neutralSprite },
            { State.Happy, happySprite },
            { State.Angry, angrySprite },
            { State.Hit, hitSprite },
            { State.Sad, sadSprite }
        };

        renderer_ = GetComponent<SpriteRenderer>();

        currentRitual_ = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().currentRitual;

        currentRitual_.OnComplete += OnAcquire;
        currentRitual_.OnReset += OnMiss;

        portraitHealth_ = GetComponent<PortraitHealth>();
        portraitHealth_.OnPortraitHit += OnHit;
    }

    float UpdateAnimation(float currentY)
    {
        float y = currentY;

        animationTimer_ += Time.deltaTime;
        animationTimer_ = Mathf.Clamp(animationTimer_, 0.0f, showTime);

        float ratio = animationTimer_ / showTime;

        switch (state_)
        {
            case State.Neutral:
                wobbleTimer_ += Time.deltaTime * wobbleSpeedNeutral;
                y = Mathf.Lerp(currentY, Mathf.Sin(wobbleTimer_) * neutralWobble, (1.0f - Mathf.Pow(0.1f, Time.deltaTime)));
                break;

            case State.Happy:
                wobbleTimer_ += Time.deltaTime * wobbleSpeedHappy;
                y = Mathf.Abs(Mathf.Sin(wobbleTimer_)) * happyWobble;
                break;

            case State.Angry:
                wobbleTimer_ += Time.deltaTime * wobbleSpeedAngry;
                y = Mathf.Sin(Mathf.Cos(wobbleTimer_)) * angryWobble;
                break;

            case State.Sad:
                y = Mathf.SmoothStep(last_, last_ - sadnessLower, ratio);
                break;

            case State.Hit:
                break;
        }

        if (ratio == 1.0f)
        {
            Show(State.Neutral);
        }

        if (y < -1.5f)
        {
            y = -1.5f;
        }

        return y;
    }

	void Update()
    {
        Vector3 p = transform.localPosition;

        p.y = UpdateAnimation(p.y);


        transform.localPosition = p;

        if (wobbleTimer_ > Mathf.PI * 2.0f)
        {
            wobbleTimer_ = 0.0f;
        }
	}
}
