using UnityEngine;

public class Scissor : MonoBehaviour
{
    public float destroyDelay = 1.5f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            var slice = other.GetComponent<Slice>();
            if (slice != null)
            {
                Vector3 baseNormal = transform.forward;

                Vector3 cutNormal = (baseNormal + transform.up * 0.25f).normalized;
                Vector3 cutOrigin = transform.position;

                var renderer = other.GetComponent<MeshRenderer>();
                if (renderer != null && renderer.material.name.StartsWith("HighlightSlice"))
                {
                    renderer.material.SetVector("CutPlaneNormal", cutNormal);
                    renderer.material.SetVector("CutPlaneOrigin", cutOrigin);
                }

                slice.ComputeSlice(cutNormal, cutOrigin);

                AutoFadeOut[] fades = FindObjectsOfType<AutoFadeOut>();
                foreach (var fade in fades)
                {
                    fade.StartFade();
                }
            }

            Destroy(gameObject, 0.05f);
        }
        else
        {
            Destroy(gameObject, destroyDelay);
        }
    }
}
