using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Color=GameColor.Color;

public class Tests : MonoBehaviour
{
    // Start is called before the first frame update
    public List<Vector3> black_positions_results;
    public List<Vector3> white_positions_results;
    public List<int> black_steps_results;
    public List<int> white_steps_results;
    void Start()
    {
        black_positions_results = new List<Vector3>();
        white_positions_results = new List<Vector3>();

        for (var i = 1; i < 16; i++) {
            Vector3 position = Board.StepsToPosition(i, Color.Black);
            black_positions_results.Add(position);
            black_steps_results.Add(Board.PositionToSteps(position));

            position = Board.StepsToPosition(i, Color.White);
            white_positions_results.Add(position);
            white_steps_results.Add(Board.PositionToSteps(position));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
