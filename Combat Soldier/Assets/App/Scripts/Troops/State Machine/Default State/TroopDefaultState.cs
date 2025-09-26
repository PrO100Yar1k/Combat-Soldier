using UnityEngine;

public abstract class TroopDefaultState : TroopBaseState
{
    public TroopDefaultState(TroopController troopController, TroopScreenCanvasController screenCanvasController, ISwitchableState switcherState) : base(troopController, screenCanvasController, switcherState) 
    {

    }

    protected override void EnableStateIcon()
    {
        Sprite targetIcon = Resources.Load<Sprite>("State Icons/default_icon");
        _screenCanvasController.ChangeStateIcon(targetIcon);
    }
}
