using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PressAnyKeyScript : MonoBehaviour
{
    public TextMeshProUGUI text; // Reference to your TextMeshProUGUI
    private float pulseSpeed = 2f; // Speed of the pulse
    private float originalAlpha;

    void Start()
    {
        if (text != null)
        {
            // Store the original alpha value
            originalAlpha = text.color.a;
        }
    }

    void Update()
    {
        if (text != null)
        {
            Color currentColor = text.color;
            // Use Mathf.Abs to ensure the value stays positive, Mathf.Sin to create the pulsating effect
            currentColor.a = originalAlpha * Mathf.Abs(Mathf.Sin(Time.time * pulseSpeed));
            text.color = currentColor;
        }
    }
}
