using Assets.App.Scripts;

public class PlayerAttackState : TroopAttackState
{
    public PlayerAttackState(RepositoryManager repositoryManager, TroopController troopController, TroopScreenCanvasController screenCanvasController, ISwitchableState switcherState, ITroopAnimator animatorController)
        : base(repositoryManager, troopController, screenCanvasController, switcherState, animatorController)
    {
        _enemyTroopSide = Faction.Enemies;
    }
}
