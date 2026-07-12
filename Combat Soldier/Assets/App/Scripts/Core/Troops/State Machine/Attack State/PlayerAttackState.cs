using UnityEngine;

public class PlayerAttackState : TroopAttackState
{
    public PlayerAttackState(RepositoryManager repositoryManager, TroopController troopController, TroopScreenCanvasController screenCanvasController, ISwitchableState switcherState)
        : base(repositoryManager, troopController, screenCanvasController, switcherState)
    {
        _enemyTroopSide = Faction.Enemies;
    }
}
