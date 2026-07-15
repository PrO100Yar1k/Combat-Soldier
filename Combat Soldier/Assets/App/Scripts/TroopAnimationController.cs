using UnityEngine;
using Assets.App.Scripts;

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
        CrossFade(IdleHash, 0.15f);
    }

    public void PlayRunning()
    {
        CrossFade(RunningHash, 0.15f);
    }

    public void PlayAttack()
    {
        CrossFade(AttackHash, 0.15f);
    }

    public void PlayDefense()
    {
        CrossFade(DefenseHash, 0.15f);
    }

    private void CrossFade(int stateHash, float duration)
    {
        if (_animator == null)
            return;

        _animator.CrossFadeInFixedTime(stateHash, duration);
    }
}
