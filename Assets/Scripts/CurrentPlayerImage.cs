using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentPlayerImage : MonoBehaviour
{
    public GameController gc;
    public Image img;
    public Sprite white;
    public Sprite black;
    // Start is called before the first frame update
    void Start()
    {
        gc = GameObject.Find("GameController").GetComponent<GameController>();
        img = GetComponent<Image>();

        black = Resources.Load<Sprite>("Images/BlackPlayer");
        white = Resources.Load<Sprite>("Images/WhitePlayer");
    }

    // Update is called once per frame
    void Update()
    {
        switch(gc.turn) {
            case Piece.Color.Black:
                img.sprite = black;
            break;
            case Piece.Color.White:
                img.sprite = white;
            break;
            default:
                img.sprite = null;
            break;
        }
    }
}
