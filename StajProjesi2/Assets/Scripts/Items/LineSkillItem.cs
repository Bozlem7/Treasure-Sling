using UnityEngine;

[CreateAssetMenu(menuName = "Item/Directional Wall Skill")]
public class LineSkillItem : Item
{
    public float maxDistance = 20f;
    public GameObject linePrefab;
    public GameObject wallPrefab;

    public override void ApplyEffect(PlayerMovement player)
    {
        player.StartCoroutine(player.UseWallSkill(this));
    }
}