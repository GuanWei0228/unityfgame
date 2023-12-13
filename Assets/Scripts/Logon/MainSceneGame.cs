using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainSceneGame : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    FirebaseManager firebaseManager;

    [SerializeField]
    GameObject loginFailed;

    //[SerializeField]
    //Text textEmail;
    //[SerializeField]
    //InputField inputNote;

    void Start()
    {
        if (firebaseManager == null)
        {
            Debug.LogError("Firebase manager is not assigned in the inspector.");
            return;
        }

        firebaseManager.auth.StateChanged += AuthStateChanged;

    }

    // Update is called once per frame
    void Update()
    {

        if (firebaseManager.user == null)
        {
            loginFailed.SetActive(true);
        }
        
    }


    public void BackToMenu() {
        loginFailed.SetActive(false);
        SceneManager.LoadScene("Menu");
    }

    public void SaveNote()
    {
        //firebaseManager.SaveData(inputNote.text);
    }

    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (firebaseManager.user == null)
        {
            //textEmail.text = "";
            
            //panelInfo.SetActive(false);
        }
        else
        {
            //textEmail.text = firebaseManager.user.Email;
            //StartCoroutine(LoadNoteTask());
            //panelInfo.SetActive(true);
        }
    }

    void OnDestroy()
    {
        firebaseManager.auth.StateChanged -= AuthStateChanged;
    }

    IEnumerator LoadNoteTask()
    {
        var task = firebaseManager.GetUserReference().Child("note").GetValueAsync();
        yield return new WaitUntil(() => task.IsCompleted);
        DataSnapshot snapshot = task.Result;
        if (snapshot.Value != null)
        {
            string note = snapshot.Value.ToString();
            print(note);
            //inputNote.text = note;
        }
        else
        {
            print("No Note!!!!");
            //inputNote.text = "";
        }

    }
}
