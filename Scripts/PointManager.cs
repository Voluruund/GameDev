using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PointManager : MonoBehaviour
{
    public static PointManager instance;

    public int points = 0;
    public TMP_Text pointsText;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        if(pointsText != null)
        {
            pointsText.text = "Points: " + points;
        }
    }

    public void UpdatePti(int up)
    {
        points += up;
        pointsText.text = "Points: " + points;

    }
    public float getPti()
    {
        return points;
    }
}
