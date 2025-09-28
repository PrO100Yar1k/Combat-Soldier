using System.Collections;
using UnityEngine;

public class EnemyDefaultState : TroopDefaultState
{
    public EnemyDefaultState(TroopController troopController, TroopScreenCanvasController screenCanvasController, ISwitchableState switcherState) : base(troopController, screenCanvasController, switcherState)
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
