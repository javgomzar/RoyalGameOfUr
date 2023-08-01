using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Color=GameColor.Color;


public class Board : MonoBehaviour
{
    public List<Piece> pieces;
    public static int[] rosettas = {4,8,14};
    public static int center_rosetta = 8;
    public List<Vector3> available_black = new List<Vector3>();
    public List<Vector3> available_white = new List<Vector3>();
    public float black_shift;
    public float white_shift;
    public float stack_distance;
    public int black_stack_depth;
    public int white_stack_depth;
    // Start is called before the first frame update
    void Start()
    {
        pieces = (new List<Piece>(GameObject.Find("Black").GetComponentsInChildren<Piece>()));
        pieces.AddRange(GameObject.Find("White").GetComponentsInChildren<Piece>());

        stack_distance = 10;
    }

    public Piece GetPiece(int steps, Color color) {
        foreach (Piece piece in pieces) {
            if (piece.steps == steps && piece.color == color) {
                return piece;
            }
        }
        return null;
    }

    public List<Piece> GetPieces(Color color) {
        List<Piece> result = new List<Piece>();
        foreach (Piece piece in pieces) {
            if (piece.color == color) {
                result.Add(piece);
            }
        }
        return result;
    }

    public void FreeAvailableSpot(Piece piece) {
        List<Vector3> available_spots = piece.color == Color.Black? available_black : available_white;
        
        available_spots.Add(piece.initial_position);
    }

    public void RemoveFromBoard(Piece piece) {
        if (piece.color == Color.Black) {
            piece.Remove(available_black[0]);
            available_black.RemoveAt(0);
        }
        else if (piece.color == Color.White) {
            piece.Remove(available_white[0]);
            available_white.RemoveAt(0);
        }
    }

    public void Stack(Piece piece) {
        if (piece.color == Color.Black) {
            piece.transform.localPosition = Board.StepsToPosition(15, piece.color) + new Vector3(0, black_shift, 0);
            piece.canvas.sortingOrder = black_stack_depth;
            
            black_shift += stack_distance;
            black_stack_depth += 1;
        }
        else {
            piece.transform.localPosition = Board.StepsToPosition(15, piece.color) + new Vector3(0, white_shift, 0);
            piece.canvas.sortingOrder = white_stack_depth;
            
            white_shift += stack_distance;
            white_stack_depth += 1;
        }
    }

    public static int PositionToSteps(Vector3 position) {
        if (Mathf.Abs(position.x) < 30) {
            return Mathf.FloorToInt((240 - position.y) / 60) + 5;
        }
        else if (position.y > 0) {
            return Mathf.FloorToInt(position.y / 60) + 1;
        }
        else if (position.y < -120) {
            return Mathf.FloorToInt((position.y + 240) / 60) + 13;
        }
        else {
            return 15;
        }
    }

    public static Vector3 StepsToPosition(int steps, Color color) {
        Vector3 result = new Vector3(0,0,0);
        if (steps >= 0 && steps <= 4) {
            result.x = color == Color.Black? 60 : -60;
            result.y = 30 + 60 * (steps-1);
        }
        else if (steps >= 5 && steps <= 12) {
            result.x = 0;
            result.y = 210 - 60 * (steps - 5);
        }
        else if (steps >= 13 && steps <= 15) {
            result.x = color == Color.Black? 60 : -60;
            result.y = -210 + 60 * (steps - 13);
        }
        return result;
    }

    public static bool IsValidSquare(Vector3 square, Color color) {        
        // Board rectangle
        if (Mathf.Abs(square.x) > 90 | Mathf.Abs(square.y) > 240) {
            return false;
        }

        // Board gaps
        // if (Mathf.Abs(square.x) > 30 && Mathf.Abs(square.y + 30) < 30) {
        //     return false;
        // }
        
        if (color == Color.Black) {
            return square.x > -30;
        }
        else if (color == Color.White) {
            return square.x < 30;
        }

        return false;
    }
}
