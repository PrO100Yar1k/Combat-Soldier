using UnityEngine;

public class TroopDefaultState : TroopBaseState
{
    public TroopDefaultState(TroopController troopController, ScreenCanvasController screenCanvasController, ISwitchableState switcherState) : base(troopController, screenCanvasController, switcherState) { }

    public override void Start()
    {
        EnableStateIcon();

        // enable idle animation
    }

    public override void Stop()
    {
        // disable idle animation
    }

    protected override void EnableStateIcon()
    {
        Sprite targetIcon = Resources.Load<Sprite>("State Icons/default_icon");
        _screenCanvasController.ChangeStateIcon(targetIcon);
    }
}
