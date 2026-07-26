using UnityEngine;
using UnityEngine.UI;

namespace Assets.App.Scripts.Core.Canvases
{
    public class TroopScreenCanvasController : MonoBehaviour, IInitializableCanvas<TroopScriptable>
    {
        [SerializeField] private StatBarView _healthBar = default;
        [SerializeField] private StatBarView _defenseBar = default;

        [SerializeField] private Image _stateIcon = default;

        private TroopScriptable _troopData = default;

        public virtual void Initialize(TroopScriptable troopData)
        {
            _troopData = troopData;

            _healthBar.Initialize(_troopData.MaxHealPoint);
            _defenseBar.Initialize(_troopData.MaxDefencePoint);
        }

        public void UpdateHealth(int currentHealth)
        {
            _healthBar.UpdateValue(currentHealth, _troopData.MaxHealPoint);
        }

        public void UpdateDefense(int currentDefense)
        {
            _defenseBar.UpdateValue(currentDefense, _troopData.MaxDefencePoint);
        }

        public void ChangeStateIcon(Sprite icon)
        {
            _stateIcon.sprite = icon;
        }

        public virtual void EnableCanvas() => gameObject.SetActive(true);
        public virtual void DisableCanvas() => gameObject.SetActive(false);
    }
}
