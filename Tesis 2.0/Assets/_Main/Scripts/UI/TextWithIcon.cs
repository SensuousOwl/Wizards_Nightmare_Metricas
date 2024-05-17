using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Main.Scripts.UI
{
    public class TextWithIcon : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private TMP_Text text;

        private int m_currentInt = -1;

        public void SetSprite(Sprite icon)
        {
        
        }
    
        public void SetStat(int value, bool force = false)
        {
            if (m_currentInt == value && !force) return;
            m_currentInt = value;
            text.text = value.ToString();
        }
    }
}
