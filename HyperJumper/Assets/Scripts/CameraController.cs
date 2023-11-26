using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;

    [Header("Values")]
    public bool isTargeting;
    public float minHeight;
    [Range(0f, 1f)] public float smoothSpeed;
    public float maxSpeed;
    public Vector2 heightOffset;

    [Header("Debug")]
    [SerializeField] private float _desiredHeight;
    [SerializeField] private float _smoothedHeight;


    private void Start()
    {
        transform.position = new(0, minHeight, -10);
    }

    private void LateUpdate()
    {
        if (!isTargeting || target == null) return;

        _desiredHeight = Mathf.Max(target.position.y + heightOffset.y, minHeight);
        _smoothedHeight = Mathf.Lerp(transform.position.y, _desiredHeight, maxSpeed * Time.deltaTime);

        transform.position = new(0, _smoothedHeight, -10);
    }
}
