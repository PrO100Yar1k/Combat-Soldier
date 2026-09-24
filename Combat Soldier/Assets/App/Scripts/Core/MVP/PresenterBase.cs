using System;

namespace App.Scripts.MVP
{
    public abstract class PresenterBase<TModel, TView> : IDisposable where TModel : ModelBase where TView : IView
    {
        protected TModel Model { get; private set; }
        protected TView View { get; private set; }

        #region Disposable

        public virtual void Dispose()
        {
            Model = null;
            View = default;
        }

        #endregion

        protected PresenterBase(TModel model, TView canvasView)
        {
            Model = model ?? throw new ArgumentNullException(nameof(model));
            View = canvasView ?? throw new ArgumentNullException(nameof(canvasView));
        }

        public virtual void EnablePresenter()
        {
            View.Show();
        }

        public virtual void DisablePresenter()
        {
            View.Hide();
        }
    }
}
