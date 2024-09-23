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

        private Camera _camera;

        private void Start()
        {
            _camera = Camera.main;
            _tilemapGenerator.OnChunkGenerated += OnChunkGenerated;
        }

        private void OnDestroy()
        {
            _tilemapGenerator.OnChunkGenerated += OnChunkGenerated;
        }

        private Vector2 RandomPoint(int2 chunk)
        {
            var cameraRect = _camera.pixelRect;
            var worldMin = _camera.ScreenToWorldPoint(cameraRect.min);
            var worldMax = _camera.ScreenToWorldPoint(cameraRect.max);
            
            var x = Random.Range(worldMin.x, worldMax.x);
            var y = _tilemapGenerator.ChunkToWorld(Random.Range(0, _tilemapGenerator.ChunkSize.y)).y;

            return new Vector2(x, y);
        }

        private void OnChunkGenerated(TilemapChunk obj)
        {
            if (Random.value < _healthChance)
            {
                var health = Instantiate(_healthPickup, Vector3.zero, Quaternion.identity, obj.Objects);
                health.transform.localPosition = RandomPoint(obj.Chunk);
            }

            if (Random.value < _powerUpPickupChance)
            {
                var powerUp = Instantiate(_powerUpPickup, Vector3.zero, Quaternion.identity, obj.Objects);
                powerUp.transform.localPosition = RandomPoint(obj.Chunk);
            }
        }
    }
}