using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class TroopModelController : MonoBehaviour
{
    [SerializeField] private Material _invisibleMaterial = default;

    private Material _defaultMaterial = default;

    private MeshRenderer _meshRenderer = default;

    public void InitializeModelController()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _defaultMaterial = _meshRenderer.material; // maybe remake it with a list of materials
    }

    public void AppearTroopModel()
        => _meshRenderer.material = _defaultMaterial;

    public void DisappearTroopModel()
        => _meshRenderer.material = _invisibleMaterial;
}
