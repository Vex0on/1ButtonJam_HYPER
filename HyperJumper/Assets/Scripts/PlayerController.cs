using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
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
                    Jump();

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
        Vector2 jumpForceVector = new(jumpDirection.x * jumpForce * horizontalMultiplier, jumpForce * verticalMultiplier);

        rb.AddForce(jumpForceVector, ForceMode2D.Impulse);
    }

    private float CalculateJumpForce()
    {
        float jumpForceHeight = Mathf.Clamp((float)_spaceHoldTime, 0f, _maxJumpHigh);
        float jumpForce = (_maxJumpForce + (jumpForceHeight * _jumpForceMultiplier));

        return jumpForce;
    }

}
