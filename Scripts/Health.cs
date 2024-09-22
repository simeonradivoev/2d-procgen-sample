using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

namespace ProcGen2D.Sample
{
    public class Health : MonoBehaviour
    {
        [SerializeField]
        private int _health;

        [SerializeField]
        private float _invincibilityDuration;

        [SerializeField]
        private float _startingInvencibility;

        public UnityEvent OnDeath;

        public UnityEvent OnHeal;

        public UnityEvent<DamageParams> OnDamage;

        private float _timeSinceStart;

        public bool IsInvincible => Time.timeSinceLevelLoad - _timeSinceStart < _startingInvencibility;

        public bool InCooldown => Time.timeSinceLevelLoad - LastDamageTime < _invincibilityDuration;

        public float LastDamageTime { get; private set; }

        public float LastHealTime { get; private set; }

        public int Amount { get; set; }

        public int Max => _health;

        private void Start()
        {
            _timeSinceStart = Time.timeSinceLevelLoad;
            LastDamageTime = float.MinValue;
            LastHealTime = float.MinValue;
            Amount = _health;
        }

        public void Heal(int amount)
        {
            Amount = math.min(Amount + amount, Max);
            LastHealTime = Time.timeSinceLevelLoad;
            OnHeal.Invoke();
        }

        public void Damage(int amount, DamageParams @params)
        {
            if (IsInvincible || InCooldown || amount <= 0 || Amount <= 0)
            {
                return;
            }

            LastDamageTime = Time.timeSinceLevelLoad;
            Amount -= amount;
            OnDamage?.Invoke(@params);
            if (Amount <= 0)
            {
                OnDeath.Invoke();
            }
        }

        public struct DamageParams
        {
            public int Amount;

            public Vector3? Position;
        }
    }
}