using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using Zenject;

public class CameraController : MonoBehaviour
{
    private TroopActionController _troopManager = default;

    [Inject]
    public void Construct(TroopActionController troopManager)
    {
        _troopManager = troopManager;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1") && !isPointerOverUI())
            _troopManager.ChangeTroopControllerAndState();
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
