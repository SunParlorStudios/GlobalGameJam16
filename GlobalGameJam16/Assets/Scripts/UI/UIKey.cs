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

    public Sprite currentFullSprite;
    public Sprite currentEmptySprite;

    private SpriteRenderer renderer_;
    private SpriteRenderer half1Renderer_;
    private SpriteRenderer half2Renderer_;

    void Awake()
    {
        renderer_ = GetComponent<SpriteRenderer>();

        half1Renderer_ = currentPlayer1.GetComponent<SpriteRenderer>();
        half2Renderer_ = currentPlayer2.GetComponent<SpriteRenderer>();

        Reset();
	}

    public void SetCurrent(int joystick, bool value)
    {
        SpriteRenderer toChange = joystick == 0 ? half1Renderer_ : half2Renderer_;
        toChange.sprite = value == true ? currentFullSprite : currentEmptySprite;
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
        animationTimer_ = -animationDelay_;
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
	
	void Update()
    {
        animationTimer_ += Time.deltaTime;
        animationTimer_ = Mathf.Clamp(animationTimer_, -animationDelay_, animationTime);

        float ratio = animationTimer_ / animationTime;
        ratio = Mathf.Clamp(ratio, 0.0f, 1.0f);

        UpdateColors(ratio);

        float scale = Mathf.SmoothStep(2.0f, 1.0f, ratio);
        transform.localScale = new Vector3(scale, scale, 1.0f);
    }
}
