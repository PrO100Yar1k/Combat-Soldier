using App.Scripts.Infrastructure.Interfaces;
using UnityEngine;

namespace App.Scripts.Core.Troops.Troop_Scripts
{
    public class TroopAnimationController : MonoBehaviour, ITroopAnimator
    {
        private readonly int IdleHash = Animator.StringToHash("Idle");
        private readonly int RunningHash = Animator.StringToHash("Run");
        private readonly int AttackHash = Animator.StringToHash("Attack");
        private readonly int DefenseHash = Animator.StringToHash("Defense");

        private Animator _animator = default;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public void PlayIdle()
        {
            if (IsAlreadyPlayingOrTransitioning(IdleHash))
                return;

            CrossFade(IdleHash, 0.15f);
        }

        public void PlayRunning()
        {
            if (IsAlreadyPlayingOrTransitioning(RunningHash))
                return;

            CrossFade(RunningHash, 0.1f);
        }

        public void PlayAttack()
        {
            CrossFade(AttackHash, 0.05f);
        }

        public void PlayDefense()
        {
            if (IsAlreadyPlayingOrTransitioning(DefenseHash))
                return;

            CrossFade(DefenseHash, 0.1f);
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
