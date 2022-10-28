using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ScoresManager : MonoBehaviour
{
    [System.Serializable]
    public struct Score
    {
        public string name;
        public float time;
    }

    [System.Serializable]
    public class Scores
    {
        public List<Score> leaderboard = new List<Score>();
    }

    public Scores LoadScores()
    {
        if (!File.Exists(Path()))
        {
            File.Create(Path()).Dispose();
            return new Scores();
        }

        using StreamReader reader = new StreamReader(Path());
        string data = reader.ReadToEnd();
        return JsonUtility.FromJson<Scores>(data);
        
    }

    public void SaveScores(Scores scores)
    {
        using StreamWriter writer = new StreamWriter(Path());
        string data = JsonUtility.ToJson(scores, true);
        writer.Write(data);
    }

    public void AddScore(Score score)
    {
        Scores scores = LoadScores();
        bool scoreAdded = false;

        for(int i = 0; i < scores.leaderboard.Count; i++)
        {
            if(score.time < scores.leaderboard[i].time)
            {
                scores.leaderboard.Insert(i, score);
                scoreAdded = true;
                break;
            }
        }

        if(!scoreAdded)
        {
            scores.leaderboard.Add(score);
        }

        SaveScores(scores);
    }

    private string Path()
    {
        return Application.streamingAssetsPath + "/scores.json";
    }
}
