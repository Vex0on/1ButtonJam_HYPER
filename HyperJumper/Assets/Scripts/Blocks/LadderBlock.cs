using UnityEngine;

public class LadderBlock : Block
{
    [SerializeField] private float _savedDrag = 0;

    public override void OnEnter(PlayerController player)
    {
        _playerController = player;
        _savedDrag = _playerRB.drag;
        _playerRB.MovePosition(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y));
        _playerRB.drag = 9999f;
        _playerController.canJump = true;
    }
    public override void OnExit()
    {
        _playerRB.drag = 0;
        _timeElapsedStandingOnBlock = 0f;
    }
}
