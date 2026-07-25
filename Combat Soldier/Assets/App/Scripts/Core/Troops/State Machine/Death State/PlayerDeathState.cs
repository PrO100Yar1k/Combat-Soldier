using Assets.App.Scripts;
using Assets.App.Scripts.Core.Canvases;

public class PlayerDeathState : TroopDeathState
{
    public PlayerDeathState(TargetSearchService targetSearchService, TroopController troopController, TroopScreenCanvasController screenCanvasController, ISwitchableState switcherState, ITroopAnimator animatorController)
        : base(targetSearchService, troopController, screenCanvasController, switcherState, animatorController)
    {

    }
}
