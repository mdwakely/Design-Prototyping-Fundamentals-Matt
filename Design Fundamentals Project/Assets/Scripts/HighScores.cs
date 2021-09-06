using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class HighScores : MonoBehaviour
{

    public int[] scores = new int[5];
    string currentDirectory;
    public string scoreFileName = "Highscores.txt";
    public GameObject manager;
    
    // Start is called before the first frame update
    void Start()
    {
        currentDirectory = Application.dataPath;
        LoadScoresFromFile();
        manager.GetComponent<Manager>().HighScoreList();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadScoresFromFile()
    {
        bool fileExists = File.Exists(currentDirectory + "\\" + scoreFileName);
        scores = new int[scores.Length];
        StreamReader fileReader = new StreamReader(currentDirectory + "\\" + scoreFileName);
        int scoreCount = 0;
        while (fileReader.Peek() != 0 && scoreCount < scores.Length)
        {
            string fileLine = fileReader.ReadLine();
            int readScore = -1;
            bool didParse = int.TryParse(fileLine, out readScore);
            if (didParse)
            {
                scores[scoreCount] = readScore;
            }
            else
            {
                scores[scoreCount] = 0;
            }
            scoreCount++;
        }
        fileReader.Close();
    }

    public void SaveScoresToFile()
    {
        StreamWriter fileWriter = new StreamWriter(currentDirectory + "\\" + scoreFileName);
        for (int i = 0; i < scores.Length; i++)
        {
            fileWriter.WriteLine(scores[i]);
        }
        fileWriter.Close();
    }


    public void AddScore(int newScore)
    {
        int desiredIndex = -1;
        for (int i = 0; i < scores.Length; i++)
        {
            if (scores[i]< newScore || scores[i] == 0)
            {
                desiredIndex = i;
                break;
            }
        }

        if (desiredIndex < 0)
        {
            return;
        }

        for (int i = scores.Length - 1; i > desiredIndex; i--)
        {
            scores[i] = scores[i - 1];
        }

        scores[desiredIndex] = newScore;
    }


}
