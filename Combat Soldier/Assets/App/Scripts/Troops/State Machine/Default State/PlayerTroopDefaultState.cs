using UnityEngine;

public class PlayerTroopDefaultState : TroopDefaultState, IReactableForDamage
{
    public PlayerTroopDefaultState(TroopController troopController, TroopScreenCanvasController screenCanvasController, ISwitchableState switcherState) : base(troopController, screenCanvasController, switcherState)
    {

    }

    public void ReactionForTakingDamage<T>(T target) where T : MonoBehaviour, IDamagable
    {

    }

    public override void Start()
    {
        EnableStateIcon();

        // enable idle animation
    }

    public override void Stop()
    {
        // disable idle animation
    }

}
