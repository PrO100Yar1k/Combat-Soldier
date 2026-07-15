namespace Assets.App.Scripts
{
    public interface ITroopAnimator
    {
        public void PlayIdle();
        public void PlayRunning();
        public void PlayAttack();
        public void PlayDefense();
    }
}