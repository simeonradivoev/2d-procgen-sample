using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ProcGen2D.Sample
{
    public class PickupSpawner : MonoBehaviour
    {
        [SerializeField]
        private GameObject _healthPickup;

        [SerializeField]
        private GameObject _powerUpPickup;

        [SerializeField]
        private float _healthChance = 0.5f;

        [SerializeField]
        private float _powerUpPickupChance = 0.2f;

        [SerializeField]
        private TilemapGenerator _tilemapGenerator;

        private void Start()
        {
            _tilemapGenerator.OnChunkGenerated += OnChunkGenerated;
        }

        private void OnDestroy()
        {
            _tilemapGenerator.OnChunkGenerated += OnChunkGenerated;
        }

        private void OnChunkGenerated(TilemapChunk obj)
        {
            if (Random.value < _healthChance)
            {
                var health = Instantiate(_healthPickup, Vector3.zero, Quaternion.identity, obj.transform);
                health.transform.localPosition = new float3(
                    Random.Range(4, _tilemapGenerator.ChunkSize.x - 4),
                    Random.Range(0, _tilemapGenerator.ChunkSize.y),
                    0);
            }

            if (Random.value < _powerUpPickupChance)
            {
                var powerUp = Instantiate(_powerUpPickup, Vector3.zero, Quaternion.identity, obj.transform);
                powerUp.transform.localPosition = new float3(
                    Random.Range(4, _tilemapGenerator.ChunkSize.x - 4),
                    Random.Range(0, _tilemapGenerator.ChunkSize.y),
                    0);
            }
        }
    }
}