using UnityEngine;

public class SlimeProjectile : MonoBehaviour
{
    public float effectDuration = 2f;
    public float lifetime = 10f;

    [HideInInspector]
    public PlayerMovement owner;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == owner.gameObject) return;

        if (other.gameObject.CompareTag("Player"))
        {

            PlayerMovement target = other.GetComponent<PlayerMovement>();
            if (target != null && !target.IsSlowed)
            {
                Transform childOther = other.gameObject.transform.Find("MushroomManSlime");
                if(childOther != null)
                {
                    childOther.gameObject.SetActive(true);
                
                
                target.ApplySlimeEffect(effectDuration, childOther);
                
                Debug.Log(childOther.transform);
                }
                Destroy(gameObject);
            
                }
        }
    }
}