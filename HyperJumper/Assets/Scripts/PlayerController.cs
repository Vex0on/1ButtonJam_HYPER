using System.Collections;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer playerSprite;

    [Header("GameObjects")]
    [SerializeField] private GameObject arrow;
    [SerializeField] private GameObject player;

    [Header("Stats")]
    [SerializeField] private float _maxJumpHigh = 2f;
    [SerializeField] private float _maxJumpForce = 5f;
    [SerializeField] private float _jumpForceMultiplier = 0.5f;
    [SerializeField] private float _shortPressTime = 0.2f;

    [SerializeField] private double _spaceHoldTime = 0f;

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
        switch (context.phase)
        {
            case InputActionPhase.Started:
                _spaceHoldTime = context.time;
                break;
            case InputActionPhase.Canceled:
                var time = context.time - _spaceHoldTime;

                if (time < _shortPressTime)
                    DirectionRotation();
                else
                {
                    rb.drag = _savedDrag;
                    Jump();
                    _isTouchingStickyBlock = false;
                    _isTouchingFireBlock = false;
                    _timeElapsedStandingOnBlock = 0;
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

        _spaceHoldTime = 0f;
    }

    private float CalculateJumpForce()
    {
        float jumpForceHeight = Mathf.Clamp((float)_spaceHoldTime, 0f, _maxJumpHigh);
        float jumpForce = (_maxJumpForce + jumpForceHeight * _jumpForceMultiplier);

        return jumpForce;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {   
        if (collision.TryGetComponent(out StickyBlockFunctionality s))
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
        if (_isTouchingStickyBlock)
        {
            IncreaseStickyDebuff();
            _timeElapsedStandingOnBlock += Time.deltaTime;
            if(_timeElapsedStandingOnBlock > _desiredTimeToChangeSpriteColor)
            {

            }
        }
        else
            DecreaseStickyDebuff();
        
        if (_isTouchingFireBlock)
        {
            IncreaseFireBuff();
            _timeElapsedStandingOnBlock += Time.deltaTime;
            if (_timeElapsedStandingOnBlock > _desiredTimeToChangeSpriteColor)
            {

            }
        }
        else
            DecreaseFireBuff();
    }
    private void IncreaseFireBuff()
    {
        playerSprite.color = Color.Lerp(Color.white, Color.red, _timeElapsedStandingOnBlock/_desiredTimeToChangeSpriteColor);
        
        if (_currentBlockJumpIncrease < _fireBlockJumpIncrease)
            _currentBlockJumpIncrease += 0.005f;

        if (_currentBlockVerticalIncrease < _fireBlockVerticalIncrease)
            _currentBlockVerticalIncrease += 0.005f;
    }
    private void DecreaseFireBuff()
    {
        playerSprite.color = Color.Lerp(Color.white, Color.yellow, _timeElapsedStandingOnBlock/_desiredTimeToChangeSpriteColor);

        if (_currentBlockJumpIncrease > 1f)
            _currentBlockJumpIncrease -= 0.005f;

        if (_currentBlockVerticalIncrease > 1f)
            _currentBlockVerticalIncrease -= 0.005f;
    }
    private void IncreaseStickyDebuff()
    {
        if(rb.drag < _stickyBlockSlowdown)
        rb.drag += 3;
        
        if (_currentBlockJumpIncrease >= _stickyBlockJumpIncrease)
        _currentBlockJumpIncrease -= 0.005f;

        if (_currentBlockVerticalIncrease >= _stickyBlockVerticalIncrease)
        _currentBlockVerticalIncrease -= 0.005f;
    }
    private void DecreaseStickyDebuff()
    {
       if (_currentBlockJumpIncrease < 1f)
        _currentBlockJumpIncrease += 0.005f;

        if (_currentBlockVerticalIncrease < 1f)
        _currentBlockVerticalIncrease += 0.005f;
    }
}
