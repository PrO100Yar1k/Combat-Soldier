using Assets.App.Scripts;

public class EnemyDefenseState : TroopDefenseState
{
    public EnemyDefenseState(TargetSearchService targetSearchService, TroopController troopController, TroopScreenCanvasController screenCanvasController, ISwitchableState switcherState, ITroopAnimator animatorController)
        : base(targetSearchService, troopController, screenCanvasController, switcherState, animatorController)
    {

    }
}

