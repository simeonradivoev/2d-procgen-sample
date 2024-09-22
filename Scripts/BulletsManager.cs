using System.Collections.Generic;
using UnityEngine;

namespace ProcGen2D.Sample
{
    public class BulletsManager : MonoBehaviour
    {
        [SerializeField]
        private ParticleSystem[] _bulletPrefabs;

        private readonly Dictionary<ParticleSystem, ParticleSystem> _instances = new();

        private void Start()
        {
            foreach (var prefab in _bulletPrefabs)
            {
                _instances.Add(prefab, Instantiate(prefab));
            }
        }

        public bool TryGetBullets(ParticleSystem prefab, out ParticleSystem instance)
        {
            return _instances.TryGetValue(prefab, out instance);
        }
    }
}