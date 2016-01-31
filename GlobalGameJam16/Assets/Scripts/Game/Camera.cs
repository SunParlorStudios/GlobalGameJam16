using UnityEngine;
using System.Collections;

public class Camera : MonoBehaviour
{
    private Material imageDistortion;
    public Shader imageDistortionShader;

    public void Awake()
    {
        imageDistortion = new Material(imageDistortionShader);
    }

    public void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, imageDistortion);
    }
}
