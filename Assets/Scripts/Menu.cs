using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor.Experimental.GraphView;

public class Menu : MonoBehaviour
{
    [SerializeField] BattleSystem battleSystem;
    public GameObject pauseMenu;
    public GameObject WinMenu;
    public GameObject LoseMenu;
    public GameObject logMenu;
    public GameObject HealthPoint;
    public Canvas minimap;
    public Text questionTextUI;
    public Text answerTextUI;
    

    int q;

    public void PlayGame() 
    {
        SceneManager.LoadScene("MainSampleScene");
        Time.timeScale = 1.0f;
    }
    public void ReStart()
    {
        SceneManager.LoadScene("MainSampleScene");
        Time.timeScale = 1.0f;
    }
    public void GoMenu()
    {
        SceneManager.LoadScene("menu");
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public void PauseGame() 
    {
        minimap.gameObject.SetActive(false);
        pauseMenu.SetActive(true);
        HealthPoint.SetActive(false);
        Time.timeScale = 0f;
    }
    public void ContinueGame()
    {
        pauseMenu.SetActive(false);
        HealthPoint.SetActive(true);
        Time.timeScale = 1.0f;
    }

    public void OpenLog()
    {
        minimap.gameObject.SetActive(false);
        HealthPoint.SetActive(false);
        logMenu.SetActive(true);
        q = 0;
        ShowNextQuestion();
        Time.timeScale = 0f;
    }

    public void CloseLog()
    {
        HealthPoint.SetActive(true);
        logMenu.SetActive(false);
        q = 0;
        Time.timeScale = 1.0f;
    }

    public void Previous() 
    {
        
        if(q > 0 ) 
        {
            q -= 1;
        }
        ShowNextQuestion();
    }

    public void Next()
    {
        if (q < battleSystem.questionAnswerPairs.Count-1)
        {
            q += 1;
        }
        ShowNextQuestion();
    }

    void ShowNextQuestion()
    {
        if (battleSystem.questionAnswerPairs.Count > 0 && q < battleSystem.questionAnswerPairs.Count)
        {
            Tuple<string, string> questionAnswerPair = battleSystem.questionAnswerPairs[q];
            string questionText = questionAnswerPair.Item1;
            string answerText = questionAnswerPair.Item2;

            questionTextUI.text = questionText;
            answerTextUI.text = answerText;

        }
    }
}
