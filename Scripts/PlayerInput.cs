using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Screen = UnityEngine.Device.Screen;

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

        private Controls _controls;

        private void Start()
        {
            _controls = new Controls();
            _controls.Enable();
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
            _input.Movement = _controls.Player.Movement.ReadValue<Vector2>();
            var touchMovement = _controls.Player.TouchMovement.ReadValue<Vector2>();
            if (Time.deltaTime > 0)
            {
                touchMovement.x /= Screen.dpi * Time.deltaTime;
                touchMovement.y /= Screen.dpi * Time.deltaTime;
            }

            _input.Movement += touchMovement;
            //touchMovement.x /= Screen.width * 0.5f * Time.deltaTime;
            //touchMovement.y /= Screen.width * 0.5f * Time.deltaTime;
            //_input.Movement += (_controls.Player.TouchPosition.ReadValue<Vector2>() - _controls.Player.TouchStart.ReadValue<Vector2>()).normalized * _controls.Player.Touch.ReadValue<float>();
            _input.Shoot = _controls.Player.Shoot.ReadValue<float>();
        }

        private void OnDestroy()
        {
            _controls.Disable();
            _health.OnDamage.RemoveListener(HealthOnOnDamage);
            _placneController.OnHitLand.RemoveListener(OnHitLand);
        }

        private void OnHitLand(Vector3 arg0)
        {
            StartCoroutine(CameraShake(false, false, Vector3.up));
        }

        private void HealthOnOnDamage(Health.DamageParams obj)
        {
            StartCoroutine(CameraShake(true, true, Vector3.right));
        }

        private IEnumerator CameraShake(bool slowTime, bool haptics, Vector3 direction)
        {
            var startingTime = Time.unscaledTime;
            if (haptics)
            {
                if (Gamepad.current != null)
                {
                    Gamepad.current.SetMotorSpeeds(0.75f,0.75f);
                }
                else
                {
                    Handheld.Vibrate();
                }
            }
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
            if (haptics)
            {
                if (Gamepad.current != null)
                {
                    Gamepad.current.ResetHaptics();
                }
            }
            if (slowTime)
            {
                Time.timeScale = 1;
            }
            _camera.transform.position = _startingCameraPosition;
        }
    }
}