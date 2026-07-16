using Assets.App.Scripts;
using UnityEngine;
using Zenject;

public class EnemyModelController : BaseTroopModelController, IVisableModel
{
    [SerializeField] private Material _transparentBase = default;

    private GameEventBus _gameEventBus = default;
    private Material[] _disappearMaterials = default;

    private const int _ignoreRaycastLayer = 2;

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
        _disappearMaterials = new Material[_defaultMaterials.Length];

        for (int i = 0; i < _disappearMaterials.Length; i++)
            _disappearMaterials[i] = _transparentBase;
    }

    public void AppearTroopModel()
    {
        if (_damagedMaterialCoroutine != null)
            return;

        _troopController.UIController.ChangeUnitCircle(true);

        _meshRenderer.materials = _defaultMaterials;
        _troopController.gameObject.layer = _defaultLayer;
    }

    public void DisappearTroopModel()
    {
        _troopController.UIController.ChangeUnitCircle(false);
        _troopController.gameObject.layer = _ignoreRaycastLayer;

        if (_disappearMaterials != null)
            _meshRenderer.materials = _disappearMaterials;

        _gameEventBus.TroopDisableUI(_troopController);
    }
}