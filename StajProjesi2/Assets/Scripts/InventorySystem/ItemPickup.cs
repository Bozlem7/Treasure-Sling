using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Item itemData;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var inventory = other.GetComponent<InventorySystem>();
            if (inventory != null && itemData != null)
            {
                if (inventory.AddItem(itemData))
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
