using System.Collections;
using UnityEngine;

namespace _Main.Scripts.DamageFlasher
{
    public class DamageFlash : MonoBehaviour
    {
        [ColorUsage(true, true)]
        [SerializeField] private Color flashColor = Color.white;
        [SerializeField] private float flashTime = 0.25f;

        private SpriteRenderer m_spriteRenderer;
        private Material m_material;

        private Coroutine m_damageFlashCoroutine;

        private void Awake()
        {
            m_spriteRenderer = GetComponentInChildren<SpriteRenderer>();

            Init();
        }

        private void Init()
        {
            m_material = m_spriteRenderer.material;
        }

        private IEnumerator DamageFlasher()
        {
            SetFlashColor();

            float l_currentFlashAmount = 0f;
            float l_elapsedTime = 0f;

            while (l_elapsedTime < flashTime)
            {
                l_elapsedTime += Time.deltaTime;

                l_currentFlashAmount = Mathf.Lerp(1f, 0f, (l_elapsedTime / flashTime));
                SetFlashAmount(l_currentFlashAmount);

                yield return null;
            }
        }

        public void CallDamageFlash()
        {
            StartCoroutine(DamageFlasher());
        }

        private void SetFlashColor()
        {
            m_material.SetColor("_FlashColor", flashColor);
        }

        private void SetFlashAmount(float p_amount)
        {
            m_material.SetFloat("_FlashAmount", p_amount);
        }
    }
}
