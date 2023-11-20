using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("GameObjects")]
    public GameObject arrow;
    public GameObject player;

    [Header("Stats")]
    public float maxJumpHigh = 2f;
    public float maxJumpForce = 5f;
    public float maxForwardForce = 3.5f;
    public float jumpForceMultiplier = 0.8f;
    public float shortPressTime = 0.2f;

    private bool isSpaceHold = false;
    private bool hasPerformedRotation = false;
    private float spaceHoldTime = 0f;

    void Update()
    {
        KeyCode spaceKey = KeyCode.Space;

        if (Input.GetKeyDown(spaceKey))
        {
            isSpaceHold = true;
            hasPerformedRotation = false;
            spaceHoldTime = 0f;
        }

        if (Input.GetKey(spaceKey) && isSpaceHold)
        {
            spaceHoldTime += Time.deltaTime;

            if (spaceHoldTime >= shortPressTime && !hasPerformedRotation)
            {
                Jump();
                isSpaceHold = false;
                spaceHoldTime = 0f;
            }
        }

        if (Input.GetKeyUp(spaceKey) && isSpaceHold)
        {
            DirectionRotation();
            hasPerformedRotation = true;
            isSpaceHold = false;
            spaceHoldTime = 0f;
        }
    }

    void DirectionRotation()
    {
        player.transform.Rotate(Vector3.up, 180f);

        if (arrow != null)
        {
            arrow.transform.rotation = Quaternion.Euler(0, player.transform.eulerAngles.y, 0);
        }
    }

    void Jump()
    {
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();

        Vector2 direction = new Vector2(player.transform.right.x, player.transform.right.y);
        Vector2 jumpVector = new Vector2(direction.x, direction.y + 0.5f);

        rb.velocity = jumpVector * CalculateJumpForce() + direction * maxForwardForce;

        spaceHoldTime = 0f;
    }

    float CalculateJumpForce()
    {
        float jumpForceHeight = Mathf.Clamp(spaceHoldTime, 0f, maxJumpHigh);
        return (maxJumpForce + jumpForceHeight * jumpForceMultiplier);
    }
}
