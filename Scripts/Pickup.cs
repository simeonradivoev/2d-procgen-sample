using UnityEngine;

namespace ProcGen2D.Sample
{
    public class Pickup : MonoBehaviour
    {
        [SerializeField]
        private int _health;

        [SerializeField]
        private bool _randomBullets;

        private void OnTriggerEnter2D(Collider2D other)
        {
            var destroy = false;

            if (other.attachedRigidbody)
            {
                if (other.attachedRigidbody.TryGetComponent(out Health health) && health.Amount < health.Max)
                {
                    health.Heal(_health);
                    destroy = true;
                }

                if (_randomBullets && other.attachedRigidbody.TryGetComponent(out PlaneController planeController))
                {
                    planeController.RandomBullet();
                    destroy = true;
                }
            }

            if (destroy)
            {
                Destroy(gameObject);
            }
        }
    }
}