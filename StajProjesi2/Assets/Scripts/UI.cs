using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TurnManager turnManager;

    void Update()
    {
        if (turnManager == null || turnManager.players == null)
            return;

        string display = "";

        for (int i = 0; i < turnManager.players.Length; i++)
        {
            var player = turnManager.players[i];
            var movement = player.GetComponent<PlayerMovement>();

            display += $"Player {i + 1}: {movement.coinsCollected}\n";
        }

        scoreText.text = display;
    }
}