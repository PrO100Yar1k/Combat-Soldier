namespace App.Scripts.MVP
{
    public abstract class ModelBase
    {
        public bool IsInitialized { get; private set; }

        protected ModelBase()
        {
            InitializeModel();
        }
        
        private void InitializeModel()
        {
            IsInitialized = true;
        }

        public abstract void ResetState();
    }
}
