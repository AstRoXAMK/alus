using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositiveBarrierOpacity : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    private PlayerController playerController;
    private float currentOpacity = 1.0f; // Default opacity

    void Start()
    {
        // Ensure that spriteRenderer is assigned
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        SetOpacity(currentOpacity);
    }

    void Update()
    {
        if (GameOver.isGameOver == false)
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                // Check if the touch phase is at the beginning of a touch
                if (touch.phase == TouchPhase.Began)
                {
                    if (touch.position.x < Screen.width / 2 && PauseMenu.isPaused == false)
                    {
                        ToggleOpacity();
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse0))
                ToggleOpacity();
        }
    }

    void ToggleOpacity()
    {
        // Toggle between 0.75 and 0.25
        currentOpacity = (currentOpacity == 1.0f) ? 0.1f : 1.0f;
        SetOpacity(currentOpacity);
    }

    void SetOpacity(float alpha)
    {
        if (spriteRenderer != null)
        {
            Color color = spriteRenderer.color;
            color.a = alpha;
            spriteRenderer.color = color;
        }
    }
}
