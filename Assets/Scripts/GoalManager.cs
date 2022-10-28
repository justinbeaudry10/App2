using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalManager : MonoBehaviour
{
    public GameObject winMsg;
    public LevelTimer timer;
    public ScoresManager scoresManager;

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController controller = other.GetComponent<PlayerController>();

        if (controller != null)
        {
            timer.toggleTimer();

            ScoresManager.Score score = new ScoresManager.Score();
            score.name = PlayerPrefs.GetString("name");
            score.time = timer.getTime();

            scoresManager.AddScore(score);

            winMsg.SetActive(true);

            StartCoroutine(RestartGame());
        }
    }

    IEnumerator RestartGame()
    {
        yield return new WaitForSeconds(2);

        SceneManager.LoadScene(0);
    }
}
