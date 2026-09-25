using System;

namespace App.Scripts.MVP
{
    public abstract class PresenterBase<TModel, TView> : IDisposable, IPresenter where TModel : ModelBase where TView : IView
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

        protected PresenterBase(TModel model, TView screenCanvasView)
        {
            Model = model ?? throw new ArgumentNullException(nameof(model));
            View = screenCanvasView ?? throw new ArgumentNullException(nameof(screenCanvasView));
        }

        public virtual void EnablePresenter()
        {
            View?.Show();
        }

        public virtual void DisablePresenter()
        {
            View?.Hide();
        }
    }
}
