using UnityEngine;
using System.Collections;

public class UIKey : MonoBehaviour
{
    public float animationDelayPerKey;
    public float animationTime;

    private float animationTimer_;
    private float animationDelay_;

    private SpriteRenderer renderer_;

	void Awake()
    {
        renderer_ = GetComponent<SpriteRenderer>();
        Reset();
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
	
	void Update()
    {
        animationTimer_ += Time.deltaTime;
        animationTimer_ = Mathf.Clamp(animationTimer_, -animationDelay_, animationTime);

        float ratio = animationTimer_ / animationTime;
        ratio = Mathf.Clamp(ratio, 0.0f, 1.0f);

        renderer_.color = new Color(1.0f, 1.0f, 1.0f, ratio);

        float scale = Mathf.SmoothStep(2.0f, 1.0f, ratio);
        transform.localScale = new Vector3(scale, scale, 1.0f);
    }
}
