namespace App.Scripts.Core.HPControllers
{
    public abstract class BaseDamageHandler : IDamageHandler
    {
        private IDamageHandler _nextHandler;

        public void SetNext(IDamageHandler nextHandler)
        {
            _nextHandler = nextHandler;
        }

        public virtual void Handle(DamageContext context)
        {
            _nextHandler?.Handle(context);
        }
    }
}