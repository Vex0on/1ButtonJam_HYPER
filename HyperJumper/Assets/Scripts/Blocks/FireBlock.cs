using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBlock : Block
{

    [SerializeField] private float _fireBlockJumpIncrease = 1.5f;
    [SerializeField] private float _fireBlockVerticalIncrease = 1.5f;
    [SerializeField] private float _timeToDissipateBuff;

    public override void OnEnter(PlayerController player)
    {
        StopAllCoroutines();
        _playerController = player;
    }
    public override void OnStay()
    {
        if (_timeElapsedStandingOnBlock < _buffApplyTime)
            _timeElapsedStandingOnBlock += Time.deltaTime;
        IncreaseFireBuff(_playerController);
    }
    public override void OnExit()
    {
        StartCoroutine(DecreaseFireBuffOT(_timeToDissipateBuff));
        _timeElapsedStandingOnBlock = 0f;
    }

    private void IncreaseFireBuff(PlayerController player)
    {
        _playerSpriteRenderer.color = Color.Lerp(Color.white, Color.yellow, _timeElapsedStandingOnBlock / _buffApplyTime);

        _playerController._currentBlockJumpIncrease = Mathf.Lerp(_playerController._currentBlockJumpIncrease, _fireBlockJumpIncrease, _timeElapsedStandingOnBlock / _buffApplyTime);

        _playerController._currentBlockVerticalIncrease = Mathf.Lerp(_playerController._currentBlockVerticalIncrease, _fireBlockVerticalIncrease, _timeElapsedStandingOnBlock / _buffApplyTime);

        _playerController.honeyParticles.Emit(Mathf.FloorToInt(_timeElapsedStandingOnBlock));

    }
    private IEnumerator DecreaseFireBuffOT(float timeToDissipateDebuff)
    {
        float time = timeToDissipateDebuff;
        float currentVerticalIncrease = _playerController._currentBlockVerticalIncrease;
        float currentJumpIncrease = _playerController._currentBlockJumpIncrease;

        while (time >= 0)
        {
            time -= Time.deltaTime;
            _playerSpriteRenderer.color = Color.Lerp(Color.white, Color.red, time / timeToDissipateDebuff);

            _playerController._currentBlockJumpIncrease = Mathf.Lerp(1f, currentJumpIncrease, time / timeToDissipateDebuff);

            _playerController._currentBlockVerticalIncrease = Mathf.Lerp(1f, currentVerticalIncrease, time / timeToDissipateDebuff);

            _playerController.fireParticles.Emit(Mathf.FloorToInt(time));

            yield return null;
        }
    }
}
