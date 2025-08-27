using UnityEngine;


public class TurnManager : MonoBehaviour
{
    public GameObject[] players;
    private int currentPlayerIndex = 0;
    private PlayerMovement[] controllers;
    public ObjectSpawner spawner;
    public InventoryUI inventoryUI;
    public PlayerController[] playerControllers;


    void Start()
    {
        for (int i = 0; i < playerControllers.Length; i++)
        {
            playerControllers[i].myPlayer.playerID = i;
        }
        controllers = new PlayerMovement[players.Length];

        for (int i = 0; i < players.Length; i++)
        {
            controllers[i] = players[i].GetComponent<PlayerMovement>();
            controllers[i].enabled = false;
        }

        StartTurn();
    }

    public void EndTurn()
    {
        controllers[currentPlayerIndex].enabled = false;

        currentPlayerIndex = (currentPlayerIndex + 1) % players.Length;
        StartTurn();

         


       
    }

    private void StartTurn()
    {
        spawner.SpawnItemAtFurthestNavmeshPoint();
        GameObject currentPlayer = players[currentPlayerIndex];
        inventoryUI.Show(currentPlayer.GetComponent<PlayerMovement>(), OnInventoryClosed);
    }

    private void OnInventoryClosed()
    {
        controllers[currentPlayerIndex].enabled = true;
    }

    public int GetCurrentPlayerIndex()
    {
        return currentPlayerIndex;
    }
}
