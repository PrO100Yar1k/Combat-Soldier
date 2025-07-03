using UnityEngine;

public abstract class TroopCanvasController : CanvasController
{
    protected TroopController _troopController = default;
    protected TroopScriptable _troopScriptable = default;

    public override void InitializeCanvas<T>(T troopController)
    {
        _troopController = troopController as TroopController;
        _troopScriptable = _troopController.TroopScriptable; // implement interface ICanvasInitializable for Scriptable Object ?

        AssignDefaultCanvasValues();
    }
}