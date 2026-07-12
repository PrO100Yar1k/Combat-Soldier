using UnityEngine;

public class TroopModelController
{
    private readonly TroopController _troopController;
    private readonly GameObject _troopGameObject;
    private readonly MeshRenderer _meshRenderer;
    private readonly GameEventBus _gameEventBus;

    private readonly Material[] _defaultMaterials;
    private readonly LayerMask _defaultLayer;

    private Material[] _disappearMaterials;

    public TroopModelController(TroopController troopController, GameObject troopGameObject, MeshRenderer meshRenderer, GameEventBus gameEventBus)
    {
        _troopController = troopController;
        _troopGameObject = troopGameObject;
        _meshRenderer = meshRenderer;
        _gameEventBus = gameEventBus;

        _defaultMaterials = _meshRenderer.sharedMaterials;
        _defaultLayer = _troopGameObject.layer;

        PrepareDisappearMaterials();
    }

    private void PrepareDisappearMaterials()
    {
        Material transparentBase = Resources.Load<Material>("Half-Invisible-Material");

        if (transparentBase == null)
            return;

        _disappearMaterials = new Material[_defaultMaterials.Length];

        for (int i = 0; i < _disappearMaterials.Length; i++)
            _disappearMaterials[i] = transparentBase;
    }

    public void AppearTroopModel()
    {
        _meshRenderer.materials = _defaultMaterials;
        _troopGameObject.layer = _defaultLayer;
    }

    public void DisappearTroopModel()
    {
        int layerIgnoreRaycast = LayerMask.NameToLayer("Ignore Raycast");
        _troopGameObject.layer = layerIgnoreRaycast;

        if (_disappearMaterials != null)
            _meshRenderer.materials = _disappearMaterials;

        _gameEventBus.TroopDisableUI(_troopController);
    }
}