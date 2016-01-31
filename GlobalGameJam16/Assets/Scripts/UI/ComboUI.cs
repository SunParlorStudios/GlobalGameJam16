using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ComboUI : MonoBehaviour
{
    public float rotationOffset;
    public float slideTime;
    public Vector2 animationOffset;
    public float wobble;
    public float wobbleSpeed;

    private float animationTimer_;
    private float wobbleTimer_;

    private Vector2 startAt_;
    private CanvasRenderer[] renderers_;
    private Text[] texts_;

    public int playerId;
    private int currentCombo_;
    private Ritual currentRitual_;

    void Start()
    {
        startAt_ = GetComponent<RectTransform>().localPosition;

        renderers_ = GetComponentsInChildren<CanvasRenderer>();
        texts_ = GetComponentsInChildren<Text>();

        currentCombo_ = 0;

        currentRitual_ = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().currentRitual;
        currentRitual_.OnReset += OnFailed;
        currentRitual_.OnComplete += OnComplete;

        Reset(0);
    }

    public void SetVisible(bool visible)
    {
        gameObject.SetActive(visible);
    }

    public void Reset(int count)
    {
        wobbleTimer_ = 0.0f;
        animationTimer_ = 0.0f;

        if (count > 0)
        {
            SetVisible(true);

            for (int i = 0; i < texts_.Length; ++i)
            {
                texts_[i].text = count + "x\nCombo";
            }

            return;
        }

        currentCombo_ = 0;
        SetVisible(false);
    }

    public void OnFailed(int joystick, float time, float magnitude)
    {
        if (joystick == playerId)
        {
            Reset(0);
        }
    }

    public void OnComplete(int joystick)
    {
        if (joystick == playerId)
        {
            Reset(++currentCombo_);
            return;
        }

        Reset(0);
    }
    
	void Update()
    {
        animationTimer_ += Time.deltaTime;
        animationTimer_ = Mathf.Clamp(animationTimer_, 0.0f, slideTime);

        float ratio = animationTimer_ / slideTime;

        Vector2 newPosition = Vector2.Lerp(startAt_ + animationOffset, startAt_, ratio);

        GetComponent<RectTransform>().localPosition = newPosition;

        for (int i = 0; i < renderers_.Length; ++i)
        {
            renderers_[i].SetAlpha(ratio);
        }

        wobbleTimer_ += Time.deltaTime * wobbleSpeed;

        if (wobbleTimer_ > Mathf.PI * 2.0f)
        {
            wobbleTimer_ = 0.0f;
        }

        transform.rotation = Quaternion.Euler(0.0f, 0.0f, Mathf.Sin(wobbleTimer_) * wobble + rotationOffset);
    }
}
