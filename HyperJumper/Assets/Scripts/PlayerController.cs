using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private SpriteRenderer playerSprite;
    private Rigidbody2D rb;

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

    [Header("Block-related variables")]
    [SerializeField] private bool _isTouchingStickyBlock;
    [SerializeField] private bool _isTouchingFireBlock;
    [SerializeField] private float _desiredTimeToChangeSpriteColor = 5f;
    [SerializeField] private float _stickyBlockSlowdown = 100f;
    [SerializeField] private float _stickyBlockJumpIncrease = 0.5f;
    [SerializeField] private float _stickyBlockVerticalIncrease = 0.5f;
    [SerializeField] private float _fireBlockJumpIncrease = 1.5f;
    [SerializeField] private float _fireBlockVerticalIncrease = 1.5f;
    [Header("Tracker vars")]
    [SerializeField] private float _currentBlockJumpIncrease = 1f;
    [SerializeField] private float _currentBlockVerticalIncrease = 1f;
    [SerializeField] private float _savedDrag = 0;
    [SerializeField] private float _timeElapsedStandingOnBlock;
    [Header("VFX")]
    [SerializeField] private ParticleSystem _fireParticles;
    [SerializeField] private ParticleSystem _honeyParticles;


    void Start()
    {
        rb = player.GetComponent<Rigidbody2D>();
        playerSprite = player.GetComponent<SpriteRenderer>();

    }

    public void Jump(InputAction.CallbackContext context)
    {

        if (!menu.isGameStarted)
        {
            return;
        }

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
                    _isTouchingStickyBlock = false;
                    _isTouchingFireBlock = false;
                    Jump();
                    rb.drag = _savedDrag;
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

    private void Jump()
    {
        float jumpForce = CalculateJumpForce();

        float verticalMultiplier = 2f;
        float horizontalMultiplier = 0.5f;

        Vector2 jumpDirection = player.transform.right;
        Vector2 jumpForceVector = new Vector2(jumpDirection.x * jumpForce * horizontalMultiplier * _currentBlockVerticalIncrease, jumpForce * verticalMultiplier * _currentBlockJumpIncrease);

        rb.AddForce(jumpForceVector, ForceMode2D.Impulse);
    }

    private float CalculateJumpForce()
    {
        float jumpForceHeight = Mathf.Clamp((float)_spaceHoldTime, 0f, _maxJumpHigh);
        float jumpForce = (_maxJumpForce + (jumpForceHeight * _jumpForceMultiplier));

        return jumpForce;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out StickyBlockFunctionality s))
        {
            _isTouchingStickyBlock = true;
            _savedDrag = rb.drag;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out StickyBlockFunctionality s))
        {
            _isTouchingStickyBlock = true;
        }
        if (collision.TryGetComponent(out FireBlockFunctionality f))
        {
            _isTouchingFireBlock = true;
        }
    }
    private void Update()
    {
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
        }
    }
    private void IncreaseFireBuff()
    {
        playerSprite.color = Color.Lerp(Color.white, Color.red, _timeElapsedStandingOnBlock / _desiredTimeToChangeSpriteColor);

        if (_currentBlockJumpIncrease < _fireBlockJumpIncrease)
            _currentBlockJumpIncrease += 0.005f;

        if (_currentBlockVerticalIncrease < _fireBlockVerticalIncrease)
            _currentBlockVerticalIncrease += 0.005f;
    }
    private void DecreaseFireBuff()
    {
        playerSprite.color = Color.Lerp(Color.red, Color.white, _timeElapsedStandingOnBlock / _desiredTimeToChangeSpriteColor);

        if (_currentBlockJumpIncrease > 1f)
            _currentBlockJumpIncrease -= 0.005f;

        if (_currentBlockVerticalIncrease > 1f)
            _currentBlockVerticalIncrease -= 0.005f;
    }
    private void IncreaseStickyDebuff()
    {
        playerSprite.color = Color.Lerp(Color.white, Color.yellow, _timeElapsedStandingOnBlock / _desiredTimeToChangeSpriteColor);

        if (rb.drag < _stickyBlockSlowdown)
            rb.drag += 3;

        if (_currentBlockJumpIncrease >= _stickyBlockJumpIncrease)
            _currentBlockJumpIncrease -= 0.005f;

        if (_currentBlockVerticalIncrease >= _stickyBlockVerticalIncrease)
            _currentBlockVerticalIncrease -= 0.005f;
    }
    private void DecreaseStickyDebuff()
    {
        playerSprite.color = Color.Lerp(Color.yellow, Color.white, _timeElapsedStandingOnBlock / _desiredTimeToChangeSpriteColor);

        if (_currentBlockJumpIncrease < 1f)
            _currentBlockJumpIncrease += 0.005f;

        if (_currentBlockVerticalIncrease < 1f)
            _currentBlockVerticalIncrease += 0.005f;
    }
}
