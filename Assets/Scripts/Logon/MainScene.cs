using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainScene : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    FirebaseManager firebaseManager;
    [SerializeField]
    InputField inputEmail;
    [SerializeField]
    InputField inputPassword;

    [SerializeField]
    GameObject panelLogin;

    [SerializeField]
    GameObject logOut;

    [SerializeField]
    GameObject loginFail;

    [SerializeField]
    GameObject registerFail;

    [SerializeField]
    GameObject registerSuccess;
    //[SerializeField]
    //GameObject panelInfo;

    //[SerializeField]
    //Text textEmail;
    //[SerializeField]
    //InputField inputNote;

    void Start(){
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
        if (Input.GetKeyDown(KeyCode.S)) {
            //firebaseManager.SaveData(inputNote.text);
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            //firebaseManager.SaveAnswer(inputNote.text);
        }
        //if (Input.GetKeyDown(KeyCode.Z))
        //{
        //    firebaseManager.LoadData(); 
        //    //StartCoroutine(LoadNoteTask());
        //}
        if (Input.GetKeyDown(KeyCode.T)) {
            firebaseManager.TestSecurityRule();
        }
    }

    public void Register() {
        firebaseManager.Register(inputEmail.text, inputPassword.text);
        if (firebaseManager.rCheck == 1) {
            loginFail.SetActive(false);
            registerFail.SetActive(true);
            registerSuccess.SetActive(false);
            firebaseManager.rCheck = 0;
        }
        if (firebaseManager.rCheck == 2)
        {
            loginFail.SetActive(false);
            registerFail.SetActive(false);
            registerSuccess.SetActive(true);
            firebaseManager.rCheck = 0;
        }
    }

    public void Login() { 
        firebaseManager.Login(inputEmail.text, inputPassword.text);

        if (firebaseManager.lCheck == 0)
        {
            loginFail.SetActive(true);
            registerFail.SetActive(false);
            registerSuccess.SetActive(false);
            firebaseManager.lCheck = 0;
        }
        if (firebaseManager.lCheck == 2)
        {
            loginFail.SetActive(false);
            registerFail.SetActive(false);
            registerSuccess.SetActive(false);
            firebaseManager.lCheck = 0;
        }
    }

    public void Logout()
    {
        firebaseManager.Logout();
        //inputNote.text = "";
    }
    public void SaveNote() {
        //firebaseManager.SaveData(inputNote.text);
    }

    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (firebaseManager.user == null)
        {
            //textEmail.text = "";
            panelLogin.SetActive(true);
            logOut.SetActive(false);
            //panelInfo.SetActive(false);
        }
        else {
            //textEmail.text = firebaseManager.user.Email;
            //StartCoroutine(LoadNoteTask());
            panelLogin.SetActive(false);
            logOut.SetActive(true);
            //panelInfo.SetActive(true);
        }
    }

    void OnDestroy()
    {
        firebaseManager.auth.StateChanged -= AuthStateChanged;
    }

    IEnumerator LoadNoteTask() {
        var task = firebaseManager.GetUserReference().Child("note").GetValueAsync();
        yield return new WaitUntil(() => task.IsCompleted);
        DataSnapshot snapshot = task.Result;
        if (snapshot.Value != null){
            string note = snapshot.Value.ToString();
            print(note);
            //inputNote.text = note;
        }
        else {
            print("No Note!!!!");
            //inputNote.text = "";
        }
        
    }
}
