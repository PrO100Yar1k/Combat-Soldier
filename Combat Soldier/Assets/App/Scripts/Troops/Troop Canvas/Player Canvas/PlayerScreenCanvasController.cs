using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScreenCanvasController : TroopScreenCanvasController
{
    [Space(3)]

    [SerializeField] private Slider _reloadingSlider = default;

    [Space(3)]

    [SerializeField] private Button _attackButton = default;
    [SerializeField] private Button _moveButton = default;

    [SerializeField] private Button _cancelButton = default;

    private Coroutine _reloadBarCoroutine = default;

    #region Events

    private void OnEnable()
    {
        GameEvents.instance.OnReloadingTroop += UpdateReloadingBar;
    }

    private void OnDisable()
    {
        GameEvents.instance.OnReloadingTroop -= UpdateReloadingBar;
    }

    #endregion

    protected override void AssignDefaultCanvasValues()
    {
        base.AssignDefaultCanvasValues();

        _attackButton.onClick.AddListener(delegate { AddEventOnActionButtons(OrderMode.Attack); });
        _moveButton.onClick.AddListener(delegate { AddEventOnActionButtons(OrderMode.Move); });

        _cancelButton.onClick.AddListener(AddEventOnCancelButton);
    }

    private void AddEventOnActionButtons(OrderMode orderMode)
    {
        GameEvents.instance.TroopEnterAnyMode(_troopController, orderMode);
        _cancelButton.gameObject.SetActive(true);
    }

    private void AddEventOnCancelButton()
    {
        GameEvents.instance.TroopCancelEnteringMode();
        _cancelButton.gameObject.SetActive(false);
    }

    private IEnumerator UpdateReloadingSlider(float timeToReload)
    {
        float timeToCompleteReload = timeToReload;

        _reloadingSlider.maxValue = timeToCompleteReload;

        while (timeToReload > 0)
        {
            timeToReload -= Time.deltaTime;

            _reloadingSlider.value = timeToCompleteReload - timeToReload;

            yield return new WaitForEndOfFrame();
        }
    }

    private void UpdateReloadingBar(float timeToReload)
    {
        if (_reloadBarCoroutine != null)
        {
            StopCoroutine(_reloadBarCoroutine);
            _reloadBarCoroutine = null;
        }

        _reloadBarCoroutine = StartCoroutine(UpdateReloadingSlider(timeToReload));
    }



    public void ChangeCancelButtonState(bool state)
        => _cancelButton.gameObject.SetActive(state);

    public override void EnableCanvas()
    {
        _cancelButton.gameObject.SetActive(false);

        base.EnableCanvas();
    }
}
