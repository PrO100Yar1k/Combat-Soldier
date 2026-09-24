namespace App.Scripts.Core.HPControllers
{
    public class DamageContext
    {
        public int IncomingDamage { get; set; }
        public int DamageToHealth { get; set; }
        public bool IsInDefenseState { get; set; }

        public DamageContext(int incomingDamage, bool isInDefenseState = false)
        {
            IncomingDamage = incomingDamage;
            DamageToHealth = incomingDamage;
            IsInDefenseState = isInDefenseState;
        }
    }
}