using App.Scripts.Infrastructure.Interfaces;
using UnityEngine;

namespace App.Scripts.Core.Troops.TroopScripts
{
    [RequireComponent(typeof(Animator))]
    public class TroopAnimationController : MonoBehaviour, ITroopAnimator
    {
        private readonly int _idleHash = Animator.StringToHash("Idle");
        private readonly int _runningHash = Animator.StringToHash("Run");
        private readonly int _attackHash = Animator.StringToHash("Attack");
        private readonly int _defenseHash = Animator.StringToHash("Defense");

        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public void PlayIdle()
        {
            if (IsAlreadyPlayingOrTransitioning(_idleHash))
                return;

            CrossFade(_idleHash, 0.15f);
        }

        public void PlayRunning()
        {
            if (IsAlreadyPlayingOrTransitioning(_runningHash))
                return;

            CrossFade(_runningHash, 0.1f);
        }

        public void PlayAttack()
        {
            CrossFade(_attackHash, 0.05f);
        }

        public void PlayDefense()
        {
            if (IsAlreadyPlayingOrTransitioning(_defenseHash))
                return;

            CrossFade(_defenseHash, 0.1f);
        }

        private void CrossFade(int stateHash, float duration)
        {
            if (_animator == null)
                return;

            _animator.CrossFadeInFixedTime(stateHash, duration);
        }

        private bool IsAlreadyPlayingOrTransitioning(int stateHash)
        {
            if (_animator == null)
                return false;

            if (_animator.IsInTransition(0))
            {
                AnimatorStateInfo nextState = _animator.GetNextAnimatorStateInfo(0);
                return nextState.shortNameHash == stateHash;
            }

            AnimatorStateInfo currentState = _animator.GetCurrentAnimatorStateInfo(0);

            return currentState.shortNameHash == stateHash;
        }
    }
}
