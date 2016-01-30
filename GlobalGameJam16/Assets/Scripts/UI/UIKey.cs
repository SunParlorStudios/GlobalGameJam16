using UnityEngine;
using System.Collections;

public class UIKey : MonoBehaviour
{
    public float animationDelayPerKey;
    public float animationTime;

    private float animationTimer_;
    private float animationDelay_;

    public GameObject currentPlayer1;
    public GameObject currentPlayer2;

    private SpriteRenderer renderer_;
    private SpriteRenderer half1Renderer_;
    private SpriteRenderer half2Renderer_;

    private UICurrent half1Current_;
    private UICurrent half2Current_;

    public float maxWobbleSpeed;
    public float maxWobble;

    private float wobble_;
    private float wobbleSpeed_;

    public float pressedAnimationSpeed;
    public float pressedScale;

    private float currentScale_;
    private float fromScale_;
    private float toScale_;
    private float pressedTimer_;

    void Awake()
    {
        renderer_ = GetComponent<SpriteRenderer>();

        half1Renderer_ = currentPlayer1.GetComponent<SpriteRenderer>();
        half2Renderer_ = currentPlayer2.GetComponent<SpriteRenderer>();

        half1Current_ = currentPlayer1.GetComponentInChildren<UICurrent>();
        half2Current_ = currentPlayer2.GetComponentInChildren<UICurrent>();

        Reset();
	}

    public void SetCurrent(int joystick, bool value)
    {
        UICurrent current = joystick == 0 ? half1Current_ : half2Current_;
        current.Set(value);
    }

    public void Press()
    {
        pressedTimer_ = 0.0f;
        fromScale_ = currentScale_;
        toScale_ = pressedScale;
    }

    public void Set(Sprite sprite, int index, float spacing)
    {
        renderer_.sprite = sprite;
        animationDelay_ = animationDelayPerKey * index;
        animationTimer_ = -animationDelay_;

        transform.localPosition = new Vector3(index * spacing, 0.0f, 0.0f);
    }

    private void Reset()
    {
        wobble_ = Random.Range(0.0f, Mathf.PI * 2.0f);
        wobbleSpeed_ = Random.Range(-maxWobbleSpeed, maxWobbleSpeed);

        animationTimer_ = -animationDelay_;

        transform.localScale = Vector3.zero;
        currentScale_ = fromScale_ = toScale_ = 1.0f;
        pressedTimer_ = pressedAnimationSpeed;
    }

    void ApplyColor(SpriteRenderer renderer, float ratio)
    {
        Color col = renderer.color;
        renderer.color = new Color(col.r, col.g, col.b, ratio);
    }

    void UpdateColors(float ratio)
    {
        ApplyColor(renderer_, ratio);
        ApplyColor(half1Renderer_, ratio);
        ApplyColor(half2Renderer_, ratio);
    }

    void UpdateScaling(float ratio)
    {
        pressedTimer_ += Time.deltaTime;
        pressedTimer_ = Mathf.Clamp(pressedTimer_, 0.0f, pressedAnimationSpeed);

        currentScale_ = Mathf.SmoothStep(fromScale_, toScale_, pressedTimer_ / pressedAnimationSpeed);

        if (currentScale_ == toScale_)
        {
            pressedTimer_ = 0.0f;
            fromScale_ = currentScale_;
            toScale_ = 1.0f;
        }

        float scale = Mathf.SmoothStep(2.0f, 1.0f, ratio) * currentScale_;
        transform.localScale = new Vector3(scale, scale, 1.0f);
    }
	
	void Update()
    {
        animationTimer_ += Time.deltaTime;
        animationTimer_ = Mathf.Clamp(animationTimer_, -animationDelay_, animationTime);

        float ratio = animationTimer_ / animationTime;
        ratio = Mathf.Clamp(ratio, 0.0f, 1.0f);

        UpdateColors(ratio);
        UpdateScaling(ratio);

        wobble_ += Time.deltaTime * wobbleSpeed_;

        if (wobble_ > Mathf.PI * 2.0f)
        {
            wobble_ = 0.0f;
        }

        Vector3 p = transform.localPosition;
        transform.localPosition = new Vector3(p.x, Mathf.Sin(wobble_) * maxWobble, p.z);
    }
}
