using System.Collections;
using System.Collections.Generic;
using _Main.Scripts.UI.Menus;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPanel : BasePanel
{
    [SerializeField] private BasePanel inventoryScreen;
    [SerializeField] private Button goBackInventoryButton;

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
