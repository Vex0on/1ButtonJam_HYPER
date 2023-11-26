using UnityEngine;

public abstract class Block : MonoBehaviour
{
    protected PlayerController _playerController;
    [SerializeField] public Rigidbody2D _playerRB;
    [SerializeField] public SpriteRenderer _playerSpriteRenderer;
    [Header("Tracker vars")]
    [SerializeField] protected float _timeElapsedStandingOnBlock;
    public float timeToDissipateBuff;
    [SerializeField] public float _buffApplyTime = 5f;

    public virtual void OnEnter(PlayerController player) { }
    public virtual void OnStay() { }
    public virtual void OnExit() { }
}
