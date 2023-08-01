using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dice : MonoBehaviour
{
    public GameController gc;
    public Text dice_text;
    public bool dice_rolled_this_turn = false;
    public int roll;
    //private int i = 0;
    private int[] predictable_dice = {0,0,2,4,4,3,3,4};
    public bool clearing = false;
    public float time;

    public void FixedUpdate() {
        if (clearing) {
            if (Time.time - time > 0.5) {
                EndClear();
            }
        }
    }

    public void DiceResult() {
        if (!dice_rolled_this_turn) {
            roll = GenerateNumber();
            dice_rolled_this_turn = true;

            dice_text.text = roll.ToString();
            gc.RefreshPossibleMoves();

            Debug.Log("Dice rolled. Result: " + roll.ToString());
        }
    }

    public int GenerateNumber() {
        int result = 0;
        
        // Random dice
        for (int j = 0; j<4; j++) {
            result += Random.Range(0,2);
        }
        return result;

        // Predictable dice
        // result = predictable_dice[i];
        // if (i == predictable_dice.Length - 1) {
        //     i = 0;
        // }
        // else {
        //     i++;
        // }
        // return result;
    }

    public void StartClear() {
        clearing = true;
        time = Time.time;
    }

    public void EndClear() {
        clearing = false;
        dice_rolled_this_turn = false;
        dice_text.text = "";
        Debug.Log("Dice cleared.");
    }
}
