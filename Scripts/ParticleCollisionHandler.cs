using System.Collections.Generic;
using UnityEngine;

namespace ProcGen2D.Sample
{
    [RequireComponent(typeof(ParticleSystem))]
    public class ParticleCollisionHandler : MonoBehaviour
    {
        [SerializeField]
        private int _damage;

        [SerializeField]
        private ParticleSystem _damageFx;

        private List<ParticleCollisionEvent> _collisionEvents;

        private ParticleSystem _particleSystem;

        private PlaneController _planeController;

        private void Start()
        {
            _particleSystem = GetComponent<ParticleSystem>();
            _planeController = GetComponentInParent<PlaneController>();
            _collisionEvents = new List<ParticleCollisionEvent>();
        }

        private void OnParticleCollision(GameObject other)
        {
            var collisionEventCount = _particleSystem.GetCollisionEvents(other, _collisionEvents);

            Vector3? pos = default;
            for (var i = 0; i < collisionEventCount; i++)
            {
                var evn = _collisionEvents[i];
                pos = evn.intersection;
            }

            if (_planeController && pos.HasValue)
            {
                _planeController.OnHitLand.Invoke(pos.Value);
            }

            if (pos.HasValue && _damageFx)
            {
                _damageFx.Emit(new ParticleSystem.EmitParams { position = pos.Value }, 1);
            }

            if (other.TryGetComponent(out Health health))
            {
                health.Damage(_damage, new Health.DamageParams { Position = pos });
                if (_planeController && health.Amount <= 0)
                {
                    _planeController.Score += health.Max;
                }
            }
        }

        private void OnParticleTrigger()
        {
            Debug.Log("Trigger");
        }
    }
}