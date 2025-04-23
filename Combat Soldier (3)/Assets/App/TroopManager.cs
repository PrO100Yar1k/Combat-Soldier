using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TroopManager : MonoBehaviour
{
    [Header("Troop INFO")] [Space(3)]

    [SerializeField] private TroopController _currentTroopController = default;

    [Header("Other Components")] [Space(3)]

    [SerializeField] private GameObject Terrain = default; // to do

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
            Fire();
        }
    }

    private void Fire()
    {
        Vector3 mousePos = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        if (Physics.Raycast(ray, out hit)) // do by layer 
        {
            if (hit.collider != null) // to do
            {
                if (hit.collider.gameObject == Terrain) 
                {
                    _troopStateController.ActivateMoveState();
                    GameEvents.instance.TroopMovement(hit.point, 5); // to do
                }

                //else if (hit.collider.gameObject == currentDivision.gameObject) currentDivision.OpenMainMenu();

                //else currentDivision.Attack();
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