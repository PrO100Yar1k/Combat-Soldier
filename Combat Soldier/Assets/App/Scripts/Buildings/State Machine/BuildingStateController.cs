using System.Collections.Generic;
using System.Linq;
using System;

public class BuildingStateController : ISwitchableBuildingState, IDisposable
{
    private readonly BuildingController _buildingController = default;

    private readonly BuildingDefaultState _troopDefaultState = default;
    private readonly BuildingAttackState _troopAttackState = default;
    private readonly BuildingDeathState _troopDeathState = default;

    private readonly List<BuildingBaseState> _allStates = default;

    private BuildingBaseState _currentState = default;

    public BuildingStateController(BuildingController buildingController, BuildingScreenCanvasController screenCanvasController)
    {
        _buildingController = buildingController;

        _troopDefaultState = new BuildingDefaultState(_buildingController, screenCanvasController, this);
        _troopAttackState = new BuildingAttackState(_buildingController, screenCanvasController, this);

        _troopDeathState = new BuildingDeathState(_buildingController, screenCanvasController, this);

        _allStates = new List<BuildingBaseState>() { _troopDefaultState, _troopAttackState, _troopDeathState };
        _currentState = _allStates[0];

        ActivateDefaultState();
    }

    public void Dispose()
    {
        foreach (IDisposable disposableState in _allStates)
            disposableState.Dispose();
    }

    public void ActivateDefaultState()
    {
        SwitchState<BuildingDefaultState>();
    }

    public void ActivateAttackState(IDamagable enemy)
    {
        SwitchState<BuildingAttackState>();

        _troopAttackState.ActivateAttack(enemy);
    }

    public void ActivateDeathState()
    {
        SwitchState<BuildingDeathState>();
    }

    public bool CheckStateForActivity<State>() where State : BuildingBaseState
    {
        BuildingBaseState state = _allStates.FirstOrDefault(s => s is State);

        return _currentState == state;
    }

    public void SwitchState<State>() where State : BuildingBaseState
    {
        BuildingBaseState state = _allStates.FirstOrDefault(s => s is State);

        _currentState.Stop();
        _currentState = state;
        _currentState.Start();
    }
}
public interface ISwitchableBuildingState
{
    public void SwitchState<T>() where T : BuildingBaseState;
}

