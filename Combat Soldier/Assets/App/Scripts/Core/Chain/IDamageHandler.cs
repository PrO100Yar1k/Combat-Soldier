namespace App.Scripts.Core.HPControllers
{
    public interface IDamageHandler
    {
        void SetNext(IDamageHandler nextHandler);
        void Handle(DamageContext context);
    }
}