using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.ThirdPerson;

public class Manager : MonoBehaviour
{
    public GameObject player;
    public GameObject ball;
    public GameObject goalObject;
    public GameObject[] spawnPoint = new GameObject[4];
    public GameObject spawnGoal;
    public GameObject firstGoal;
    public Canvas menu;
    public Canvas inGameCanvas;
    public Canvas highScoreMenu;
    public Text readyGo;
    public Text timerText;
    public float timeLeft = 30f;
    public bool gameStarted = false;
    public Text scoreText;
    public float currentScore = 0f;
    public float goalScore = 100f;
    public Text highScoreText;
    public Canvas pauseMenu;
    public HighScores highScores;
    public GameObject manager;


    private void Start()
    {
        highScoreMenu.enabled = false;
        inGameCanvas.enabled = false;
        pauseMenu.enabled = false;
        spawnGoal = Instantiate(goalObject, firstGoal.transform.position, firstGoal.transform.rotation);
        
    }

    private void Update()
    {
        if (gameStarted)
        {
            if (timeLeft <= 0)
            {
                StartCoroutine(GameOver());
                Time.timeScale = 1;
            }
            else
            {
                Timer();
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    Debug.Log("escape key pressed");
                    Time.timeScale = 0;
                    inGameCanvas.enabled = false;
                    pauseMenu.enabled = true;
                }
            }
        }
        
    }

    private IEnumerator GameStart()
    {

        Time.timeScale = 1;
        timeLeft = 30f;
        readyGo.text = "Ready...";
        readyGo.enabled = true;
        Debug.Log("ready text working");
        timerText.text = "30:00";
        Debug.Log("timertext working");
        yield return new WaitForSeconds(3);
        Debug.Log("waitfor working");
        player.GetComponent<ThirdPersonUserControl>().canMove = true;
        Debug.Log("can move working");
        readyGo.text = "Go!";
        yield return new WaitForSeconds(0.5f);
        readyGo.enabled = false;
        gameStarted = true;
        
        
    }


    private IEnumerator ResetPlayer()
    {
        player.GetComponent<ThirdPersonUserControl>().canMove = false;
        Debug.Log("canmove false");
        yield return new WaitForSeconds(2);
        //return the player to start position
        player.transform.position = new Vector3(0, 0, 0);
        player.transform.rotation = new Quaternion(0, 0, 0, 0);
        //return the ball to start position and stopping movement
        ball.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        ball.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        ball.transform.position = new Vector3(0, 0, 1);
        Destroy(spawnGoal);
        Debug.Log("False working");
        timeLeft = 30f;
        Debug.Log("Timeleft working");
        SpawnGoal();
        StartCoroutine(GameStart());
    }



    public void Score()
    {
        currentScore = currentScore + goalScore + (timeLeft * 0.5f);
        scoreText.text = "Score: " + Mathf.RoundToInt(currentScore).ToString();
        
    }

    public void HighScoreList()
    {
        int highScoreOne = manager.GetComponent<HighScores>().scores[0];
        int highScoreTwo = manager.GetComponent<HighScores>().scores[1];
        int highScoreThree = manager.GetComponent<HighScores>().scores[2];
        int highScoreFour = manager.GetComponent<HighScores>().scores[3];
        int highScoreFive = manager.GetComponent<HighScores>().scores[4];

        highScoreText.text = "1. " + highScoreOne + "\n" + "2. " + highScoreTwo + "\n" + "3. " + highScoreThree + "\n" + "4. " + highScoreFour + "\n" + "5. " + highScoreFive;
    }

    private void SpawnGoal()
    {
        int randomSpawn = Random.Range(0, spawnPoint.Length);
        spawnGoal = Instantiate(goalObject, spawnPoint[randomSpawn].transform.position, spawnPoint[randomSpawn].transform.rotation);
    }


    public void ResetLevel()
    {
        StartCoroutine(ResetPlayer());
        Score();
        gameStarted = false;
    }


    public void Timer()
    {
        timeLeft -= Time.deltaTime;
        timerText.text = timeLeft.ToString("F2");
        
            
    }

    public void StartGame()
    {
        player.GetComponent<ThirdPersonUserControl>().canMove = false;
        menu.enabled = false;
        scoreText.text = "Score: 0";
        inGameCanvas.enabled = true;
        StartCoroutine(GameStart());
    }

    
    public void Resume()
    {
        Time.timeScale = 1;
        pauseMenu.enabled = false;
        inGameCanvas.enabled = true;
    }
    public void ExitLevel()
    {
        pauseMenu.enabled = false;
        menu.enabled = true;
        Destroy(spawnGoal);
        spawnGoal = Instantiate(goalObject, firstGoal.transform.position, firstGoal.transform.rotation);
        Time.timeScale = 0;
        //return the player to start position
        player.transform.position = new Vector3(0, 0, 0);
        player.transform.rotation = new Quaternion(0, 0, 0, 0);
        //return the ball to start position and stopping movement
        ball.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        ball.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        ball.transform.position = new Vector3(0, 0, 1);
        gameStarted = false;
        timeLeft = 30f;
        highScores.AddScore(Mathf.RoundToInt(currentScore));
        highScores.SaveScoresToFile();
        currentScore = 0;
        HighScoreList();
    }

    public void BackToMenu()
    {
        highScoreMenu.enabled = false;
        menu.enabled = true;
    }
    public void HighScoreMenu()
    {
        menu.enabled = false;
        highScoreMenu.enabled = true;
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    private IEnumerator GameOver()
    {
        timerText.text = "00:00";
        player.GetComponent<ThirdPersonUserControl>().canMove = false;
        readyGo.text = "Game Over";
        readyGo.enabled = true;
        yield return new WaitForSeconds(3);
        ExitLevel();
        

    }

}
