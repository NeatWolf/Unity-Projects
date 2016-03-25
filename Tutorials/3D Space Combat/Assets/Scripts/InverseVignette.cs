using UnityEngine;
using UnityStandardAssets.ImageEffects;

[RequireComponent(typeof(Camera))]
public class InverseVignette : MonoBehaviour
{
    public Shader vignetteShader;

    public float intensity = 0.036f;
    public float blur = 0.0f;

    private Material vignetteMaterial = null;
    private bool isOn = false;

    private Material GetMaterial()
    {
        if (vignetteMaterial == null)
        {
            vignetteMaterial = new Material(vignetteShader);
            vignetteMaterial.hideFlags = HideFlags.HideAndDontSave;
        }
        return vignetteMaterial;
    }

    void Start()
    {
        if (vignetteShader == null)
        {
            Debug.LogError("shader missing!", this);
        }
    }

    public void TurnOn()
    {
        isOn = true;
    }

    public void TurnOff()
    {
        isOn = false;
    }

    void OnRenderImage(RenderTexture source, RenderTexture dest)
    {
        if (isOn)
        {
            GetMaterial().SetFloat("_Intensity", intensity);
            GetMaterial().SetFloat("_Blur", blur);
            Graphics.Blit(source, dest, GetMaterial());
        }
        else
        {
            Graphics.Blit(source, dest);
        }
    }
}