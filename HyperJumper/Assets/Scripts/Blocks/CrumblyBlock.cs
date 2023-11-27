using System.Collections;
using UnityEngine;

public class CrumblyBlock : Block
{
    public SpriteRenderer spriteRenderer;
    public BoxCollider2D mainCollider;
    public BoxCollider2D sideBouncesCollider;

    [SerializeField] private float _waitTimeForBlockRespawn;
    [SerializeField] private Sprite[] _destructionStageSprites;
    [SerializeField] private float _blockStageCrumbleTime;
    [SerializeField] private float _currentBlockStageCrumbleTime;
    private int _currentDestructionStage = 0;

    private void Start()
    {
        spriteRenderer.sprite = _destructionStageSprites[0];
        _currentDestructionStage++;
        _currentBlockStageCrumbleTime = _blockStageCrumbleTime;
    }
    public override void OnStay()
    {
        _timeElapsedStandingOnBlock += Time.deltaTime;
        if(_timeElapsedStandingOnBlock >= _currentBlockStageCrumbleTime)
        {
            spriteRenderer.sprite = _destructionStageSprites[_currentDestructionStage];
            _currentDestructionStage++;
            _currentBlockStageCrumbleTime += _blockStageCrumbleTime;
        }
        if(_currentDestructionStage == _destructionStageSprites.Length) 
        {
            StartCoroutine(ResetBlock(_waitTimeForBlockRespawn));
        }
    }
    public override void OnExit()
    {
        _timeElapsedStandingOnBlock = 0f;
    }
    private IEnumerator ResetBlock(float waitTime)
    {
        spriteRenderer.enabled = false;
        mainCollider.enabled = false;
        sideBouncesCollider.enabled = false;
        yield return new WaitForSeconds(waitTime);
        spriteRenderer.enabled = true;
        mainCollider.enabled = true;
        sideBouncesCollider.enabled = true;
        _timeElapsedStandingOnBlock = 0f;
        spriteRenderer.sprite = _destructionStageSprites[0];
        _currentDestructionStage = 0;
        _currentBlockStageCrumbleTime = 2;
    }
}
