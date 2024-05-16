using System;
using _Main.Scripts.Bullets;
using _Main.Scripts.DevelopmentUtilities;
using _Main.Scripts.Interfaces;
using _Main.Scripts.Managers;
using _Main.Scripts.Services;
using _Main.Scripts.Services.MicroServices.InventoryService;
using _Main.Scripts.Services.Stats;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Main.Scripts.PlayerScripts
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(HealthController))]
    public class PlayerModel : MonoBehaviour
    {
        public static PlayerModel Local { get; private set; }
        
        [SerializeField] private PlayerData playerData;
        private PlayerView m_view;

        private Rigidbody2D m_rigidbody;
        private IPlayerController m_playerController;

        private float m_currCriticalDamageMult;
        private int m_currCoins;
        private int m_currXp;

        private float m_fireRateTimer;
        private Camera m_mainCamera;
        private Vector3 m_crossAirPos;
        private bool m_isReviveActive;

        public event Action OnRevive;
        
        public HealthController HealthController { get; private set; }
        private static IStatsService StatsController => ServiceLocator.Get<IStatsService>();
        public static IInventoryService InventoryService => ServiceLocator.Get<IInventoryService>();

        private readonly Collider2D[] m_itemsCollider = new Collider2D[1];
	private PoolGeneric<Bullet> m_bulletPool;        
        private Vector2 m_dir;
        public Vector2 CurrDir => m_dir;

        private void Awake()
        {
            m_rigidbody = GetComponent<Rigidbody2D>();
            m_view = GetComponent<PlayerView>();
            HealthController = GetComponent<HealthController>();
            HealthController.Initialize();
            
            HealthController.OnDie += Die;
            HealthController.OnTakeDamage += OnTakeDamageHC;
            m_playerController = GetComponent<IPlayerController>();

            m_bulletPool = new PoolGeneric<Bullet>(playerData.Bullet);

            Local = this;
        }

        private void Start()
        {
            m_fireRateTimer = 0f;
            m_mainCamera = Camera.main;
            
            SubscribeEventsController();
            LevelManager.Instance.SetPlayerModel(this);
        }
        private void OnDisable()
        {
            UnsubscribeEventsController();
        }

        private void OnDestroy()
        {
            Local = default;
        }

        private void SubscribeEventsController()
        {
            m_playerController.OnUseItem += OnUseItemHandler;
            m_playerController.OnInteract += OnInteractHandler;
            m_playerController.OnMove += Move;
            m_playerController.OnShoot += Shoot;
            m_playerController.OnUpdateCrosshair += UpdateCrosshair;
        }
        private void UnsubscribeEventsController()
        {
            m_playerController.OnUseItem -= OnUseItemHandler;
            m_playerController.OnInteract -= OnInteractHandler;
            m_playerController.OnMove -= Move;
            m_playerController.OnShoot -= Shoot;
            m_playerController.OnUpdateCrosshair -= UpdateCrosshair;
        }
        
        private static void OnUseItemHandler()
        {
            InventoryService.UseActiveItem();
        }
        
        private void OnInteractHandler()
        {
            var l_size = Physics2D.OverlapCircleNonAlloc(transform.position, playerData.InteractRadius, m_itemsCollider,
                playerData.InteractLayerMask);

            for (var l_i = 0; l_i < l_size; l_i++)
            {
                if (m_itemsCollider[l_i].TryGetComponent(out IInteract l_interact))
                    l_interact.Interact();
            }
        }

        private void Move(Vector2 p_dir)
        {
            m_dir = p_dir;
            var l_newPosition = transform.position + (Vector3)m_dir * (StatsController.GetStatById(StatsId.MovementSpeed) * Time.deltaTime);
            m_rigidbody.MovePosition(l_newPosition);
            
            m_view.UpdateDir(p_dir);
            m_view.SetWalkSpeed((m_dir * StatsController.GetStatById(StatsId.MovementSpeed)).magnitude);
        }

        private void Shoot()
        {
            //Check for the rate fire to be > 0f before shooting the next bullet
            if (m_fireRateTimer > Time.time)
                return;

            var l_position = transform.position;
            var l_bull = m_bulletPool.GetorCreate();
            l_bull.Initialize(l_position, StatsController.GetStatById(StatsId.ProjectileSpeed), StatsController.GetStatById(StatsId.Damage),
                (m_crossAirPos - l_position).normalized, StatsController.GetStatById(StatsId.Range), playerData.TargetLayer);
            l_bull.OnDeactivate += DeactivateBulletHandler;
            m_fireRateTimer = Time.time + StatsController.GetStatById(StatsId.FireRate);
            
            m_view.PlayAttackAnim();
        }

        private void DeactivateBulletHandler(Bullet p_obj)
        {
            p_obj.OnDeactivate -= DeactivateBulletHandler;
            p_obj.gameObject.SetActive(false);
            m_bulletPool.AddPool(p_obj);
        }

        private void UpdateCrosshair(Vector2 p_position)
        {
            m_crossAirPos = m_mainCamera.ScreenToWorldPoint(p_position);
        }

        private void OnTakeDamageHC(float p_value)
        {
            m_view.PlayHurtAnim();
        }

        public void SetRevive(bool p_isActive) => m_isReviveActive = p_isActive;
        
        private void Die()
        {
            if (m_isReviveActive)
            {
                HealthController.Heal(HealthController.GetMaxHealth() / 2);
                OnRevive?.Invoke();
                return;
            }
            SceneManager.LoadScene("DeathScene");
            m_view.PlayDeadAnim();
        }
    }
}
