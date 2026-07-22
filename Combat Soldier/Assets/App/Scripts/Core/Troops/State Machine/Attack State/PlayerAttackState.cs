using Assets.App.Scripts;

public class PlayerAttackState : TroopAttackState
{
    public PlayerAttackState(TargetSearchService targetSearchService, TroopController troopController, TroopScreenCanvasController screenCanvasController, ISwitchableState switcherState, ITroopAnimator animatorController)
        : base(targetSearchService, troopController, screenCanvasController, switcherState, animatorController)
    {
        _enemyTroopSide = Faction.Enemies; //extension methods
    }
}
