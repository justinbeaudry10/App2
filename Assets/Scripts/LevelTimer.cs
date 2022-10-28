using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelTimer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    bool timeRunning = true;
    float time = 0;

    // Update is called once per frame
    void Update()
    {
        if(timeRunning)
        {
            time += Time.deltaTime;
        }

        timerText.text = time.ToString("F2");
    }

    public void toggleTimer()
    {
        timeRunning = !timeRunning;
    }

    public float getTime()
    {
        return time;
    }
}
