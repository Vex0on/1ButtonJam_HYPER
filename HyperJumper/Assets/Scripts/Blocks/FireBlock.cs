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
        _playerSpriteRenderer.color = Color.Lerp(Color.white, Color.red, _timeElapsedStandingOnBlock / _buffApplyTime);

        _playerController._currentBlockJumpIncrease = Mathf.Lerp(_playerController._currentBlockJumpIncrease, _fireBlockJumpIncrease, _timeElapsedStandingOnBlock / _buffApplyTime);

        _playerController._currentBlockVerticalIncrease = Mathf.Lerp(_playerController._currentBlockVerticalIncrease, _fireBlockVerticalIncrease, _timeElapsedStandingOnBlock / _buffApplyTime);

        float particleMultiplier = Mathf.Lerp(0f,1f,_timeElapsedStandingOnBlock/_buffApplyTime);
        int particleCount = Mathf.CeilToInt(particleMultiplier);
        _playerController.fireParticles.Emit(particleCount);

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

            float particleMultiplier = Mathf.Lerp(0f, 1f, time / timeToDissipateDebuff);
            int particleCount = Mathf.CeilToInt(particleMultiplier);
            _playerController.fireParticles.Emit(particleCount);

            yield return null;
        }
    }
}