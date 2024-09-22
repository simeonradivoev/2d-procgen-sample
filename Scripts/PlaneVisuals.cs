using UnityEngine;

namespace ProcGen2D.Sample
{
    public class PlaneVisuals : MonoBehaviour
    {
        private static readonly int Invincibility = Shader.PropertyToID("_Invincibility");

        private static readonly int Hit = Shader.PropertyToID("_Hit");

        [SerializeField]
        private Renderer _renderer;

        [SerializeField]
        private float _turnSpeed = 5;

        [SerializeField]
        private float _turnAmount;

        private Health _health;

        private PlaneInput _input;

        private MaterialPropertyBlock _propertyBlock;

        private float _rotation;

        private float _turnVelocity;

        private void Start()
        {
            _health = GetComponent<Health>();
            _input = GetComponent<PlaneInput>();
            _propertyBlock = new MaterialPropertyBlock();
        }

        private void Update()
        {
            _renderer.GetPropertyBlock(_propertyBlock);
            _propertyBlock.SetFloat(Invincibility, _health.IsInvincible ? 1 : 0);
            _propertyBlock.SetFloat(Hit, _health.InCooldown ? 1 : 0);
            _renderer.SetPropertyBlock(_propertyBlock);
            _rotation = Mathf.SmoothDampAngle(_rotation, _input.Horizontal * _turnAmount, ref _turnVelocity, _turnSpeed);
            _renderer.transform.localRotation = Quaternion.Euler(0, 0, _rotation);
        }
    }
}