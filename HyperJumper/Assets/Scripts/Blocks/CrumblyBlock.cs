using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrumblyBlock : Block
{
    private SpriteRenderer _objectSpriteRenderer;
    [SerializeField] private float _waitTimeForBlockRespawn;
    [SerializeField] private Sprite[] _destructionStageSprites;
    [SerializeField] private float _blockStageCrumbleTime;
    private float _currentBlockStageCrumbleTime;
    private int _currentDestructionStage = 0;

    private void Start()
    {
        _objectSpriteRenderer = GetComponent<SpriteRenderer>();
        _objectSpriteRenderer.sprite = _destructionStageSprites[0];
        _currentDestructionStage++;
        _currentBlockStageCrumbleTime = _blockStageCrumbleTime;
    }
    public override void OnStay()
    {
        _timeElapsedStandingOnBlock += Time.deltaTime;
        if(_timeElapsedStandingOnBlock >= _currentBlockStageCrumbleTime)
        {
            _objectSpriteRenderer.sprite = _destructionStageSprites[_currentDestructionStage];
            _currentDestructionStage++;
            _currentBlockStageCrumbleTime += _blockStageCrumbleTime;
        }
        if(_currentDestructionStage == _destructionStageSprites.Length) 
        {
            StartCoroutine(ActivateObjectAfterTime(_waitTimeForBlockRespawn));
        }
    }
    IEnumerator ActivateObjectAfterTime(float waitTime)
    {
        gameObject.SetActive(false);
        yield return new WaitForSeconds(waitTime);
        gameObject.SetActive(true);
    }
}
