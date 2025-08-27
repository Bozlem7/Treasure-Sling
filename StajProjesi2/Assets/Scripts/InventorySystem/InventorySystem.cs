using System.Collections.Generic;
using UnityEngine;


public class InventorySystem : MonoBehaviour
{
    public int maxSlots = 3;
    public List<Item> items = new List<Item>();

    public bool IsFull => items.Count >= maxSlots;

    public bool AddItem(Item item)
    {
        if (IsFull)
        {
            return false;
        }

        items.Add(item);
        return true;
    }

    public void UseItem(int index, PlayerMovement player)
    {
        if (index < 0 || index >= items.Count)
            return;

        items[index].ApplyEffect(player);
        items.RemoveAt(index);
    }
}
