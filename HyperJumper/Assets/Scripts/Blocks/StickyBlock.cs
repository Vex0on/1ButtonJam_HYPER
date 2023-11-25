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
    [SerializeField] private float _timeToDissipateDebuff;

    public override void OnEnter(PlayerController player)
    {
        _playerController = player;
        _savedDrag = _playerRB.drag;
    }
    public override void OnStay()
    {
        if(_timeElapsedStandingOnBlock < _timeToDissipateDebuff)
            _timeElapsedStandingOnBlock += Time.deltaTime;
        IncreaseStickyDebuff(_playerController);
    }
    public override void OnExit()
    {
        _playerRB.drag = _savedDrag;
        StartCoroutine(DecreaseStickyDebuffOT(_timeToDissipateDebuff));
    }

    private void IncreaseStickyDebuff(PlayerController player)
    {
        _playerSpriteRenderer.color = Color.Lerp(Color.white, Color.yellow, _timeElapsedStandingOnBlock / _buffApplyTime);

        _playerRB.drag = Mathf.Lerp(_savedDrag, _stickyBlockSlowdown, _timeElapsedStandingOnBlock / _buffApplyTime);

        _playerController._currentBlockJumpIncrease = Mathf.Lerp(1f, _stickyBlockJumpIncrease, _timeElapsedStandingOnBlock / _buffApplyTime);

        _playerController._currentBlockVerticalIncrease = Mathf.Lerp(1f, _stickyBlockVerticalIncrease, _timeElapsedStandingOnBlock / _buffApplyTime);

    }
    private IEnumerator DecreaseStickyDebuffOT(float timeToDissipateDebuff)
    {
        float time = timeToDissipateDebuff;
        float currentVerticalIncrease = _playerController._currentBlockVerticalIncrease;
        float currentJumpIncrease = _playerController._currentBlockJumpIncrease;

        while (time >= 0)
        {
            time -= Time.deltaTime;
            _playerSpriteRenderer.color = Color.Lerp(Color.white, Color.yellow, time / timeToDissipateDebuff);

            _playerController._currentBlockJumpIncrease = Mathf.Lerp(1f, currentJumpIncrease, time / timeToDissipateDebuff);

            _playerController._currentBlockVerticalIncrease = Mathf.Lerp(1f, currentVerticalIncrease, time / timeToDissipateDebuff);

            yield return null;
        }
    }
}
