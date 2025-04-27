using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class TroopModelController : MonoBehaviour
{
    [SerializeField] private Material _invisibleMaterial = default;

    private Material[] _defaultMaterialList = default; // to do

    #region Events

    private void SubscribeToEvents()
    {

    }

    private void UnSubscribeFromEvents()
    {

    }

    #endregion

    public void InitializeModelController()
    {
        _defaultMaterialList = GetComponent<MeshRenderer>().materials;

        // to do
    }
}
