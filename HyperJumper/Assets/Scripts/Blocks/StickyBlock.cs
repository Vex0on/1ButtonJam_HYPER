using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class StickyBlock : Block
{
    [SerializeField] private float _stickyBlockSlowdown = 100f;
    [SerializeField] private float _stickyBlockJumpIncrease = 0.5f;
    [SerializeField] private float _stickyBlockVerticalIncrease = 0.5f;
    [SerializeField] private float _savedDrag = 0;

    public override void OnEnter(PlayerController player)
    {
        StopAllCoroutines();
        _playerController = player;
        _savedDrag = _playerRB.drag;
    }
    public override void OnStay()
    {
        if(_timeElapsedStandingOnBlock < _buffApplyTime)
            _timeElapsedStandingOnBlock += Time.deltaTime;
        IncreaseStickyDebuff(_playerController);
    }
    public override void OnExit()
    {
        _playerRB.drag = _savedDrag;
        _playerController.ResetVars(Color.yellow);
        _timeElapsedStandingOnBlock = 0f;
        _playerController.honeyParticles.Stop();
    }

    private void IncreaseStickyDebuff(PlayerController player)
    {
        _playerSpriteRenderer.color = Color.Lerp(Color.white, Color.yellow, _timeElapsedStandingOnBlock / _buffApplyTime);

        if(_playerRB.drag < _stickyBlockSlowdown)
            _playerRB.drag += 3;

        _playerController._currentBlockJumpIncrease = Mathf.Lerp(_playerController._currentBlockJumpIncrease, _stickyBlockJumpIncrease, _timeElapsedStandingOnBlock / _buffApplyTime);

        _playerController._currentBlockVerticalIncrease = Mathf.Lerp(_playerController._currentBlockVerticalIncrease, _stickyBlockVerticalIncrease, _timeElapsedStandingOnBlock / _buffApplyTime);

        float particleMultiplier = Mathf.Lerp(0f, 1f, _timeElapsedStandingOnBlock / _buffApplyTime);
        int particleCount = Mathf.CeilToInt(particleMultiplier);
        _playerController.honeyParticles.Emit(particleCount);


    }
}
