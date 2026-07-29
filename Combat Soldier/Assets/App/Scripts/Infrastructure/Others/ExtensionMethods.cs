namespace Assets.App.Scripts.Infrastructure.Others
{
    public static class ExtensionMethods
    {
        public static Faction GetOpposite(this Faction faction)
        {
            if (faction == Faction.None)
                return Faction.None;

            return faction == Faction.Allies ? Faction.Enemies : Faction.Allies;
        }
    }
}
