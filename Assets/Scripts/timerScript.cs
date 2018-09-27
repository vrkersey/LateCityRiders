using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class timerScript : MonoBehaviour {

    public float startTime;
    private float timeRemaining;
    private Text timeDisplay;

    // Use this for initialization
    void Start()
    {
        timeDisplay = GameObject.Find("Time").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if( !GameObject.FindGameObjectWithTag("Player").GetComponent<playerController>().win)
        timeRemaining = startTime >= 0 ? startTime + Time.timeSinceLevelLoad : 0;
        int minutes = (int)timeRemaining / 60;
        int seconds = (int)timeRemaining % 60;

        string textTime = string.Format("{0:00} : {1:00}", minutes, seconds);
        timeDisplay.text = textTime;
    }
}
