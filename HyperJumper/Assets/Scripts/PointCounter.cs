using TMPro;
using UnityEngine;

public class PointCounter : MonoBehaviour
{
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Vector2 _startingPoint;
    [SerializeField] private Vector2 _highestPoint;


    private void Start()
    {
        _playerController = GetComponent<PlayerController>();
        _startingPoint = _playerController.gameObject.transform.position; 
    }

    private void Update()
    {
        Vector2 currentPoint = new(0, _playerController.gameObject.transform.position.y - _startingPoint.y);
        if(currentPoint.y > _highestPoint.y)
            _highestPoint = currentPoint;

        float pointsToConvert = Mathf.Floor(_highestPoint.y * 100);
        _text.text = "Points: "+pointsToConvert.ToString();
    }
}
