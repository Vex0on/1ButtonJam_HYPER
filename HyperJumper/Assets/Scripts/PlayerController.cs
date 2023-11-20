using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("GameObjects")]
    [SerializeField] private GameObject arrow;
    [SerializeField] private GameObject player;

    [Header("Stats")]
    [SerializeField] private float maxJumpHigh = 2f;
    [SerializeField] private float maxJumpForce = 5f;
    [SerializeField] private float jumpForceMultiplier = 0.5f;
    [SerializeField] private float shortPressTime = 0.2f;

    private bool isSpaceHold = false;
    private bool hasPerformedRotation = false;
    private float spaceHoldTime = 0f;
    private Rigidbody2D rb;

    void Start()
    {
        rb = player.GetComponent<Rigidbody2D>();
    }

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
        }

        if (Input.GetKeyUp(spaceKey) && isSpaceHold)
        {
            if (spaceHoldTime < shortPressTime)
            {
                DirectionRotation();
                hasPerformedRotation = true;
            }
            else if (spaceHoldTime >= shortPressTime && !hasPerformedRotation)
            {
                Jump();
            }

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
        float jumpForce = CalculateJumpForce();

        float verticalMultiplier = 2f;
        float horizontalMultiplier = 0.5f;

        Vector2 jumpDirection = player.transform.right;
        Vector2 jumpForceVector = new Vector2(jumpDirection.x * jumpForce * horizontalMultiplier, jumpForce * verticalMultiplier);

        rb.AddForce(jumpForceVector, ForceMode2D.Impulse);

        spaceHoldTime = 0f;
    }


    float CalculateJumpForce()
    {
        float jumpForceHeight = Mathf.Clamp(spaceHoldTime, 0f, maxJumpHigh);
        float jumpForce = (maxJumpForce + jumpForceHeight * jumpForceMultiplier);

        return jumpForce;
    }
    

}
