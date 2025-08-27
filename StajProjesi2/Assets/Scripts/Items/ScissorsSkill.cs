using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/ScissorsSkill")]
public class ScissorsSkill : Item
{
    public GameObject scissorPrefab;
    public GameObject linePrefab;
    public float maxDistance = 15f;
    public float throwForce = 20f;

    public override void ApplyEffect(PlayerMovement player)
    {
        player.StartCoroutine(player.UseScissorSkill(this));
    }
}
