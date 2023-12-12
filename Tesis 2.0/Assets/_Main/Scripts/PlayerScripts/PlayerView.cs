using System;
using UnityEngine;
using UnityEngine.UIElements;
using Slider = UnityEngine.UI.Slider;

namespace _Main.Scripts.PlayerScripts
{
    public class PlayerView : MonoBehaviour
    {
        [SerializeField] private PlayerAnimData animData;
        private Animator m_animator;
        [SerializeField] private SpriteRenderer renderer;
        [SerializeField] private GameObject healthBar;
        [SerializeField] private Slider healthBarFillObj;
        private RectTransform healthBarRectTrans;
        private void Awake()
        {
            m_animator = GetComponentInChildren<Animator>();
        }

        private void Start()
        {
            healthBarRectTrans = healthBar.GetComponent<RectTransform>();
        }

        public void UpdateDir(Vector3 p_dir)
        {
            renderer.flipX = p_dir.x < 0;
        }
        public void PlayIdleAnim()
        {
            m_animator.Play(animData.IdleNameAnim);

        }

        private float test = 100f;
        private float test2 = 100f;
        private float testspeeed = 1.05f;
        private void Update()
        {
            test2 = testspeeed + test;
            UpdateMaxHpBar(test, test + test2 * testspeeed * Time.deltaTime);
            test = test + test2 * testspeeed * Time.deltaTime;
        }

        public void UpdateMaxHpBar(float prevMaxHp, float currMaxHp)
        {
            var diff = (currMaxHp /prevMaxHp);
            //healthBarRectTrans.rect.Set(healthBarRectTrans.rect.x, healthBarRectTrans.rect.y, healthBarRectTrans.rect.width * diff, healthBarRectTrans.rect.height);
            
            // healthBar.transform.localScale = new Vector3(healthBar.transform.localScale.x * diff,1,1);
        }

        public void UpdateHpBar(float currHp, float maxHp)
        {
            var hpPercentaje = (currHp / maxHp);
            healthBarFillObj.value = hpPercentaje;
            
        }
        public void PlayAttackAnim()
        {
            m_animator.Play(animData.AttackNameAnim);
        }

        public void SetWalkSpeed(float p_speed)
        {
            m_animator.SetFloat("Speed", p_speed);
        }
        public void PlayHurtAnim()
        {
            m_animator.Play(animData.HurtNameAnim);
        }

        public void PlayDeadAnim()
        {
            m_animator.Play(animData.DeadNameAnim);
        }
    }
}