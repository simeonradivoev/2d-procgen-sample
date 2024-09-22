using System;
using Unity.Mathematics;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace ProcGen2D.Sample
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField]
        private Entry[] _enemyPrefabs;

        [SerializeField]
        private float2 _spawnCooldown;

        [SerializeField]
        private float _startingGracePeriod;

        private Camera _camera;

        private float _lastSpawnTime;

        private Random _random;

        private void Start()
        {
            _random = new Random((uint)Environment.TickCount);
            _camera = Camera.main;
            _lastSpawnTime = Time.timeSinceLevelLoad + _startingGracePeriod;
        }

        private void Update()
        {
            if (_lastSpawnTime < Time.timeSinceLevelLoad)
            {
                var prefab = _enemyPrefabs[_enemyPrefabs.WeightedRandom(e => e.Weight, ref _random)];
                var spawnCount = _random.NextInt(prefab.SpawnCount.x, prefab.SpawnCount.y);
                for (var i = 0; i < spawnCount; i++)
                {
                    var spawnPos = _camera.ScreenToWorldPoint(
                        new Vector3(
                            _random.NextFloat(Screen.width * prefab.Padding.x, Screen.width - Screen.width * prefab.Padding.y),
                            Screen.height - 32f));
                    var enemy = Instantiate(prefab.Enemy, spawnPos, Quaternion.LookRotation(Vector3.forward, Vector3.down));
                }
                _lastSpawnTime = Time.timeSinceLevelLoad + _random.NextFloat(_spawnCooldown.x, _spawnCooldown.y);
            }
        }

        [Serializable]
        private class Entry
        {
            public GameObject Enemy;

            public float Weight;

            public float2 Padding;

            public int2 SpawnCount;
        }
    }
}