using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameHistory : MonoBehaviour
{
    public TextMeshProUGUI infoTextMode;
    public TextMeshProUGUI infoTextPoints;

    void Start()
    {
        var (vsAI, points1, points2) = SaveSystem.LoadGame();

        if (points1 == 0 && points2 == 0)
        {
            infoTextMode.text = "";
            infoTextPoints.text = "";
        }
        else
        {
            string mode = vsAI ? "1P" : "2P";
            string winner = points1 > points2 ? "WIN: 1P" :
                            points2 > points1 ? "WIN: 2P" : "DRAW";

            infoTextMode.text = $"({mode})\n";

            if (points1 > points2)
            {
                infoTextPoints.text = $"{points1}(W) : {points2}\n";
            }
            else if (points1 < points2)
            {
                infoTextPoints.text = $"{points1} : {points2}(W)\n";
            }
            else if (points1 == points2)
            {
                infoTextPoints.text = $"{points1} : {points2}\n";
            }


        }
    }
}
