using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalItem : MonoBehaviour
{
    public ObjectSpawner spawner;

    private void Start()
    {
        spawner = FindObjectOfType<ObjectSpawner>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovement player = other.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.AddCoin();
            }

            spawner.ClearCurrentItem();
            Destroy(gameObject);
        }
    }
}
