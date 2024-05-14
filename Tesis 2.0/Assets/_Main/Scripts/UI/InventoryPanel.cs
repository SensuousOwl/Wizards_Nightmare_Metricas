using System.Collections;
using System.Collections.Generic;
using _Main.Scripts.UI.Menus;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPanel : BasePanel
{
    [SerializeField] private BasePanel inventoryScreen;
    [SerializeField] private Button goBackInventoryButton;
    [SerializeField] private TMP_Text nameActiveText;
    [SerializeField] private TMP_Text namePassiveText;
    [SerializeField] private TMP_Text Text;

    public void Initialize()
    {
        inventoryScreen.Close();
        goBackInventoryButton.onClick.AddListener(CloseInventoryPanel);
        
    }
    private void CloseInventoryPanel()
    {
        inventoryScreen.Close();
    }
}
