using DefaultNamespace;
using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

namespace ProcGen2D.Sample
{
    public class WorldController : MonoBehaviour
    {
        [SerializeField]
        private float _speed;

        [SerializeField]
        private Transform _world;

        [SerializeField]
        private TilemapGenerator _tilemapGenerator;

        [SerializeField]
        private VerticalBiomeSource _biomeSource;

        [FormerlySerializedAs("_chinkDistance")]
        [SerializeField]
        private int2 _chunkDistance;

        [SerializeField]
        private int _pixelSize;

        private Vector3 _position;

        private void Start()
        {
            _biomeSource.Initialize();
            _tilemapGenerator.Seed = (uint)Environment.TickCount;
            _tilemapGenerator.Initialize();

            for (var y = -_chunkDistance.x; y <= _chunkDistance.y; y++)
            {
                var pos = new int2(0, y);
                _tilemapGenerator.Generate(_biomeSource, pos);
            }
        }

        private void Update()
        {
            _position += Vector3.down * _speed * Time.deltaTime;
            _world.transform.localPosition = new Vector3(
                Mathf.RoundToInt(_position.x * _pixelSize) / (float)_pixelSize,
                Mathf.RoundToInt(_position.y * _pixelSize) / (float)_pixelSize);

            var currentChunk =
                _tilemapGenerator.WorldToChunkIndex(new float2(_world.transform.localPosition.x, math.abs(_world.transform.localPosition.y)));
            var min = currentChunk.y - _chunkDistance.x;
            var max = currentChunk.y + _chunkDistance.y;

            for (var y = min; y <= max; y++)
            {
                var pos = new int2(0, y);
                if (_tilemapGenerator.GeneratingChunks.Count <= 0 && !_tilemapGenerator.Chunks.ContainsKey(pos))
                {
                    StartCoroutine(_tilemapGenerator.GenerateAsync(_biomeSource, pos));
                }
            }

            foreach (var chunk in _tilemapGenerator.Chunks)
            {
                if (chunk.Key.y < min && _tilemapGenerator.DisposingChunks.Count <= 0)
                {
                    StartCoroutine(_tilemapGenerator.DisposeAsync(chunk.Key));
                    break;
                }
            }
        }
    }
}