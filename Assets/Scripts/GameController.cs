using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public enum PlayMode {
    cpu,
    twoPlayers
}

public class GameController : MonoBehaviour
{
    public PlayMode play_mode;
    public Dice dice;
    public Piece.Color turn;

    public List<Piece> possible_moves = new List<Piece>();
    public Canvas win_canvas;
    public Canvas n_players_canvas;
    public Text win_text;
    public Fade fade;
    public Board board;
    public List<Piece.Color> ai_players;

    // Start is called before the first frame update
    void Start()
    {
        win_text = win_canvas.GetComponentInChildren<Text>();
        fade = GameObject.Find("BlackBackground").GetComponent<Fade>();
        board = GameObject.Find("Board").GetComponent<Board>();
        ai_players = new List<Piece.Color>();
    }

    public bool ValidateMove(Piece piece) {
        int new_steps = piece.steps + dice.roll;
        if (new_steps <= 15 & dice.dice_rolled_this_turn & turn == piece.color & piece.steps != 15) {
            if (new_steps == 15) {
                return true;
            }
            
            // You can't take a piece that is yours
            Piece taken_piece = board.GetPiece(new_steps, piece.color);
            if (taken_piece != null) {
                return false;
            }
            
            // You can't take a piece in the center rosetta
            taken_piece = board.GetPiece(new_steps, Piece.OppositeColor(piece.color));
            if (taken_piece != null) {
                return taken_piece.steps != Board.center_rosetta;
            }

            return true;
        }
        return false;
    }

    public void RefreshPossibleMoves() {
        possible_moves = new List<Piece>();

        if (dice.roll != 0) {
            foreach (Piece piece in board.GetPieces(turn)) {
                if (ValidateMove(piece)) {
                    possible_moves.Add(piece);
                }
            }
        }
        if (possible_moves.Count == 0) {
            NextTurn(null);
        }
    }

    public void ExecuteMove(Piece piece) {
        // New pieces leave a free spot behind
        if (piece.steps == 0) {
            board.FreeAvailableSpot(piece);
        }

        int new_steps = piece.steps + dice.roll;

        // Eating rule
        if (new_steps > 4 && new_steps < 13) {
            Piece taken_piece = board.GetPiece(new_steps, Piece.OppositeColor(piece.color));
            if (taken_piece != null) {
                board.RemoveFromBoard(taken_piece);
            }
        }
        
        // Stacking pieces when they arrive at the end
        if (new_steps == 15) {
            piece.steps = 15;
            board.Stack(piece);
        }
        // Moving the piece
        else {
            piece.canvas.sortingOrder = 1;
            piece.steps += dice.roll; 
            piece.transform.localPosition = Board.StepsToPosition(new_steps, piece.color);
        }

        NextTurn(piece);
    }

    public void NextTurn(Piece piece_moved) {
        // Win condition
        if (board.black_stack_depth == 7) {
            Win(Piece.Color.Black);
            return;
        }
        else if (board.white_stack_depth == 7) {
            Win(Piece.Color.White);
            return;
        }

        // // DEBUG win condition 
        // if (board.black_stack_depth == 0) {
        //     Win(Piece.Color.Black);
        // }
        // else if (board.white_stack_depth == 0) {
        //     Win(Piece.Color.White);
        // }

        if (piece_moved != null) {
            // If you fall on a rosetta, you can roll the dice again!
            if (!Board.rosettas.Contains(piece_moved.steps)) {
                turn = Piece.OppositeColor(turn);
            }
        }
        else {
            turn = Piece.OppositeColor(turn);
        }
        
        Debug.Log("Next turn: " + (turn == Piece.Color.Black? "Black" : "White") + ".");
        dice.StartClear();
        possible_moves = new List<Piece>();

        // while (dice.clearing) { 

        // }

        if (ai_players.Contains(turn)) {
            dice.DiceResult();
            ExecuteMove(possible_moves[0]);
            NextTurn(possible_moves[0]);
        }
    }

    public void Win(Piece.Color color) {
        win_text.text = System.Enum.GetName(typeof(Piece.Color), color) + " wins!";
        win_canvas.enabled = true;
        dice.StartClear();
        FadeOut();
    }

    public void SetModeCPU() {
        play_mode = PlayMode.cpu;
        n_players_canvas.enabled = false;
    }

    public void SetMode2Players() {
        play_mode = PlayMode.twoPlayers;
        n_players_canvas.enabled = false;
    }

    public void FadeIn() {
        fade.StartFadeIn();
    }

    public void FadeOut() {
        fade.StartFadeOut();
    }
}
