using Assets.App.Scripts;
using Assets.App.Scripts.Core.Canvases;

public abstract class TroopDefaultState : TroopBaseState
{
    protected override string StateIconLocation
        => "State Icons/Default-State-Icon";

    protected TroopDefaultState(TargetSearchService targetSearchService, TroopController troopController, TroopScreenCanvasController screenCanvasController, ISwitchableState switcherState, ITroopAnimator animatorController)
        : base(targetSearchService, troopController, screenCanvasController, switcherState, animatorController)
    {

    }

    protected override void PlayStateAnimation()
    {
        _animatorController.PlayIdle();
    }
}
