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
        _playerController = player;
    }
    public override void OnStay()
    {
        if (_timeElapsedStandingOnBlock < _timeToDissipateBuff)
            _timeElapsedStandingOnBlock += Time.deltaTime;
        IncreaseFireBuff(_playerController);
    }
    public override void OnExit()
    {
        StartCoroutine(DecreaseFireBuff(_timeToDissipateBuff));
    }
    private void IncreaseFireBuff(PlayerController player)
    {
        _playerSpriteRenderer.color = Color.Lerp(Color.white, Color.red, _timeElapsedStandingOnBlock / _buffApplyTime);

        if (_playerController._currentBlockJumpIncrease < _fireBlockJumpIncrease)
            _playerController._currentBlockJumpIncrease += 0.005f;

        if (_playerController._currentBlockVerticalIncrease < _fireBlockVerticalIncrease)
            _playerController._currentBlockVerticalIncrease += 0.005f;
    }
    private IEnumerator DecreaseFireBuff(float timeToDissipateBuff)
    {
        float time = timeToDissipateBuff;
        float currentVerticalIncrease = _playerController._currentBlockVerticalIncrease;
        float currentJumpIncrease = _playerController._currentBlockJumpIncrease;

        while (time >= 0)
        {
            time -= Time.deltaTime;
            _playerSpriteRenderer.color = Color.Lerp(Color.white, Color.yellow, time / timeToDissipateBuff);

            _playerController._currentBlockJumpIncrease = Mathf.Lerp(currentJumpIncrease, _fireBlockJumpIncrease, time / timeToDissipateBuff);

            _playerController._currentBlockVerticalIncrease = Mathf.Lerp(currentVerticalIncrease, _fireBlockVerticalIncrease, time / timeToDissipateBuff);

            yield return null;
        }
    }
}
