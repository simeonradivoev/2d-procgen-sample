using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace ProcGen2D.Sample
{
    [RequireComponent(typeof(PlaneInput))]
    public class PlaneController : MonoBehaviour
    {
        [SerializeField]
        private float _speed;

        [SerializeField]
        private bool2 _boundWithinView;

        [SerializeField]
        private ParticleSystem[] _bullets;

        [SerializeField]
        private float _bulletTypeResetTime;

        public UnityEvent<Vector3> OnHitLand = new();

        public UnityEvent OnShoot;

        public UnityEvent OnRandomBullet;

        public UnityEvent OnDespawn;

        private Camera _camera;

        private Collider2D _collider;

        private Health _health;

        private PlaneInput _input;

        private int _lastNumberOfParticles;

        private float _lastShootTime;

        private float _timeSinceBulletChange;

        public int Bullet { get; private set; }

        public int Score { get; set; }

        private void Start()
        {
            _camera = Camera.main;
            _collider = GetComponentInChildren<Collider2D>();
            _input = GetComponent<PlaneInput>();
            _health = GetComponent<Health>();
            _health.OnDeath.AddListener(LeaveBullets);
            OnDespawn.AddListener(LeaveBullets);
        }

        private void Update()
        {
            var pixelRect = _camera.pixelRect;
            var minCamera = _camera.ScreenToWorldPoint(pixelRect.min);
            var maxCamera = _camera.ScreenToWorldPoint(pixelRect.max);

            var worldWidth = (maxCamera.x - minCamera.x) / 64f; 

            var bounds = new Rect { min = minCamera + _collider.bounds.size, max = maxCamera - _collider.bounds.size };

            var position = (Vector2)transform.position;
            position += (Vector2)transform.TransformDirection(_input.Movement * _speed * worldWidth * Time.deltaTime);
            if (_boundWithinView.x)
            {
                position.x = Mathf.Clamp(position.x, bounds.xMin, bounds.xMax);
            }
            if (_boundWithinView.y)
            {
                position.y = Mathf.Clamp(position.y, bounds.yMin, bounds.yMax);
            }
            transform.position = position;

            var bullet = _bullets[Bullet % _bullets.Length];
            if (_input.Shoot > 0 && bullet)
            {
                if (!bullet.isEmitting)
                {
                    bullet.transform.position = transform.position;
                    bullet.transform.rotation = transform.rotation;
                    bullet.Play();
                }
            }

            if (bullet.particleCount != _lastNumberOfParticles)
            {
                if (bullet.particleCount > _lastNumberOfParticles)
                {
                    OnShoot.Invoke();
                }
                _lastNumberOfParticles = bullet.particleCount;
            }

            if (Time.timeSinceLevelLoad - _timeSinceBulletChange > _bulletTypeResetTime)
            {
                Bullet = 0;
            }
        }

        private void OnDestroy()
        {
            _health.OnDeath.RemoveListener(LeaveBullets);
            OnDespawn.RemoveListener(LeaveBullets);
        }

        public void RandomBullet()
        {
            if (_bullets.Length <= 1)
            {
                return;
            }
            _timeSinceBulletChange = Time.timeSinceLevelLoad;
            Bullet = Random.Range(1, _bullets.Length);
            OnRandomBullet.Invoke();
        }

        private void LeaveBullets()
        {
            foreach (var bullet in _bullets)
            {
                bullet.transform.parent = null;
                var main = bullet.main;
                main.stopAction = ParticleSystemStopAction.Destroy;
            }
        }
    }
}