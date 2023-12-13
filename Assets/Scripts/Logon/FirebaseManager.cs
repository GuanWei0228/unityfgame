using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Firebase;
using Firebase.Database;
using System.Threading.Tasks;
using UnityEngine.UI;
using static UnityEngine.UIElements.UxmlAttributeDescription;


public class FirebaseManager : MonoBehaviour
{
    // Start is called before the first frame update

    public Firebase.Auth.FirebaseAuth auth;
    public Firebase.Auth.FirebaseUser user;

    private float loadDataTimer = 0f;
    private float loadDataInterval = 2f; // Adjust the interval as needed


    public int rCheck;
    public int lCheck;
    public string snap = "123";
    public string Qsn = "123";
    public string Ans = "123";

    void Start()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        if (auth == null)
        {
            Debug.LogError("Firebase auth is not initialized properly.");
        }
        else
        {
            auth.StateChanged += AuthStateChanged;
        }

    }

    // Update is called once per frame
    void Update()
    {
        LoadData();
        LoadAns();
        LoadQsn();

    }

    public void Register(string email, string password) {
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                rCheck = 1;
                return ;
            }
            if (task.IsCanceled)
            {
                return;
            }
            if (task.IsCompletedSuccessfully)
            {
                rCheck = 2;
                print("Registered");
            }
        });
    }

    public async void Login(string email, string password) {
        await auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsFaulted){
                lCheck = 1;
                return;
            }
            if(task.IsCompletedSuccessfully) {
                lCheck = 2;
                print("Login!!");
            }
        });
    }

    public void Logout() {
        auth.SignOut();
    }
    void AuthStateChanged(object sender, System.EventArgs eventArgs) {
        if (auth.CurrentUser != user) {
            user = auth.CurrentUser;
            if (user != null) {
                print($"Login - {user.Email}");
            }
        }
    }

    public void SaveData(string data) {
        if (user != null)
        {
            DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
            reference.Child(user.UserId).Child("Note").Child("MyNote").SetValueAsync(data).ContinueWith(task =>
            {
                if (task.IsCompletedSuccessfully)
                {
                    print("saved!");
                    return;
                }
            });
        }
        else {
            print("No user!");
            return;
        }
        
    }

    public void SaveQ(string data)
    {
        if (user != null)
        {
            DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
            reference.Child(user.UserId).Child("Note").Child("QsnNote").Child("Qsn").SetValueAsync(data).ContinueWith(task =>
            {
                if (task.IsCompletedSuccessfully)
                {
                    print("saved!");
                    return;
                }
            });
            
        }
        else
        {
            print("No user!");
            return;
        }

    }

    public void SaveA(string data)
    {
        if (user != null)
        {
            DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
            reference.Child(user.UserId).Child("Note").Child("QsnNote").Child("Ans").SetValueAsync(data).ContinueWith(task =>
            {
                if (task.IsCompletedSuccessfully)
                {
                    print("saved!");
                    return;
                }
            });

        }
        else
        {
            print("No user!");
            return;
        }

    }


    public void LoadData() {
        if (user != null)
        {
            StartCoroutine(LoadDataCoroutine());
            
            
        }
        else {
            print("No user.");
        }

    }

    public void LoadQsn()
    {
        if (user != null)
        {
            StartCoroutine(LoadQsnCoroutine());
            

        }
        else
        {
            print("No user.");
        }

    }

    public void LoadAns()
    {
        if (user != null)
        {
            StartCoroutine(LoadAnsCoroutine());
            

        }
        else
        {
            print("No user.");
        }

    }

    private IEnumerator LoadDataCoroutine()
    {
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        var task = reference.Child(user.UserId).Child("Note").Child("MyNote").GetValueAsync();

        yield return new WaitUntil(() => task.IsCompleted);

        if (task.Exception != null)
        {
            print(task.Exception);
            yield break;
        }

        DataSnapshot snapshot = task.Result;
        snap = snapshot.Value.ToString();
        //inputNote.text = snapshot.Value.ToString();
    }

    private IEnumerator LoadQsnCoroutine()
    {
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        var task = reference.Child(user.UserId).Child("Note").Child("QsnNote").Child("Qsn").GetValueAsync();

        yield return new WaitUntil(() => task.IsCompleted);

        if (task.Exception != null)
        {
            print(task.Exception);
            yield break;
        }

        DataSnapshot snapshot = task.Result;
        Qsn = snapshot.Value.ToString();
        //inputNote.text = snapshot.Value.ToString();
    }

    private IEnumerator LoadAnsCoroutine()
    {
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        var task = reference.Child(user.UserId).Child("Note").Child("QsnNote").Child("Ans").GetValueAsync();

        yield return new WaitUntil(() => task.IsCompleted);

        if (task.Exception != null)
        {
            print(task.Exception);
            yield break;
        }

        DataSnapshot snapshot = task.Result;
        Ans = snapshot.Value.ToString();
        //inputNote.text = snapshot.Value.ToString();
    }


    public DatabaseReference GetUserReference() {
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        return reference.Child("Users").Child(user.UserId);
    }

    void OnDestroy(){
        auth.StateChanged -= AuthStateChanged;
    }


}



public class GameRecord {
    public string datetime;
    public int score;
    public GameRecord(int score){
        this.datetime = DateTime.Now.ToString();
        this.score = score;
    }
}