using Firebase;
using Firebase.Auth;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FirebaseAuthManager : MonoBehaviour
{
    [SerializeField] TMP_InputField idField;
    [SerializeField] TMP_InputField pwField;

    [SerializeField] FirebaseUser user;
    [SerializeField] FirebaseAuth auth;


    // Start is called before the first frame update
    void Start()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            var depStatus = task.Result;
            if (depStatus == Firebase.DependencyStatus.Available)
            {
                auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                    "Could not resolve all Firebase dependencies: {0}", depStatus));
            }
        });
    }

    public void Login()
    {
        auth.SignInWithEmailAndPasswordAsync(idField.text, pwField.text).ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("로그인 오류");
                return;
            }
            if (task.IsCanceled)
            {
                Debug.Log("로그인 취소");
                return;
            }

            FirebaseUser registered = task.Result.User;
        });
    }

    public void Register()
    {
        auth.CreateUserWithEmailAndPasswordAsync(idField.text, pwField.text).ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("등록 오류");
                return;
            }
            if (task.IsCanceled)
            {
                Debug.Log("등록 취소");
                return;
            }

            FirebaseUser registeredUser = task.Result.User;
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
