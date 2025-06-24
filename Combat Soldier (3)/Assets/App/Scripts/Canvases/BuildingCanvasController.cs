using System;
using UnityEngine;

public abstract class BuildingCanvasController : CanvasController
{
    protected BuildingController _buildingController = default;
    protected BuildingScriptable _buildingScriptable = default;
}