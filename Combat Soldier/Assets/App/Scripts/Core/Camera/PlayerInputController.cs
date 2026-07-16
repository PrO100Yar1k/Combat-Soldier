using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using Zenject;

public class PlayerInputController : MonoBehaviour
{
    private PlayerSelectionController _selectionController;
    private PlayerCommandController _commandController;

    [Inject]
    public void Construct(PlayerSelectionController selectionController, PlayerCommandController commandController)
    {
        _selectionController = selectionController;
        _commandController = commandController;
    }

    private void Update()
    {
        if (isPointerOverUI())
            return;

        if (Input.GetButtonDown("Fire1"))
        {
            _selectionController.SelectObject();
        }

        if (Input.GetButtonDown("Fire2"))
        {
            _commandController.ExecuteCommand();
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
