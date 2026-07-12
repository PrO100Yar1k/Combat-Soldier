using UnityEngine;

public class EnemyAttackState : TroopAttackState
{
    public EnemyAttackState(RepositoryManager repositoryManager, TroopController troopController, TroopScreenCanvasController screenCanvasController, ISwitchableState switcherState)
        : base(repositoryManager, troopController, screenCanvasController, switcherState)
    {
        _enemyTroopSide = Faction.Allies;
    }
}
