using UnityEngine;

public class PlayerTroopDefenseState : TroopDefenseState, IReactableForDamage
{
    public PlayerTroopDefenseState(TroopController troopController, TroopScreenCanvasController screenCanvasController, ISwitchableState switcherState) : base(troopController, screenCanvasController, switcherState)
    {

    }

    public void ReactionForTakingDamage<T>(T target) where T : MonoBehaviour, IDamagable
    {
        Debug.Log("Your Unit Was Damaged");
    }
}
