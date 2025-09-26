using System.Collections;
using UnityEngine;

public class EnemyTroopDefaultState : TroopDefaultState, IReactableForDamage
{
    public EnemyTroopDefaultState(TroopController troopController, TroopScreenCanvasController screenCanvasController, ISwitchableState switcherState) : base(troopController, screenCanvasController, switcherState)
    {

    }

    public void ReactionForTakingDamage<T>(T target) where T : MonoBehaviour, IDamagable
    {

    }

    public override void Start()
    {
        EnableStateIcon();
    }

    public override void Stop()
    {

    }

    private IEnumerator FindPlayerUnits()
    {

        yield return null;
    }
}
