using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextWithIcon : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text text;

    private int currentInt = -1;

    public void SetSprite(Sprite icon)
    {
        
    }
    
    public void SetStat(int value, bool force = false)
    {
        if (currentInt == value && !force) return;
        currentInt = value;
        text.text = value.ToString();
    }
}
