using UnityEngine;

public class EnemyTroopDefenseState : TroopDefenseState, IReactableForDamage
{
    public EnemyTroopDefenseState(TroopController troopController, TroopScreenCanvasController screenCanvasController, ISwitchableState switcherState) : base(troopController, screenCanvasController, switcherState)
    {

    }

    public void ReactionForTakingDamage<T>(T target) where T : MonoBehaviour, IDamagable
    {
        //Vector3 targetPosition = target.transform.position;


    }
}

