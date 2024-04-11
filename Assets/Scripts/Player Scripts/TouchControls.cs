using System.Collections.Generic;
using UnityEngine;

public class TouchControls : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;

    void Awake()
    {
        if (playerController == null)
        {
            Debug.LogError("PlayerController is not assigned.");
        }
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch touch = Input.GetTouch(i);

                // Check if the touch is on the left or right side of the screen
                if (touch.position.x < Screen.width / 2)
                {
                    if (touch.phase == TouchPhase.Began)
                        RightSideTouched();
                }
                if (touch.position.x > Screen.width / 2)
                {
                    if (touch.phase == TouchPhase.Began)
                        LeftSideTouched();
                }
            }
        }
    }

    void LeftSideTouched()
    {
        if (playerController != null)
        {
            playerController.ShootProjectile();
            Debug.Log("Left side touched");
        }
    }

    void RightSideTouched()
    {
        if (playerController != null)
        {
            playerController.MovementPolarity();
            Debug.Log("Right side touched");
        }
    }
}
