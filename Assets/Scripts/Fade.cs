using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    public float alpha;
    private Image img;
    bool fade_in;
    bool fade_out;
    public float velocity;
    public float time;
    public Canvas canvas;
    // Start is called before the first frame update
    void Start()
    {
        img = GetComponent<Image>();
        canvas = GetComponentInParent<Canvas>();

        fade_in = false;
        fade_out = false;

        alpha = 1;
        canvas.enabled = true;
    }

    void FixedUpdate()
    {
        // Set alpha
        Color color = img.color;
        color.a = alpha;
        img.color = color;

        // Do fades
        if (fade_in) {
            fade_in = FadeIn();
            if (!fade_in) {
                alpha = 0;
            }
        }
        if (fade_out) {
            fade_out = FadeOut();
            if (!fade_out) {
                alpha = 1;
            }
        }
    }

    bool FadeIn() {
        alpha -= velocity * (Time.time - time);
        return alpha > 0;
    }

    bool FadeOut() {
        alpha += velocity * (Time.time - time);
        return alpha < 1;
    }

    public void StartFadeIn() {
        if (!fade_in) {
            fade_in = true;
        }
        time = Time.time;
    }

    public void StartFadeOut() {
        if (!fade_out) {
            fade_out = true;
        }
        time = Time.time;
    }
}
