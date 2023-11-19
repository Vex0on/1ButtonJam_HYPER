using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject arrow;
    public GameObject player;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Click");
            PlayerChangeDirection();
        }
    }

    void PlayerChangeDirection()
    {
        player.transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);

        if (arrow != null)
        {
            arrow.transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
        }
    }
}