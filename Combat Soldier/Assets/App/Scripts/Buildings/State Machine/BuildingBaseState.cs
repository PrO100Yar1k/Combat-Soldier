
public abstract class BuildingBaseState : System.IDisposable
{
    protected BuildingScreenCanvasController _screenCanvasController = default;

    protected BuildingController _buildingController = default;

    protected ISwitchableBuildingState _switcherState = default;

    protected BuildingScriptable _troopScriptable = default;

    public BuildingBaseState(BuildingController troopController, BuildingScreenCanvasController screenCanvasController, ISwitchableBuildingState switcherState)
    {
        _buildingController = troopController;
        _switcherState = switcherState;

        _screenCanvasController = screenCanvasController;
        _troopScriptable = troopController.BuildingScriptable;
    }

    public void Dispose()
        => UnSubscribeFromEvents();

    public abstract void Start();

    public abstract void Stop();

    protected virtual void SubscribeToEvents() { } //

    protected virtual void UnSubscribeFromEvents() { } //
}
