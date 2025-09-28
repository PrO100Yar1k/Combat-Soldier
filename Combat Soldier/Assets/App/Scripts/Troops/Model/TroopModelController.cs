using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class TroopModelController : MonoBehaviour
{
    [SerializeField] private Material _invisibleMaterial = default;

    private TroopController _troopController = default;
    private GameObject _troopGameObject = default;

    private MeshRenderer _meshRenderer = default;

    private Material _defaultMaterial = default;
    private LayerMask _defaultLayer = default;

    public void InitializeModelController(TroopController troopController, GameObject troopGameObject)
    {
        _troopController = troopController;
        _troopGameObject = troopGameObject;

        _meshRenderer = GetComponent<MeshRenderer>();

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
        _meshRenderer.material = _invisibleMaterial;

        //GameEvents.instance.TroopDisableUI(_troopController); // ?
    }
}
