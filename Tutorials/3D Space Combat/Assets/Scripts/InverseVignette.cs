using UnityEngine;
using System;
using UnityStandardAssets.CinematicEffects;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class InverseVignette : MonoBehaviour
{
    #region Attributes
    [AttributeUsage(AttributeTargets.Field)]
    public class SettingsGroup : Attribute
    {}

    [AttributeUsage(AttributeTargets.Field)]
    public class AdvancedSetting : Attribute
    {}
    #endregion

    #region Settings

        public bool enabled = false;

        [ColorUsage(false)]
        [Tooltip("Vignette color. Use the alpha channel for transparency.")]
        public Color color = new Color(0f, 0f, 0f, 1f);

        [Tooltip("Sets the vignette center point (screen center is [0.5,0.5]).")]
        public Vector2 center = new Vector2(0.5f, 0.5f);

        [Range(0f, 3f), Tooltip("Amount of vignetting on screen.")]
        public float intensity = 1.4f;

        [Range(0.01f, 3f), Tooltip("Smoothness of the vignette borders.")]
        public float smoothness = 0.8f;

        [AdvancedSetting, Range(0f, 1f), Tooltip("Lower values will make a square-ish vignette.")]
        public float roundness = 1f;

        [Range(0f, 1f), Tooltip("Blurs the corners of the screen. Leave this at 0 to disable it.")]
        public float blur = 0f;

        [Range(0f, 1f), Tooltip("Desaturate the corners of the screen. Leave this to 0 to disable it.")]
        public float desaturate = 0f;
    #endregion

    public float Intensity
    {
        get { return intensity; }
        set { intensity = value; }
    }

    private enum Pass
    {
        BlurPrePass,
        Chroma,
        Distort,
        Vignette,
        ChromaDistort,
        ChromaVignette,
        DistortVignette,
        ChromaDistortVignette
    }

    [SerializeField]
    private Shader m_Shader;
    public Shader shader
    {
        get
        {
            if (m_Shader == null)
                m_Shader = Shader.Find("CustomImageEffect/InverseVignette");

            return m_Shader;
        }
    }

    private Material m_Material;
    public Material material
    {
        get
        {
            if (m_Material == null)
                m_Material = ImageEffectHelper.CheckShaderAndCreateMaterial(shader);

            return m_Material;
        }
    }

    private RenderTextureUtility m_RTU;

    private void OnEnable()
    {
        if (!ImageEffectHelper.IsSupported(shader, false, false, this))
            enabled = false;

        m_RTU = new RenderTextureUtility();
    }

    private void OnDisable()
    {
        if (m_Material != null)
            DestroyImmediate(m_Material);

        m_Material = null;
        m_RTU.ReleaseAllTemporaryRenderTextures();
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (!this.enabled)
        {
            Graphics.Blit(source, destination);
            return;
        }

        material.shaderKeywords = null;

        if (this.enabled)
        {
            material.SetColor("_VignetteColor", this.color);

            if (this.blur > 0f)
            {
                // Downscale + gaussian blur (2 passes)
                int w = source.width / 2;
                int h = source.height / 2;
                var rt1 = m_RTU.GetTemporaryRenderTexture(w, h, 0, source.format);
                var rt2 = m_RTU.GetTemporaryRenderTexture(w, h, 0, source.format);

                material.SetVector("_BlurPass", new Vector2(1f / w, 0f));
                Graphics.Blit(source, rt1, material, (int)Pass.BlurPrePass);

                material.SetVector("_BlurPass", new Vector2(0f, 1f / h));
                Graphics.Blit(rt1, rt2, material, (int)Pass.BlurPrePass);

                material.SetVector("_BlurPass", new Vector2(1f / w, 0f));
                Graphics.Blit(rt2, rt1, material, (int)Pass.BlurPrePass);
                material.SetVector("_BlurPass", new Vector2(0f, 1f / h));
                Graphics.Blit(rt1, rt2, material, (int)Pass.BlurPrePass);

                material.SetTexture("_BlurTex", rt2);
                material.SetFloat("_VignetteBlur", this.blur * 3f);
                material.EnableKeyword("VIGNETTE_BLUR");
            }

            if (this.desaturate > 0f)
            {
                material.EnableKeyword("VIGNETTE_DESAT");
                material.SetFloat("_VignetteDesat", 1f - this.desaturate);
            }

            material.SetVector("_VignetteCenter", this.center);

            if (Mathf.Approximately(this.roundness, 1f))
            {
                material.EnableKeyword("VIGNETTE_CLASSIC");
                material.SetVector("_VignetteSettings", new Vector2(this.intensity, this.smoothness));
            }
            else
            {
                material.EnableKeyword("VIGNETTE_FILMIC");
                float roundness = (1f - this.roundness) * 6f + this.roundness;
                material.SetVector("_VignetteSettings", new Vector3(this.intensity, this.smoothness, roundness));
            }
        }

        Graphics.Blit(source, destination, material, 1);

        m_RTU.ReleaseAllTemporaryRenderTextures();
    }
}
