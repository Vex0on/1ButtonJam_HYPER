using TMPro;
using UnityEngine;

public class PointsCounter : MonoBehaviour
{
    public PlayerController player;

    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private float _startingPoint;
    [SerializeField] private float _highestPoint;


    private void Start()
    {
        _startingPoint = player.GetCurrentHeight(); 
    }

    private void Update()
    {
        float currentPoint = player.GetCurrentHeight() - _startingPoint;
        if(currentPoint > _highestPoint)
            _highestPoint = currentPoint;

        float pointsToConvert = Mathf.Floor(_highestPoint * 100);
        _text.SetText("Height: "+pointsToConvert.ToString());
    }
}
