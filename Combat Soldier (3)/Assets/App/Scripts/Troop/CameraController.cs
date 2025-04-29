using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    [SerializeField] private TroopManager _troopManager = default;

    private void Update()
    {
        // check with new input system

        if (Input.GetButtonDown("Fire1") && !isPointerOverUI())  //IsPointerOverUI() MUST BE ALWAYS ON FALSE
        {
            _troopManager.ChangeTroopControllerAndState();
        }
    }

    private bool isPointerOverUI()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        for (int i = 0; i < results.Count; i++)
            if (results[i].gameObject.layer == 5)
                return true;

        return false;
    }
}
