using UnityEngine;
using Zenject;

public class PlayerSelectionController : MonoBehaviour
{
    [SerializeField] private LayerMask _selectableLayers = default;

    private MonoBehaviour _selectedTarget = default;
    private GameEventBus _gameEventBus = default;

    [Inject]
    public void Construct(GameEventBus gameEvents)
    {
        _gameEventBus = gameEvents;
    }

    public void SelectObject()
    {
        RaycastHit hit = GetRaycastHit();

        if (hit.collider == null)
            return;

        int hitLayer = hit.collider.gameObject.layer;

        if (IsLayerInMask(hitLayer, _selectableLayers) && hit.collider.TryGetComponent(out IDamagable targetUnit))
        {
            if (targetUnit is MonoBehaviour targetMono)
            {
                DeselectAll();

                _selectedTarget = targetMono;
                _gameEventBus.OpenTroopMenu(_selectedTarget);
            }
        }
        else DeselectAll();
    }

    public void DeselectAll()
    {
        _selectedTarget = null;

        _gameEventBus.DeselectController();
        _gameEventBus.DisableActiveCanvas();
    }

    private bool IsLayerInMask(int layer, int mask)
    {
        return ((1 << layer) & mask) != 0;
    }

    private RaycastHit GetRaycastHit()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        return Physics.Raycast(ray, out RaycastHit hit) ? hit : default;
    }
}