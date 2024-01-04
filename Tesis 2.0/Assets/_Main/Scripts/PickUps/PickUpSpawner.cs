using System;
using System.Collections.Generic;
using _Main.Scripts.DevelopmentUtilities;
using UnityEngine;

namespace _Main.Scripts.PickUps
{
    public  class PickUpSpawner : MonoBehaviour
    {
        public static PickUpSpawner Instance;
        [SerializeField] private List<PickUp> allPickUps = new List<PickUp>();
        [SerializeField] private List<float> allChances = new List<float>();

        private RouletteWheel<PickUp> m_wheel;

        private void Awake()
        {
            if (Instance != default)
            {
                Destroy(this);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(this);
        }

        private void Start()
        {
            m_wheel = new RouletteWheel<PickUp>();
            m_wheel.SetCachedDictionaryFromLists(allPickUps, allChances);
        }

        public void SpawnPickUp(Vector3 pos, PickUp pickUp)
        {
            Instantiate(pickUp, pos, Quaternion.identity);
        }

        public void SpawnRandomPickUp(Vector3 pos)
        {
            var pickUp = m_wheel.RunWithCached();
            Instantiate(pickUp, pos, Quaternion.identity);
        }
    }
}