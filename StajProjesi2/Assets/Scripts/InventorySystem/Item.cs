using UnityEngine;

[CreateAssetMenu(menuName = "Item")]
public class Item : ScriptableObject
{
    public string itemName;
    public Sprite icon;

    public virtual void ApplyEffect(PlayerMovement player)
    {
        Debug.Log($"{itemName} etkisi {player.name} üzerinde kullanýldý.");
    }
}
