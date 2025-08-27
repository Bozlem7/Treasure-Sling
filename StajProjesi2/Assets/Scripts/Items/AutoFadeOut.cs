using System.Collections.Generic;
using UnityEngine;

public class AutoFadeOut : MonoBehaviour
{
    public float fadeDuration = 2f;
    private float fadeTimer = 0f;
    private bool isFading = false;

    // store per-renderer per-material original colors
    private List<Renderer> renderers = new List<Renderer>();
    private List<Color[]> originalColors = new List<Color[]>();
    private List<Material[]> instancedMaterials = new List<Material[]>();

    void Awake()
    {
        // gather all renderers in this object (and children)
        renderers.AddRange(GetComponentsInChildren<Renderer>());

        // store original colors and instance materials
        for (int i = 0; i < renderers.Count; i++)
        {
            var r = renderers[i];
            Material[] mats = r.materials; // this creates instances if necessary
            instancedMaterials.Add(mats);

            Color[] cols = new Color[mats.Length];
            for (int m = 0; m < mats.Length; m++)
            {
                Material mat = mats[m];
                // ensure material supports color alpha change by switching render mode where possible
                TryMakeMaterialTransparent(mat);
                if (mat.HasProperty("_Color"))
                    cols[m] = mat.color;
                else if (mat.HasProperty("_BaseColor"))
                    cols[m] = mat.GetColor("_BaseColor");
                else
                    cols[m] = Color.white;
            }
            originalColors.Add(cols);
        }
    }

    void Update()
    {
        if (!isFading) return;

        fadeTimer += Time.deltaTime;
        float t = Mathf.Clamp01(fadeTimer / fadeDuration);
        float alpha = Mathf.Lerp(1f, 0f, t);

        for (int i = 0; i < instancedMaterials.Count; i++)
        {
            var mats = instancedMaterials[i];
            var orig = originalColors[i];
            for (int m = 0; m < mats.Length; m++)
            {
                Material mat = mats[m];
                if (mat == null) continue;

                if (mat.HasProperty("_Color"))
                {
                    Color c = orig[m];
                    c.a = alpha;
                    mat.color = c;
                }
                else if (mat.HasProperty("_BaseColor"))
                {
                    Color c = orig[m];
                    c.a = alpha;
                    mat.SetColor("_BaseColor", c);
                }
                // else: shader may not expose a color — can't fade it easily
            }
        }

        if (fadeTimer >= fadeDuration)
        {
            Destroy(gameObject);
        }
    }

    public void StartFade()
    {
        if (isFading) return;
        isFading = true;
        fadeTimer = 0f;
    }

    private void TryMakeMaterialTransparent(Material mat)
    {
        if (mat == null) return;
        // Try common Standard shader properties:
        // Set blending to transparent where possible.
        if (mat.HasProperty("_Mode"))
        {
            // Standard shader (legacy). 3 = Transparent
            mat.SetFloat("_Mode", 3);
            mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            mat.SetInt("_ZWrite", 0);
            mat.DisableKeyword("_ALPHATEST_ON");
            mat.EnableKeyword("_ALPHABLEND_ON");
            mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            mat.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
        }
        else
        {
            // URP / HDRP might use _Surface or _BlendMode etc. Try common fallbacks:
            if (mat.HasProperty("_Surface"))
            {
                // 0 = Opaque, 1 = Transparent (URP lit)
                mat.SetFloat("_Surface", 1f);
            }
            if (mat.HasProperty("_Blend"))
            {
                // some shader variants
                mat.SetFloat("_Blend", 0f);
            }
            // best effort: set renderQueue
            mat.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
        }
    }
}
