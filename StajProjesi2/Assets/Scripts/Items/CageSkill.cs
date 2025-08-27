using UnityEngine;

[CreateAssetMenu(menuName = "Item/Cage Skill")]
public class CageSkill : Item
{
    public GameObject cageObject;
    public GameObject rangeIndicatorPrefab;
    public float throwForce = 15f;
    public float maxRadius = 50f;
    public GameObject miniCageObject;
    //public float slimeLifetime = 10f;

    public override void ApplyEffect(PlayerMovement player)
    {
        player.StartCoroutine(player.UseCageSkill(this));
    }
}
