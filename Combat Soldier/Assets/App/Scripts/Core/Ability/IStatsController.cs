using App.Scripts.Infrastructure.Enums;
using App.Scripts.Infrastructure.Others;

namespace App.Scripts.Core.Ability
{
    public interface IStatsController
    {
        float GetStatValueFloat(StatType statType);
        int GetStatValueInt(StatType statType);
        Result<Stat> GetStat(StatType statType);
    }
}