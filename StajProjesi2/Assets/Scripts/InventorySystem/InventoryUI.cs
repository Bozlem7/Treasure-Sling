using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public GameObject panel;
    public List<Button> slotButtons;   // Slot UI'larý sýrayla atanmalý
    public Button skipButton;

    private Action onClosed;

    public void Show(PlayerMovement player, Action onClosedCallback)
    {
        panel.SetActive(true);
        onClosed = onClosedCallback;

        InventorySystem inventory = player.GetComponent<InventorySystem>();

        for (int i = 0; i < slotButtons.Count; i++)
        {
            int index = i;
            var button = slotButtons[i];
            var text = button.GetComponentInChildren<TMP_Text>();

            if (i < inventory.items.Count)
            {
                var item = inventory.items[i];
                button.interactable = true;
                text.text = item.itemName;

                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() =>
                {
                    inventory.UseItem(index, player);
                    Hide();
                    onClosed?.Invoke();
                });
            }
            else
            {
                button.interactable = false;
                text.text = "-";
            }
        }

        skipButton.onClick.RemoveAllListeners();
        skipButton.onClick.AddListener(() =>
        {
            Hide();
            onClosed?.Invoke();
        });
    }

    public void Hide()
    {
        panel.SetActive(false);
    }
}
