using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(Rigidbody))]
public class Slice : MonoBehaviour
{
    public SliceOptions sliceOptions;
    public CallbackOptions callbackOptions;

    private int currentSliceCount;
    private GameObject fragmentRoot;

    public void ComputeSlice(Vector3 sliceNormalWorld, Vector3 sliceOriginWorld)
    {
        var mesh = GetComponent<MeshFilter>().sharedMesh;

        if (mesh != null)
        {
            if (fragmentRoot == null)
            {
                fragmentRoot = new GameObject($"{name}Slices");
                fragmentRoot.transform.SetParent(transform.parent);
                fragmentRoot.transform.position = transform.position;
                fragmentRoot.transform.rotation = transform.rotation;
                fragmentRoot.transform.localScale = Vector3.one;
            }

            var sliceTemplate = CreateSliceTemplate();
            var sliceNormalLocal = transform.InverseTransformDirection(sliceNormalWorld);
            var sliceOriginLocal = transform.InverseTransformPoint(sliceOriginWorld);

            Fragmenter.Slice(gameObject,
                             sliceNormalLocal,
                             sliceOriginLocal,
                             sliceOptions,
                             sliceTemplate,
                             fragmentRoot.transform);

            Destroy(sliceTemplate);
            gameObject.SetActive(false);

            foreach (Transform t in fragmentRoot.GetComponentsInChildren<Transform>())
            {
                if (t == fragmentRoot.transform) continue;
                if (t.gameObject.GetComponent<AutoFadeOut>() == null)
                {
                    t.gameObject.AddComponent<AutoFadeOut>();
                }
            }

            if (callbackOptions.onCompleted != null)
            {
                callbackOptions.onCompleted.Invoke();
            }
        }
    }

    private void ExplodeFragments(Vector3 direction)
    {
        foreach (Transform child in fragmentRoot.transform)
        {
            Rigidbody rb = child.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 offset = direction.normalized * Random.Range(0.1f, 0.3f);
                rb.position += offset;
                rb.velocity += offset * 3f;
            }

            AutoFadeOut fade = child.GetComponent<AutoFadeOut>();
            if (fade != null && child.gameObject.activeInHierarchy)
            {
            }
        }
    }

    private GameObject CreateSliceTemplate()
    {
        GameObject obj = new GameObject();
        obj.name = "Slice";
        obj.tag = tag;

        obj.AddComponent<MeshFilter>();

        var meshRenderer = obj.AddComponent<MeshRenderer>();
        meshRenderer.sharedMaterials = new Material[2] {
            GetComponent<MeshRenderer>().sharedMaterial,
            sliceOptions.insideMaterial
        };

        var thisCollider = GetComponent<Collider>();
        var fragmentCollider = obj.AddComponent<MeshCollider>();
        fragmentCollider.convex = true;
        fragmentCollider.sharedMaterial = thisCollider.sharedMaterial;
        fragmentCollider.isTrigger = thisCollider.isTrigger;

        var thisRigidBody = GetComponent<Rigidbody>();
        var fragmentRigidBody = obj.AddComponent<Rigidbody>();
        fragmentRigidBody.velocity = thisRigidBody.velocity;
        fragmentRigidBody.angularVelocity = thisRigidBody.angularVelocity;
        fragmentRigidBody.drag = thisRigidBody.drag;
        fragmentRigidBody.angularDrag = thisRigidBody.angularDrag;
        fragmentRigidBody.useGravity = thisRigidBody.useGravity;

        if (sliceOptions.enableReslicing && (currentSliceCount < sliceOptions.maxResliceCount))
        {
            CopySliceComponent(obj);
        }

        return obj;
    }

    private void CopySliceComponent(GameObject obj)
    {
        var sliceComponent = obj.AddComponent<Slice>();
        sliceComponent.sliceOptions = sliceOptions;
        sliceComponent.callbackOptions = callbackOptions;
        sliceComponent.currentSliceCount = currentSliceCount + 1;
        sliceComponent.fragmentRoot = fragmentRoot;
    }
}
