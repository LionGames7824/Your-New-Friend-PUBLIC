using UnityEngine;
using UnityEngine.UI;


namespace YourNewFriend
{
    public class BossHealthBar : MonoBehaviour
    {
        [SerializeField] private BossHealth _bossHealth;
        [SerializeField] private Slider _slider;

        private void OnEnable()
        {
            _bossHealth.OnHealthChanged += UpdateHealth;
        }

        private void OnDisable()
        {
            _bossHealth.OnHealthChanged -= UpdateHealth;
        }

        private void UpdateHealth(int current, int max)
        {
            _slider.value = (float)current / max;
        }
    }
}