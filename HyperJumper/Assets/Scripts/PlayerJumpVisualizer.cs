using UnityEngine;
using UnityEngine.UI;

public class PlayerJumpVisualizer : MonoBehaviour
{
    public PlayerController player;
    public Slider scrollbar;


    private void Update()
    {
        scrollbar.value = player.GetJumpPercentage();
    }
}
