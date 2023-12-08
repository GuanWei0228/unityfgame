using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] BattleSystem battleSystem;
    [SerializeField] FirebaseManager firebaseManager;

    public GameObject pauseMenu;
    public GameObject WinMenu;
    public GameObject LoseMenu;
    public GameObject logMenu;
    public GameObject noteMenu;
    public GameObject qsnNote;
    public GameObject note;
    public GameObject HealthPoint;
    public GameObject LevelMenu;
    public Canvas minimap;
    public Text questionTextUI;
    public Text answerTextUI;
    public InputField inputField;
    public Text QsnText;
    

    int q;

    void Update()
    {
        
    }

    public void PlayGame() 
    {
        LevelMenu.SetActive(true);
        //SceneManager.LoadScene("MainSampleScene");
        Time.timeScale = 1.0f;
    }
    public void ChooseEasy()
    {
        PlayerPrefs.SetString("PlayerName", "Easy");
        SceneManager.LoadScene("MainSampleScene");
    }
    public void ChooseMedium()
    {
        PlayerPrefs.SetString("PlayerName", "Medium");
        SceneManager.LoadScene("MainSampleScene");
    }
    public void ChooseHard()
    {
        PlayerPrefs.SetString("PlayerName", "Hard");
        SceneManager.LoadScene("MainSampleScene");
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

    public void OpenNoteMenu()
    {
        minimap.gameObject.SetActive(false);
        HealthPoint.SetActive(false);
        noteMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void CloseNoteMenu()
    {
        HealthPoint.SetActive(true);
        noteMenu.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public void OpenQsnNote()
    {
        //minimap.gameObject.SetActive(false);
        HealthPoint.SetActive(false);
        qsnNote.SetActive(true);
        QsnText.text = "Q: " + firebaseManager.Qsn + "\n" + "A: " + firebaseManager.Ans;
        Time.timeScale = 0f;
    }

    public void CloseQsnNote()
    {
        HealthPoint.SetActive(true);
        qsnNote.SetActive(false);
        
    }

    public void OpenNote()
    {
        //minimap.gameObject.SetActive(false);
        HealthPoint.SetActive(false);
        note.SetActive(true);
        inputField.text = firebaseManager.snap;
        Time.timeScale = 0f;
    }

    public void CloseNote()
    {
        HealthPoint.SetActive(true);
        note.SetActive(false);
        firebaseManager.SaveData(inputField.text);
        inputField.text = null;

    }

    public void SaveQA()
    {
        firebaseManager.SaveQ(questionTextUI.text);
        firebaseManager.SaveA(answerTextUI.text);
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
