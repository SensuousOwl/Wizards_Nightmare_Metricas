using UnityEngine;

namespace _Main.Scripts.ScriptableObjects.Audio
{
    [CreateAssetMenu(menuName = "PsychoKitties/Audio/BGM")]
    public class LevelBGMAudioData : ScriptableObject
    {
        public AudioClip NoBattleBGM, InBattleBGM, BossBattleBGM;
        public AudioClip NoBattleIntro, InBattleIntro, BossBattleIntro;
    }
}