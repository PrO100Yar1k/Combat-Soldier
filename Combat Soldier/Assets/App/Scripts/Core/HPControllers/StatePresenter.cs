using UnityEngine;
using App.Scripts.Views;
using App.Scripts.Core.HPControllers;
using App.Scripts.Core.Troops.StateMachine.State_Controller;

namespace App.Scripts.Core.Troops
{
    public class StatePresenter // : IPresenter
    {
        private readonly TroopStateController _stateController;
        private readonly StateIconView _view;

        public StatePresenter(TroopStateController stateController, StateIconView view)
        {
            _stateController = stateController;
            _view = view;

            SetupChangingStateIcon();
            SetupChangingStateButton();
        }
        
        private void SetupChangingStateButton()
        {
            if (_stateController is ISwitchableOppositeState switchableOppositeState)
            {
                _view.SetupChangeStateButton(switchableOppositeState);
            }
        }
        
        private void SetupChangingStateIcon()
        {
            _stateController.OnStateIconChanged += HandleStateIconChanged;
        }
        
        private void HandleStateIconChanged(Sprite spriteIcon)
        {
            _view.SetStateIcon(spriteIcon);
        }
    }
}