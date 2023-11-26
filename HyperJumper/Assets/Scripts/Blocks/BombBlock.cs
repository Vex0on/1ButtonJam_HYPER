using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombBlock : Block
{
    [SerializeField] private float _timeToDissipateDebuff;
    [SerializeField] private float _explosionJumpIncrease = 0.5f;
    [SerializeField] private float _explosionVerticalIncrease = 0.5f;
    [SerializeField] private float _explosionForce = 0.5f;
    [SerializeField] private ParticleSystem _explosionVFX;
    public override void OnEnter(PlayerController player)
    {
        StopAllCoroutines();
        _playerController = player;
        ExplodePlayer();
    }
    public override void OnExit()
    {
        StartCoroutine(DecreaseExplosionDebuff(_timeToDissipateDebuff));
        _timeElapsedStandingOnBlock = 0f;
    }

    private void ExplodePlayer()
    {
        Vector2 explosionDirection = _playerController.transform.position - transform.position;

        _playerRB.AddForce(explosionDirection.normalized * _explosionForce, ForceMode2D.Impulse);
        _playerController._currentBlockJumpIncrease = _explosionJumpIncrease;
        _playerController._currentBlockVerticalIncrease = _explosionVerticalIncrease;
        ParticleSystem explosionParticle = Instantiate(_explosionVFX, this.transform);
        explosionParticle.Play();
        StartCoroutine(CreateExplosionVFX());

    }
    private IEnumerator DecreaseExplosionDebuff(float timeToDissipateDebuff)
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

            _playerController.honeyParticles.Emit(Mathf.FloorToInt(time));

            yield return null;
        }
    }
    private IEnumerator CreateExplosionVFX()
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }

}
