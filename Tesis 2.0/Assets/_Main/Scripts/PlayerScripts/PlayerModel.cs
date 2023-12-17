using System;
using _Main.Scripts.Interfaces;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Main.Scripts.PlayerScripts
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(HealthController))]
    public class PlayerModel : MonoBehaviour
    {
        [SerializeField] private PlayerData playerData;
        private PlayerView m_view;

        private Rigidbody2D m_rigidbody;
        private IPlayerController m_playerController;

        private float m_currCriticalDamageMult;
        private int m_currCoins;
        private int m_currXp;

        private float m_dashTimer;
        private float m_fireRateTimer;
        private Camera m_mainCamera;
        private Vector3 m_crossAirPos;
        public HealthController HealthController { get; private set; }
        public StatsController StatsController { get; private set; }
        public Inventory Inventory { get; private set; }

        private readonly Collider2D[] m_itemsCollider = new Collider2D[1];
        

        private void Awake()
        {
            m_rigidbody = GetComponent<Rigidbody2D>();
            m_view = GetComponent<PlayerView>();
            HealthController = GetComponent<HealthController>();
            HealthController.Initialize(playerData.MaxHp);
            HealthController.OnDie += Die;
            m_playerController = GetComponent<IPlayerController>();

            Inventory = new Inventory(this);
        }
        

        private void Start()
        {
            Debug.Log(gameObject.layer);
            InitializeStats();

            m_fireRateTimer = 0f;
            m_dashTimer = 0f;
            m_mainCamera = Camera.main;
            
            SubscribeEventsController();
        }
        private void OnDisable()
        {
            UnsubscribeEventsController();
        }

        private void SubscribeEventsController()
        {
            m_playerController.OnUseItem += OnUseItemHandler;
            m_playerController.OnInteract += OnInteractHandler;
            m_playerController.OnMove += Move;
            m_playerController.OnDash += Dash;
            m_playerController.OnShoot += Shoot;
            m_playerController.OnUpdateCrosshair += UpdateCrosshair;
        }
        private void UnsubscribeEventsController()
        {
            m_playerController.OnUseItem -= OnUseItemHandler;
            m_playerController.OnInteract -= OnInteractHandler;
            m_playerController.OnMove -= Move;
            m_playerController.OnDash -= Dash;
            m_playerController.OnShoot -= Shoot;
            m_playerController.OnUpdateCrosshair -= UpdateCrosshair;
        }
        
        private void InitializeStats()
        {
            StatsController = new StatsController(playerData);
        }
        
        private void OnUseItemHandler()
        {
            Inventory.UseItem();
        }
        
        private void OnInteractHandler()
        {
            var l_size = Physics2D.OverlapCircleNonAlloc(transform.position, playerData.InteractRadius, m_itemsCollider,
                playerData.InteractLayerMask);

            for (var l_i = 0; l_i < l_size; l_i++)
            {
                if (m_itemsCollider[l_i].TryGetComponent(out IInteract l_interact))
                    l_interact.Interact(this);
            }
        }

        private void Move(Vector2 p_dir)
        {
            var l_dir = (Vector3)p_dir;
            var l_newPosition = transform.position + l_dir * (StatsController.GetStatById(StatsId.MovementSpeed) * Time.deltaTime);
            m_rigidbody.MovePosition(l_newPosition);
            
            m_view.UpdateDir(p_dir);
            m_view.SetWalkSpeed((l_newPosition - transform.position).magnitude);
        }

        private void Dash(Vector2 p_dir)
        {
            if (m_dashTimer > Time.time)
                return;
            Debug.Log($"dashh: {p_dir * StatsController.GetStatById(StatsId.DashTranslation)}");
            m_rigidbody.AddForce(p_dir * StatsController.GetStatById(StatsId.DashTranslation), ForceMode2D.Impulse);
            m_dashTimer = StatsController.GetStatById(StatsId.DashCooldown) + Time.time;
        }

        private void Shoot()
        {
            //Check for the rate fire to be > 0f before shooting the next bullet
            if (m_fireRateTimer > Time.time)
                return;

            var l_position = transform.position;
            var l_bull = Instantiate(playerData.Bullet, l_position, playerData.Bullet.transform.rotation);
            l_bull.Initialize(StatsController.GetStatById(StatsId.ProjectileSpeed), StatsController.GetStatById(StatsId.Damage),
                (m_crossAirPos - l_position).normalized, StatsController.GetStatById(StatsId.Range), playerData.TargetLayer);
            m_fireRateTimer = Time.time + StatsController.GetStatById(StatsId.FireRate);
            
            m_view.PlayAttackAnim();
        }

        private void UpdateCrosshair(Vector2 p_pos)
        {
            m_crossAirPos = m_mainCamera.ScreenToWorldPoint(p_pos);
        }

        
        private void Die()
        {
            SceneManager.LoadScene("MainMenuScene");
            m_view.PlayDeadAnim();
            Debug.Log($"YOU DIED");
        }
    }
}
