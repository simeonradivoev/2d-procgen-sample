﻿using Unity.Mathematics;
using UnityEngine;

namespace ProcGen2D.Sample
{
    [RequireComponent(typeof(PlaneInput))]
    public class EnemyController : MonoBehaviour
    {
        [SerializeField]
        private ParticleSystem _deathFx;

        [SerializeField]
        private float _horizontalSine;

        [SerializeField]
        private float _horizontal;

        [SerializeField]
        private float _offScreenDestroyCooldown = 1;

        private Camera _camera;

        private Health _health;

        private PlaneInput _input;

        private float _timeOffscreen;

        private void Start()
        {
            _camera = Camera.main;
            _input = GetComponent<PlaneInput>();
            _health = GetComponent<Health>();

            _health.OnDeath.AddListener(OnDeath);
        }

        private void Update()
        {
            _input.Vertical = 1;
            _input.Horizontal = Mathf.Sin(Time.timeSinceLevelLoad) * _horizontalSine + _horizontal;
            _input.Shoot = 1;

            var viewportPos = _camera.WorldToViewportPoint(transform.position);
            var outsideView = viewportPos.x < 0 || viewportPos.x > 1 || viewportPos.y < 0 || viewportPos.y > 1;
            if (outsideView)
            {
                _timeOffscreen += Time.deltaTime;
                if (_timeOffscreen > _offScreenDestroyCooldown)
                {
                    Destroy(gameObject);
                }
            }
        }

        private void OnDestroy()
        {
            _health.OnDeath.RemoveListener(OnDeath);
        }

        private void OnDeath()
        {
            if (_deathFx)
            {
                InstantiateAsync(_deathFx, transform.position, quaternion.identity);
            }

            Destroy(gameObject);
        }
    }
}