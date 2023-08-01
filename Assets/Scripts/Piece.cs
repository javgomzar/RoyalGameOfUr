using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Piece : MonoBehaviour, IDragHandler, IEndDragHandler, IPointerDownHandler
{
    public Canvas canvas;
    public enum Color {Black, White};
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

    public static Color OppositeColor(Color color) {
        if (color == Color.Black) {
            return Color.White;
        }
        else {
            return Color.Black;
        }
    }

    public void OnPointerDown(PointerEventData eventData) {
        initial_position = transform.localPosition;
    }

    public void OnDrag(PointerEventData eventData) {
        if (gc.dice.dice_rolled_this_turn && gc.turn == color && steps != 15) {
            transform.localPosition = GetMouse();
            canvas.sortingOrder = 8;
        }
    }

    public Vector3 GetMouse() {
        return new Vector3(Input.mousePosition.x - cam.pixelWidth/2, Input.mousePosition.y - cam.pixelHeight/2,0);
    }

    public void OnEndDrag(PointerEventData eventData) {
        if (Input.GetMouseButtonUp(1)) {
            Vector3 mouse_offset = GetMouse();
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
        else {
            Debug.Log("Attempted move outside the board.");
        }
    }

    public void Remove(Vector3 available_position) {
        steps = 0;
        transform.localPosition = available_position;
    }
}
