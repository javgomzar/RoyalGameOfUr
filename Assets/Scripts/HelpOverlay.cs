using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelpOverlay : MonoBehaviour
{
    public Canvas help_canvas;
    public Image arrows;
    public float blinking_period;
    // Start is called before the first frame update
    void Start()
    {
        help_canvas = GetComponent<Canvas>();
        arrows = GameObject.Find("Arrows").GetComponent<Image>();

        blinking_period = 0.2f;
    }

    // Update is called once per frame
    void Update()
    {
        Color alpha = arrows.color;
        alpha.a = 0.3f + 0.3f * Mathf.Cos(Time.time / blinking_period);
        arrows.color = alpha;
    }

    public void ToggleOverlay() {
        help_canvas.enabled = !help_canvas.enabled;
    }
}
