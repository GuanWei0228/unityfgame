using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] BattleSystem battleSystem;
    public GameObject pauseMenu;
    public GameObject logMenu;
    public Text questionTextUI;
    public Text answerTextUI;

    int q;

    public void PlayGame() 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public void PauseGame() 
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }
    public void ContinueGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public void OpenLog()
    {
        logMenu.SetActive(true);
        q = 0;
        ShowNextQuestion();
        Time.timeScale = 0f;
    }

    public void CloseLog()
    {
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
