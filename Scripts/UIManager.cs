using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

namespace ProcGen2D.Sample
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject _player;

        [SerializeField]
        private Slider _shieldSlider;

        [SerializeField]
        private GameObject _damageFlash;

        [SerializeField]
        private GameObject _healFlash;

        [SerializeField]
        private Image _bulletType;

        [SerializeField]
        private Sprite[] _bullets;

        [SerializeField]
        private TMP_Text _score;

        [SerializeField]
        private GameObject _gameOver;

        [SerializeField]
        private TMP_Text _gameOverScore;

        private PlaneController _controller;

        private Health _health;

        private void Start()
        {
            _health = _player.GetComponent<Health>();
            _controller = _player.GetComponent<PlaneController>();
        }

        private void Update()
        {
            ((RectTransform)_shieldSlider.transform).sizeDelta = new Vector2(16 * _health.Max, 16);
            _shieldSlider.maxValue = _health.Max;
            _shieldSlider.value = _health.Amount;
            _damageFlash.SetActive(Time.timeSinceLevelLoad - _health.LastDamageTime < 0.2f);
            _healFlash.SetActive(Time.timeSinceLevelLoad - _health.LastHealTime < 0.3f);
            _bulletType.sprite = _bullets[math.min(_controller.Bullet, _bullets.Length)];
            _score.text = _controller.Score.ToString();
            _gameOverScore.text = $"Score: {_controller.Score}";
            _gameOver.SetActive(_health.Amount <= 0);
            if (_health.Amount <= 0)
            {
                Time.timeScale = 0;
            }
        }
    }
}