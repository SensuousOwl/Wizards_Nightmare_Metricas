namespace PlayerScripts
{
    public struct StatsData
    {
        public float CurrMovementSpeed { get; set; }
        public float CurrEnergy { get; set; }
        public float CurrFireRate { get; set; }
        public float CurrRange { get; set; }
        public float CurrCriticalChance { get; set; }
        public float CurrDashCooldown { get; set; }
        public float CurrDashTrans { get; set; }
        public float CurrProjectileSpeed { get; set; }
        public int CurrDamage { get; set; }

        public StatsData(PlayerData p_playerData)
        {
            CurrMovementSpeed = p_playerData.MovementSpeed;
            CurrEnergy = p_playerData.Energy;
            CurrFireRate = p_playerData.FireRate;
            CurrRange = p_playerData.Range;
            CurrCriticalChance = p_playerData.CriticalChance;
            CurrDashCooldown = p_playerData.DashCooldown;
            CurrDashTrans = p_playerData.DashTranslation;
            CurrProjectileSpeed = p_playerData.ProjectileSpeed;
            CurrDamage = p_playerData.Damage;
        }
    }
}