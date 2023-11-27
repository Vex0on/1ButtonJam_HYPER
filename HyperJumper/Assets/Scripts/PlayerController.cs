using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public LevelGenerator LevelGenerator;
    public Rigidbody2D rb;
    public SpriteRenderer spriteRenderer;

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
    [SerializeField] private bool _pressingSpace;
    [SerializeField] private bool _isGrounded;
    [SerializeField] private double _spaceHoldTime;
    [SerializeField] private Vector2 jumpForceVector;


    public void Jump(InputAction.CallbackContext context)
    {
        if (!menu.isGameStarted) return;

        switch (context.phase)
        {
            case InputActionPhase.Started:
                _spaceHoldTime = 0;
                _pressingSpace = true;
                break;
            case InputActionPhase.Canceled:
                if (_spaceHoldTime < _shortPressTime)
                {
                    ChangeDirection();

                    if (_ladderBlock != null)
                        _ladderBlock.OnEnter(this);
                }
                else
                {
                    Jump(currentBlockVerticalIncrease, currentBlockJumpIncrease);
                }

                _pressingSpace = false;
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
        rb.drag = 0;

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

    private void Update()
    {
        if (!menu.isGameStarted) return;

        if (_pressingSpace)
            _spaceHoldTime += Time.deltaTime;

        if (_block != null)
            _block.OnStay();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Block block))
        {
            block._playerRB = rb;
            block._playerSpriteRenderer = spriteRenderer;
            _block = block;
            _block.OnEnter(this);
        }
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
            ladderBlock._playerSpriteRenderer = spriteRenderer;
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
    public float GetJumpPercentage()
    {
        return (float)_spaceHoldTime / _maxJumpHigh;
    }
}
