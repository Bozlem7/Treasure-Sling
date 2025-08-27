using UnityEngine;

[CreateAssetMenu(menuName = "Item/Slime Skill")]
public class SlimeSkillItem : Item
{
    public GameObject slimePrefab;
    public GameObject rangeIndicatorPrefab;
    public float throwForce = 15f;
    public float maxRadius = 10f;
    public float slimeLifetime = 10f;

    public override void ApplyEffect(PlayerMovement player)
    {
        player.StartCoroutine(player.UseSlimeSkill(this));
    }
}
