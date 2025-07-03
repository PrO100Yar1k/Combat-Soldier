using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class BuildingScreenCanvasController : BuildingCanvasController
{
    [SerializeField] private Slider _healPointSlider = default;

    [SerializeField] private TextMeshProUGUI _healPointText = default;

    public override void InitializeCanvas<T>(T buildingController)
    {
        _buildingController = buildingController as BuildingController;
        _buildingScriptable = _buildingController.BuildingScriptable;

        AssignDefaultCanvasValues();
    }

    protected override void AssignDefaultCanvasValues()
    {
        _healPointSlider.maxValue = _buildingScriptable.MaxHealPoint;
        _healPointSlider.value = _healPointSlider.maxValue;
    }

    public void ChangeHealPointSlider(int targetHealPoint)
    {
        targetHealPoint = Mathf.Clamp(targetHealPoint, 0, _buildingScriptable.MaxHealPoint);
        _healPointSlider.value = targetHealPoint;

        _healPointText.text = $"{targetHealPoint}";
    }
}