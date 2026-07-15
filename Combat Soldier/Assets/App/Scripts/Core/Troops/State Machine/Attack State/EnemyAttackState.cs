using Assets.App.Scripts;

public class EnemyAttackState : TroopAttackState
{
    public EnemyAttackState(RepositoryManager repositoryManager, TroopController troopController, TroopScreenCanvasController screenCanvasController, ISwitchableState switcherState, ITroopAnimator animatorController)
        : base(repositoryManager, troopController, screenCanvasController, switcherState, animatorController)
    {
        _enemyTroopSide = Faction.Allies;
    }
}
