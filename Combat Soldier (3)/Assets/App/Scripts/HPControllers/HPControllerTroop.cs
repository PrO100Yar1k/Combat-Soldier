
public class HPControllerTroop : HPController
{
    private readonly TroopController _troopController = default;

    private int _currentDefensePoint = default;
    private float _currentBlockRate = default;

    public HPControllerTroop(TroopController troopController, ScreenCanvasController troopCanvasController, TroopScriptable troopScriptable) : base (troopCanvasController)
    {
        _troopController = troopController;

        AssignBasicParameters(troopScriptable);
        ChangeSliderAndTextValues();
    }

    private void AssignBasicParameters(TroopScriptable troopScriptable)
    {
        _currentName = troopScriptable.Name;
        _currentBlockRate = troopScriptable.BlockRate;

        _currentHealPoint = troopScriptable.MaxHealPoint;
        _currentDefensePoint = troopScriptable.MaxDefencePoint;
    }

    protected override void ChangeSliderAndTextValues()
    {
        _troopCanvasController.ChangeHealPointSlider(_currentHealPoint);
        _troopCanvasController.ChangeDefensePointSlider(_currentDefensePoint);
    }

    #region Take Damage
    
    public override void TakeDamage(int attackDamage)
    {
        if (attackDamage <= 0)
            return;

        if (_troopController.StateController.CheckStateForActivity<TroopDefenseState>())
            TakeDamageWithDefenseState(attackDamage);

        else TakeDamageWithoutDefenseState(attackDamage);

        ChangeSliderAndTextValues();

        CheckHealPointsForTroopDeath();
    }

    private void TakeDamageWithDefenseState(int attackDamage)
    {
        int blockedHP = (int)(attackDamage * _currentBlockRate);
        int takenDamage = attackDamage - blockedHP;

        if (_currentDefensePoint >= blockedHP)
        {
            _currentDefensePoint -= blockedHP;
        }
        else
        {
            _currentHealPoint -= blockedHP - _currentDefensePoint;
            _currentDefensePoint = 0;
        }

        _currentHealPoint -= takenDamage;
    }

    private void TakeDamageWithoutDefenseState(int attackDamage)
    {
        _currentHealPoint -= attackDamage;
    }

    #endregion

    #region Defense Points

    public void IncreaseDefensePoints(int defensePoint)
    {
        if (defensePoint <= 0)
            return;

        _currentDefensePoint += defensePoint;

        ChangeSliderAndTextValues();
    }

    #endregion

    #region Death

    protected override void CheckHealPointsForTroopDeath()
    {
        if (_currentHealPoint <= 0)
            base.TroopDeath(_troopController, _troopController.gameObject);
    }

    #endregion
}