using UnityEngine;

public class TroopModelController
{
    private readonly TroopController _troopController = default;
    private readonly GameObject _troopGameObject = default;

    private readonly MeshRenderer _meshRenderer = default;

    private readonly Material _defaultMaterial = default;
    private readonly LayerMask _defaultLayer = default;

    public TroopModelController(TroopController troopController, GameObject troopGameObject, MeshRenderer meshRenderer)
    {
        _troopController = troopController;
        _troopGameObject = troopGameObject;

        _meshRenderer = meshRenderer;

        _defaultMaterial = _meshRenderer.material; // maybe remake it with a list of materials
        _defaultLayer = _troopGameObject.layer;
    }

    public void AppearTroopModel()
    {
        _meshRenderer.material = _defaultMaterial;
        _troopGameObject.layer = _defaultLayer;
    }

    public void DisappearTroopModel()
    {
        int layerIgnoreRaycast = LayerMask.NameToLayer("Ignore Raycast");

        _troopGameObject.layer = layerIgnoreRaycast;
        _meshRenderer.material = Resources.Load<Material>("Half-Invisible-Material");

        //GameEvents.instance.TroopDisableUI(_troopController); // ?
    }
}
