using System.Collections;
using UnityEngine;

namespace ProcGen2D.Sample
{
    [RequireComponent(typeof(PlaneInput))]
    public class PlayerInput : MonoBehaviour
    {
        private Camera _camera;

        private Health _health;

        private PlaneInput _input;

        private PlaneController _placneController;

        private Vector3 _startingCameraPosition;

        private void Start()
        {
            _camera = Camera.main;
            _startingCameraPosition = _camera.transform.position;
            _input = GetComponent<PlaneInput>();
            _health = GetComponent<Health>();
            _placneController = GetComponent<PlaneController>();
            _health.OnDamage.AddListener(HealthOnOnDamage);
            _placneController.OnHitLand.AddListener(OnHitLand);
        }

        private void Update()
        {
            _input.Horizontal = Input.GetAxis("Horizontal");
            _input.Vertical = Input.GetAxis("Vertical");
            _input.Shoot = Input.GetAxis("Fire1");
        }

        private void OnDestroy()
        {
            _health.OnDamage.RemoveListener(HealthOnOnDamage);
            _placneController.OnHitLand.RemoveListener(OnHitLand);
        }

        private void OnHitLand(Vector3 arg0)
        {
            StartCoroutine(CameraShake(false, Vector3.up));
        }

        private void HealthOnOnDamage(Health.DamageParams obj)
        {
            StartCoroutine(CameraShake(true, Vector3.right));
        }

        private IEnumerator CameraShake(bool slowTime, Vector3 direction)
        {
            var startingTime = Time.unscaledTime;
            if (slowTime)
            {
                Time.timeScale = 0.5f;
            }
            while (Time.unscaledTime - startingTime < 0.2f)
            {
                var time = (Time.timeSinceLevelLoad - startingTime) / 0.2f;
                _camera.transform.position += direction * Mathf.Sin(time * Mathf.PI * 2) * 0.1f;
                yield return new WaitForEndOfFrame();
            }
            if (slowTime)
            {
                Time.timeScale = 1;
            }
            _camera.transform.position = _startingCameraPosition;
        }
    }
}