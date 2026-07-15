using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class TroopModelController : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer _meshRenderer = default;
    [SerializeField] private Material _transparentBase = default;

    [SerializeField] private List<Material> _damagedUnitMaterial = default;

    [SerializeField] private LayerMask _ignoreRaycastMask = default;

    private EnemyTroopController _troopController = default;
    private GameEventBus _gameEventBus = default;

    private Material[] _defaultMaterials = default;
    private Material[] _disappearMaterials = default;

    private LayerMask _defaultLayer = default;

    private Coroutine _damagedMaterialCoroutine = default;

    public void Initialize(EnemyTroopController troopController)
    {
        _troopController = troopController;

        _defaultMaterials = _meshRenderer.sharedMaterials;
        _defaultLayer = _troopController.gameObject.layer;

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
        _troopController.gameObject.layer = _ignoreRaycastMask;

        if (_disappearMaterials != null)
            _meshRenderer.materials = _disappearMaterials;

        _gameEventBus.TroopDisableUI(_troopController);
    }

    public void ChangeMaterialToDamaged()
    {
        _damagedMaterialCoroutine = _troopController.StartCoroutine(ChangeMaterialToDamagedCoroutine());
    }

    private IEnumerator ChangeMaterialToDamagedCoroutine()
    {
        _meshRenderer.materials = _damagedUnitMaterial.ToArray();

        yield return new WaitForSeconds(0.25f);

        _meshRenderer.materials = _defaultMaterials;
    }
}