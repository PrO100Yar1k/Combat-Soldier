using Assets.App.Scripts;

public class EnemyDeathState : TroopDeathState
{
    public EnemyDeathState(TargetSearchService targetSearchService, TroopController troopController, TroopScreenCanvasController screenCanvasController, ISwitchableState switcherState, ITroopAnimator animatorController)
        : base(targetSearchService, troopController, screenCanvasController, switcherState, animatorController)
    {

    }
}
