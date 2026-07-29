using Assets.App.Scripts;
using Assets.App.Scripts.Core.Canvases;
using Assets.App.Scripts.Infrastructure.Others;

public class PlayerAttackState : TroopAttackState
{
    public PlayerAttackState(TargetSearchService targetSearchService, TroopController troopController, TroopScreenCanvasController screenCanvasController, ISwitchableState switcherState, ITroopAnimator animatorController)
        : base(targetSearchService, troopController, screenCanvasController, switcherState, animatorController)
    {
        _enemyTroopSide = Faction.Allies.GetOpposite();
    }
}
