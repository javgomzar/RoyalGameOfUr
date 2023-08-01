using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static Utils;
using Color=GameColor.Color;

public class Piece : MonoBehaviour, IDragHandler, IEndDragHandler, IPointerDownHandler
{
    public Canvas canvas;
    public Color color;
    public int steps;
    public GameController gc;
    public Camera cam;
    public Vector3 initial_position;

    void Start() { 
        gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        steps = 0;
        canvas = GetComponent<Canvas>();
    }

    public void OnPointerDown(PointerEventData eventData) {
        initial_position = transform.localPosition;
    }

    public void OnDrag(PointerEventData eventData) {
        if (gc.dice.dice_rolled_this_turn && gc.turn == color && steps != 15) {
            transform.localPosition = GetMouse(cam);
            canvas.sortingOrder = 8;
        }
    }

    public void OnEndDrag(PointerEventData eventData) {
        Vector3 mouse_offset = GetMouse(cam);
        bool valid_square = Board.IsValidSquare(mouse_offset, color);

        if (valid_square) {
            int new_steps = Board.PositionToSteps(mouse_offset);
            if (new_steps == steps + gc.dice.roll && gc.possible_moves.Contains(this)) {
                gc.ExecuteMove(this);
                Debug.Log("Executed move to square " + new_steps);
                return;
            }
            Debug.Log("Attempted move is invalid.");
        }
        else {
            Debug.Log("Attempted move outside the board.");
        }
        
        if (steps != 15) {
            canvas.sortingOrder = 1;
            transform.localPosition = initial_position;
        }
    }

    public void Remove(Vector3 available_position) {
        steps = 0;
        transform.localPosition = available_position;
    }
}
