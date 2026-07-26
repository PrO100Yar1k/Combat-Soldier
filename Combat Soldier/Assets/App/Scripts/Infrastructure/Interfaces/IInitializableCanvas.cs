namespace Assets.App.Scripts
{
    public interface IInitializableCanvas<TData> : ICanvasView
    {
        public void Initialize(TData data);
    }
}
