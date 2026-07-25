using Assets.App.Scripts;
using Assets.App.Scripts.Core.Canvases;

public class EnemyAttackState : TroopAttackState
{
    public EnemyAttackState(TargetSearchService targetSearchService, TroopController troopController, TroopScreenCanvasController screenCanvasController, ISwitchableState switcherState, ITroopAnimator animatorController)
        : base(targetSearchService, troopController, screenCanvasController, switcherState, animatorController)
    {
        _enemyTroopSide = Faction.Allies;
    }
}
