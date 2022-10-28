using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Leaderboard : MonoBehaviour
{
    public GameObject rowPrefab;
    public ScoresManager scoresManager;

    void Start()
    {
        ScoresManager.Scores leaders = scoresManager.LoadScores();
        
        for(int i = 0; i < leaders.leaderboard.Count; i++)
        {
            GameObject rowGameObject = Instantiate(rowPrefab);
            rowGameObject.transform.SetParent(this.gameObject.transform, false);
            rowGameObject.transform.Find("Name").GetComponent<TMP_Text>().text = leaders.leaderboard[i].name;
            rowGameObject.transform.Find("Score").GetComponent<TMP_Text>().text = leaders.leaderboard[i].time.ToString("0.##");

        }

        gameObject.GetComponentInParent<ScrollRect>().verticalNormalizedPosition = 1;
   
    }

}
