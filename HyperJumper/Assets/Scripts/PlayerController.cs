using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public LevelGenerator LevelGenerator;
    public Rigidbody2D rb;

    [Header("GameObjects")]
    [SerializeField] private Transform foot;
    [SerializeField] private GameObject arrow;
    public MainMenu menu;

    [Header("Values")]
    [SerializeField] private float _groundCheckWidth;
    [SerializeField] private float _distanceToGround;
    public LayerMask groundLayer;

    [Header("Stats")]
    [SerializeField] private float _maxJumpHigh = 2f;
    [SerializeField] private float _maxJumpForce = 5f;
    [SerializeField] private float _jumpForceMultiplier = 0.5f;
    [SerializeField] private float _shortPressTime = 0.2f;

    [SerializeField] private double _startSpaceHoldTime;
    [SerializeField] private double _spaceHoldTime;

    public float currentBlockVerticalIncrease = 1f;
    public float currentBlockJumpIncrease = 1f;

    [Header("VFX")]
    public ParticleSystem fireParticles;
    public ParticleSystem honeyParticles;

    [Header("Read Only")]
    [SerializeField] private Block _block;
    [SerializeField] private LadderBlock _ladderBlock;

    [Header("Debug")]
    public bool canJump;
    [SerializeField] private bool _isGrounded;
    [SerializeField] private Vector2 jumpForceVector;


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
                {
                    ChangeDirection();

                    if (_ladderBlock == null) break;
                    _ladderBlock.OnEnter(this);
                }
                else
                {
                    Jump(currentBlockVerticalIncrease, currentBlockJumpIncrease);
                }

                break;
        }
    }

    private void ChangeDirection()
    {
        transform.Rotate(Vector3.up, 180f);

        if (arrow == null) return;
        arrow.transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
    }

    private void Jump(float horizontalBuff, float verticalBuff)
    {
        _isGrounded = IsGrounded();
        if (!canJump && !_isGrounded) return;

        float jumpForce = CalculateJumpForce();

        float verticalMultiplier = 2f;
        float horizontalMultiplier = 0.5f;

        Vector2 jumpDirection = transform.right;
        jumpForceVector = new(jumpDirection.x * jumpForce * horizontalMultiplier * horizontalBuff, jumpForce * verticalMultiplier * verticalBuff);

        rb.AddForce(jumpForceVector, ForceMode2D.Impulse);
        canJump = false;
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
            StopAllCoroutines();
            block._playerRB = rb;
            block._playerSpriteRenderer = GetComponent<SpriteRenderer>();
            _block = block;
            _block.OnEnter(this);
        }
    }
    private void Update()
    {
        if (_block == null) return;
        _block.OnStay();
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (_block == null) return;
        _block.OnExit();
        _block = null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out LadderBlock ladderBlock))
        {
            ladderBlock._playerRB = rb;
            ladderBlock._playerSpriteRenderer = GetComponent<SpriteRenderer>();
            _ladderBlock = ladderBlock;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (_ladderBlock == null) return;
        _ladderBlock.OnExit();
        _ladderBlock = null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(new Ray(transform.position, jumpForceVector));

        Gizmos.color = Color.blue;
        Vector2 position = foot.position;

        Vector2 bottomLeft = position + new Vector2(-_groundCheckWidth, -_distanceToGround);
        Vector2 bottomRight = position + new Vector2(_groundCheckWidth, -_distanceToGround);

        Gizmos.DrawLine(bottomLeft, bottomRight);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(foot.transform.position, LevelGenerator.distanceToNextLevel);
    }

    private bool IsGrounded()
    {
        Vector2 position = foot.position;

        Vector2 bottomLeft = position + new Vector2(-_groundCheckWidth, 0f);
        Vector2 bottomRight = position + new Vector2(_groundCheckWidth, 0f);

        RaycastHit2D hitLeft = Physics2D.Raycast(bottomLeft, Vector2.down, _distanceToGround, groundLayer);
        RaycastHit2D hitRight = Physics2D.Raycast(bottomRight, Vector2.down, _distanceToGround, groundLayer);

        return hitLeft.collider != null || hitRight.collider != null;
    }

    public float GetCurrentHeight()
    {
        return foot.position.y;
    }
    public void ResetVars(Color spriteDebuffColor)
    {
        StartCoroutine(ResetVariablesGradually(_block.timeToDissipateBuff, spriteDebuffColor));
    }

    private IEnumerator ResetVariablesGradually(float timeToDissipateDebuff, Color color)
    {
        float time = timeToDissipateDebuff;
        float currentVerticalIncrease = currentBlockVerticalIncrease;
        float currentJumpIncrease = currentBlockJumpIncrease;
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        while (time >= 0)
        {
            time -= Time.deltaTime;
            sr.color = Color.Lerp(Color.white, color, time / timeToDissipateDebuff);
            currentBlockJumpIncrease = Mathf.Lerp(1f, currentJumpIncrease, time / timeToDissipateDebuff);
            currentBlockVerticalIncrease = Mathf.Lerp(1f, currentVerticalIncrease, time / timeToDissipateDebuff);
            honeyParticles.Emit(Mathf.FloorToInt(time));

            yield return null;
        }
    }
    public double GetSpaceHoldTime()
    {
        return _spaceHoldTime;
    }
    public float GetMaxJumpHigh()
    {
        return _maxJumpHigh;
    }
    public float GetShortPressTime()
    {
        return _shortPressTime;
    }
}
