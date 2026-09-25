using App.Scripts.Core.Troops.TroopScripts;

namespace App.Scripts.Infrastructure.Others
{
    public static class ExtensionMethods
    {
        public static Faction GetOpposite(this Faction faction) // make a dictionary if it would be more than 2 various of faction
        {
            if (faction == Faction.None)
                return Faction.None;

            return faction == Faction.Allies ? Faction.Enemies : Faction.Allies;
        }
    }
}
