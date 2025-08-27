using UnityEngine;
using UnityEngine.AI;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject itemPrefab;
    public TurnManager turnManager;
    public int sampleCount = 100;
    public float maxSampleRadius = 30f;

    private GameObject currentItem;

    public void SpawnItemAtFurthestNavmeshPoint()
    {
        if (currentItem != null) return;

        GameObject[] players = turnManager.players;

        Vector3 bestPosition = Vector3.zero;
        float bestDistance = -1f;

        for (int i = 0; i < sampleCount; i++)
        {
            Vector3 randomDirection = Random.insideUnitSphere * maxSampleRadius;
            randomDirection.y = 0;

            Vector3 sampleOrigin = GetCenterPoint(players);
            Vector3 candidate = sampleOrigin + randomDirection;

            if (NavMesh.SamplePosition(candidate, out NavMeshHit hit, 5f, NavMesh.AllAreas))
            {
                Vector3 navPos = hit.position;

                float minDistToAnyPlayer = float.MaxValue;

                foreach (GameObject player in players)
                {
                    float dist = Vector3.Distance(navPos, player.transform.position);
                    if (dist < minDistToAnyPlayer)
                        minDistToAnyPlayer = dist;
                }

                if (minDistToAnyPlayer > bestDistance)
                {
                    bestDistance = minDistToAnyPlayer;
                    bestPosition = navPos;
                }
            }
        }

        currentItem = Instantiate(itemPrefab, bestPosition, Quaternion.identity);

        if (currentItem.TryGetComponent<GoalItem>(out var pickup))
        {
            pickup.spawner = this;
        }
    }

    private Vector3 GetCenterPoint(GameObject[] players)
    {
        Vector3 total = Vector3.zero;
        foreach (var p in players)
            total += p.transform.position;
        return total / players.Length;
    }
    public void ClearCurrentItem()
    {
        currentItem = null;
    }
}