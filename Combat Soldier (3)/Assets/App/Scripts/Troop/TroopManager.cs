using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TroopManager : MonoBehaviour
{
    [Header("Troop INFO")] [Space(3)]

    [SerializeField] private TroopController _currentTroopController = default;

    [Header("Layers")] [Space(3)]

    [SerializeField] private LayerMask _terrainLayer = default;

    [SerializeField] private LayerMask _troopsLayer = default;

    private TroopStateController _troopStateController;

    private RaycastHit hit;

    #region Events
    private void OnEnable()
    {
        // to do
    }

    private void OnDisable()
    {

    }
    #endregion

    private void Start()
    {
        _troopStateController = _currentTroopController.StateController;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1")) {
            RaycastToScreenPoint();
        }
    }

    private void RaycastToScreenPoint()
    {
        Vector3 mousePos = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider != null)
            {
                LayerMask hitObjectLayer = hit.collider.gameObject.layer;
                int shiftedMask = (1 << hitObjectLayer);

                if ((shiftedMask & _terrainLayer.value) != 0) {
                    _troopStateController.ActivateMoveState();
                    GameEvents.instance.TroopMovement(hit.point, 5); // to change
                }
                else if ((shiftedMask & _troopsLayer.value) != 0) { // ?
                    _currentTroopController = hit.collider.GetComponent<TroopController>();
                    _currentTroopController.UIController.OpenMainMenu();
                }
            }
        }
    }
}

public enum TroopType
{
    Soldier_Type_1,
    Soldier_Type_2,
    AntiTank_Soldier
}

public enum AttackType
{
    Land,
    Air,
    Both
}