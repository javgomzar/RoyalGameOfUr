using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectFirstPlayer : MonoBehaviour, IPointerDownHandler
{
    public Piece.Color color;
    public GameController gc;
    public Dice dice;
    public Canvas menu_canvas;
    // Start is called before the first frame update
    void Start()
    {
        gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        gc.dice.enabled = false;

        menu_canvas = GameObject.Find("StartingPlayerMenuCanvas").GetComponent<Canvas>();
        menu_canvas.enabled = true;
    }

    public void OnPointerDown(PointerEventData eventData) {
        gc.turn = color;

        if (gc.play_mode == PlayMode.cpu) {
            gc.ai_players.Add(Piece.OppositeColor(color));
        }
        menu_canvas.enabled = false;
        gc.dice.enabled = true;
        gc.FadeIn();
    }
}
