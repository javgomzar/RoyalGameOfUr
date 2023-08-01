using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Restart : MonoBehaviour
{
    public GameController gc;
    public Canvas sfp_canvas;
    // Start is called before the first frame update
    void Start()
    {
        gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        sfp_canvas = GameObject.Find("StartingPlayerMenuCanvas").GetComponent<Canvas>();
    }
 
    public void RestartGame() {
        foreach (Piece piece in gc.board.pieces) {
            if (piece.steps != 0) {
                gc.board.RemoveFromBoard(piece);
            }
            piece.canvas.sortingOrder = 1;
        }

        gc.win_canvas.enabled = false;
        gc.board.black_stack_depth = 0;
        gc.board.white_stack_depth = 0;
        gc.board.available_black = new List<Vector3>();
        gc.board.available_white = new List<Vector3>();
        gc.board.black_shift = 0;
        gc.board.white_shift = 0;

        sfp_canvas.enabled = true;
    }
}
