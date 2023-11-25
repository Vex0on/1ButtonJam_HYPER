using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;

    [Header("GameObjects")]
    [SerializeField] private GameObject arrow;
    [SerializeField] private GameObject player;
    [SerializeField] private MainMenu menu;

    [Header("Stats")]
    [SerializeField] private float _maxJumpHigh = 2f;
    [SerializeField] private float _maxJumpForce = 5f;
    [SerializeField] private float _jumpForceMultiplier = 0.5f;
    [SerializeField] private float _shortPressTime = 0.2f;

    [SerializeField] private double _startSpaceHoldTime;
    [SerializeField] private double _spaceHoldTime;


    [SerializeField] public float _currentBlockVerticalIncrease = 1f;
    [SerializeField] public float _currentBlockJumpIncrease = 1f;
    [Header("VFX")]
    [SerializeField] private ParticleSystem _fireParticles;
    [SerializeField] private ParticleSystem _honeyParticles;

    [SerializeField] private Block _block;
    void Start()
    {
        rb = player.GetComponent<Rigidbody2D>();
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (!menu.isGameStarted) return;

        switch (context.phase)
        {
            case InputActionPhase.Started:
                _startSpaceHoldTime = context.time;
                break;
            case InputActionPhase.Canceled:
                _spaceHoldTime = context.time - _startSpaceHoldTime;

                if (_spaceHoldTime < _shortPressTime)
                    DirectionRotation();
                else
                {
                    //_isTouchingStickyBlock = false;
                    //_isTouchingFireBlock = false;
                    if (_block != null)
                    {
                        _block.OnExit();
                        Jump(_currentBlockVerticalIncrease, _currentBlockJumpIncrease);
                        _block = null;
                    }
                    else
                        Jump(_currentBlockVerticalIncrease, _currentBlockJumpIncrease);
                    //rb.drag = _savedDrag;
                }

                break;
        }
    }

    private void DirectionRotation()
    {
        player.transform.Rotate(Vector3.up, 180f);

        if (arrow != null)
        {
            arrow.transform.rotation = Quaternion.Euler(0, player.transform.eulerAngles.y, 0);
        }
    }

    private void Jump(float horizontalBuff, float verticalBuff)
    {
        float jumpForce = CalculateJumpForce();

        float verticalMultiplier = 2f;
        float horizontalMultiplier = 0.5f;

        Vector2 jumpDirection = player.transform.right;
        Vector2 jumpForceVector = new Vector2(jumpDirection.x * jumpForce * horizontalMultiplier * horizontalBuff, jumpForce * verticalMultiplier * verticalBuff);

        rb.AddForce(jumpForceVector, ForceMode2D.Impulse);
    }

    private float CalculateJumpForce()
    {
        float jumpForceHeight = Mathf.Clamp((float)_spaceHoldTime, 0f, _maxJumpHigh);
        float jumpForce = (_maxJumpForce + (jumpForceHeight * _jumpForceMultiplier));

        return jumpForce;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Block block))
        {
            block._playerRB = rb;
            block._playerSpriteRenderer = GetComponent<SpriteRenderer>();
            _block = block;
            _block.OnEnter(this);
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (_block == null) return;
        _block.OnStay();
    }
    private void Update()
    {
        /*
        if(_isTouchingFireBlock && !_isTouchingStickyBlock)
        {
            _timeElapsedStandingOnBlock += Time.deltaTime;
            IncreaseFireBuff();
            _fireParticles.Emit(1);
        }
        else
        {
            DecreaseFireBuff();
        }
        if (_isTouchingStickyBlock && !_isTouchingFireBlock)
        {
            _timeElapsedStandingOnBlock += Time.deltaTime;
            IncreaseStickyDebuff();
            _honeyParticles.Emit(1);
        }
        else
        {
            DecreaseStickyDebuff();
        }
        if(!_isTouchingStickyBlock && !_isTouchingFireBlock)
        {
            _timeElapsedStandingOnBlock -= Time.deltaTime;
        }*/
    }
    
   
}
