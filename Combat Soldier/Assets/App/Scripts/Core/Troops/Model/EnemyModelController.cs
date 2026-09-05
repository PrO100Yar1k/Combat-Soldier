using App.Scripts.Core.Troops.Troop_Scripts;
using App.Scripts.Infrastructure.Events;
using App.Scripts.Infrastructure.Interfaces;
using UnityEngine;
using Zenject;

namespace App.Scripts.Core.Troops.Model
{
    public class EnemyModelController : BaseTroopModelController, IVisableModel
    {
        [SerializeField] private Material _transparentBase = default;

        private GameEventBus _gameEventBus = default;
        private Material[] _disappearMaterials = default;

        private int _ignoreRaycastLayer = default;

        public override void Initialize(TroopController troopController)
        {
            base.Initialize(troopController);
            PrepareDisappearMaterials();
        }

        [Inject]
        public void Construct(GameEventBus gameEventBus)
        {
            _gameEventBus = gameEventBus;
        }

        private void PrepareDisappearMaterials()
        {
            _disappearMaterials = new Material[_defaultMaterialsArray.Length];

            for (int i = 0; i < _disappearMaterials.Length; i++)
                _disappearMaterials[i] = _transparentBase;

            _ignoreRaycastLayer = LayerMask.NameToLayer("Ignore Raycast");
        }

        public void AppearTroopModel()
        {
            if (_damagedMaterialCoroutine == null)
                _troopController.UIController.ChangeUnitCircle(true);

            _meshRenderer.sharedMaterials = _defaultMaterialsArray;
            _troopController.gameObject.layer = _defaultLayer;
        }

        public void DisappearTroopModel()
        {
            _troopController.UIController.ChangeUnitCircle(false);
            _troopController.gameObject.layer = _ignoreRaycastLayer;

            if (_disappearMaterials != null)
                _meshRenderer.sharedMaterials = _disappearMaterials;

            _gameEventBus.TroopDisableUI(_troopController);
        }
    }
}