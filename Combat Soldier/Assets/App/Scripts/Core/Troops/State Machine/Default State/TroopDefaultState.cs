using Assets.App.Scripts;
using UnityEngine;

public abstract class TroopDefaultState : TroopBaseState
{
    protected TroopDefaultState(RepositoryManager repositoryManager, TroopController troopController, TroopScreenCanvasController screenCanvasController, ISwitchableState switcherState, ITroopAnimator animatorController)
        : base(repositoryManager, troopController, screenCanvasController, switcherState, animatorController)
    {

    }

    protected override void PlayStateAnimation()
    {
        _animatorController.PlayIdle();
    }

    protected override void EnableStateIcon()
    {
        Sprite targetIcon = Resources.Load<Sprite>("State Icons/default_icon");
        _screenCanvasController.ChangeStateIcon(targetIcon);
    }
}
