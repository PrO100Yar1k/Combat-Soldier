using UnityEngine;

public abstract class TroopDefaultState : TroopBaseState // remove this class
{
    public TroopDefaultState(RepositoryManager repositoryManager, TroopController troopController, TroopScreenCanvasController screenCanvasController, ISwitchableState switcherState)
        : base(repositoryManager, troopController, screenCanvasController, switcherState) 
    {

    }

    protected override void EnableStateIcon()
    {
        Sprite targetIcon = Resources.Load<Sprite>("State Icons/default_icon");
        _screenCanvasController.ChangeStateIcon(targetIcon);
    }
}
