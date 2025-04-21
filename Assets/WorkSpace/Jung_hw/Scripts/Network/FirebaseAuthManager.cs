using Firebase;
using Firebase.Auth;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FirebaseAuthManager : MonoBehaviourPunCallbacks
{
    [SerializeField] Color failTextColor;
    [SerializeField] Color successTextColor;

    [Header("Login")]
    [SerializeField] TMP_InputField idField;
    [SerializeField] TMP_InputField pwField;
    [SerializeField] TMP_Text loginNotifyText;

    [Header("Register")]
    [SerializeField] GameObject registerPanel;
    [SerializeField] TMP_InputField idRegiField;
    [SerializeField] TMP_InputField pwRegiField;
    [SerializeField] TMP_InputField pwCheckRegiField;
    [SerializeField] TMP_InputField nicknameRegiField;
    [SerializeField] TMP_Text regiNotifyText;

    public static FirebaseUser user;
    public FirebaseAuth auth;


    // Start is called before the first frame update
    void Awake()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            var depStatus = task.Result;
            if (depStatus == DependencyStatus.Available)
            {
                auth = FirebaseAuth.DefaultInstance;
            }
            else
            {
                Debug.LogError(string.Format(
                    "Could not resolve all Firebase dependencies: {0}", depStatus));
            }
        });
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void SetRegisterpanel(bool state)
    {
        registerPanel.SetActive(state);
    }

    public void Login()
    {
        StartCoroutine(LoginCor(idField.text, pwField.text));
    }

    IEnumerator LoginCor(string id, string pw)
    {
        Task<AuthResult> LoginTask = auth.SignInWithEmailAndPasswordAsync(id, pw);

        yield return new WaitUntil(predicate: () => LoginTask.IsCompleted);

        if (LoginTask.Exception != null)
        {
            Debug.LogWarning(message: "다음과 같은 이유로 로그인 실패:" + LoginTask.Exception);

            //파이어베이스에선 에러를 분석할 수 있는 형식을 제공
            FirebaseException firebaseEx = LoginTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

            string message = "";
            switch (errorCode)
            {
                case AuthError.MissingEmail:
                    message = "이메일 누락";
                    break;
                case AuthError.MissingPassword:
                    message = "패스워드 누락";
                    break;
                case AuthError.WrongPassword:
                    message = "패스워드 틀림";
                    break;
                case AuthError.InvalidEmail:
                    message = "이메일 형식이 옳지 않음";
                    break;
                case AuthError.UserNotFound:
                    message = "아이디가 존재하지 않음";
                    break;
                default:
                    message = "관리자에게 문의 바랍니다";
                    break;
            }
            loginNotifyText.color = failTextColor;
            loginNotifyText.text = message;
        }
        else// 그렇지 않다면 로그인
        {
            user = LoginTask.Result.User; //유저 정보 기억
            loginNotifyText.text = "";
            idField.text = user.DisplayName;
            loginNotifyText.color = successTextColor;
            loginNotifyText.text = "로그인 완료, 반갑습니다 " + user.DisplayName + "님";
            ConnectToServer();
        }
    }

    public void Register()
    {
        if (pwRegiField.text != pwCheckRegiField.text)
        {
            Debug.Log("비밀번호 틀림");
            return;
        }
        StartCoroutine(Register(idRegiField.text, pwRegiField.text, nicknameRegiField.text));
    }

    IEnumerator Register(string id, string pw, string name)
    {
        if (name == "")
        {
            regiNotifyText.color = failTextColor;
            regiNotifyText.text = "닉네임 미기입";
        }
        else
        {
            var RegisterTask = auth.CreateUserWithEmailAndPasswordAsync(id, pw);

            yield return new WaitUntil(predicate: () => RegisterTask.IsCompleted);

            if (RegisterTask.Exception != null)
            {
                Debug.LogWarning(message: "실패 사유" + RegisterTask.Exception);
                FirebaseException firebaseEx = RegisterTask.Exception.GetBaseException() as FirebaseException;
                AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

                string message = "회원가입 실패";
                switch (errorCode)
                {
                    case AuthError.MissingEmail:
                        message = "이메일 누락";
                        break;
                    case AuthError.MissingPassword:
                        message = "패스워드 누락";
                        break;
                    case AuthError.WeakPassword:
                        message = "패스워드 약함";
                        break;
                    case AuthError.EmailAlreadyInUse:
                        message = "중복 이메일";
                        break;
                    default:
                        message = "기타 사유. 관리자 문의 바람";
                        break;
                }
                regiNotifyText.color = failTextColor;
                regiNotifyText.text = message;
            }
            else //생성 완료
            {
                user = RegisterTask.Result.User;

                if (user != null)
                {
                    UserProfile profile = new UserProfile { DisplayName = name };

                    //파이어베이스에 닉네임 정보 올림
                    Task ProfileTask = user.UpdateUserProfileAsync(profile);
                    yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);

                    if (ProfileTask.Exception != null)
                    {
                        Debug.LogWarning(message: "닉네임 설정 실패" + ProfileTask.Exception);
                        FirebaseException firebaseEx = ProfileTask.Exception.GetBaseException() as FirebaseException;
                        AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
                        regiNotifyText.color = failTextColor;
                        regiNotifyText.text = "닉네임 설정 실패";
                    }
                    else
                    {
                        regiNotifyText.color = successTextColor;
                        regiNotifyText.text = "생성 완료, 반갑습니다 " + user.DisplayName + "님";
                        StartCoroutine(LoginCor(id, pw));
                    }
                }
            }
        }
    }

    public void ConnectToServer()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.NickName = FirebaseAuthManager.user.DisplayName;
        Debug.Log("연결완 " + PhotonNetwork.NickName);
        SceneManager.LoadScene("Lobby");

    }

    public void OnTab(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            if (EventSystem.current.currentSelectedGameObject == null)
            {
                EventSystem.current.SetSelectedGameObject(EventSystem.current.firstSelectedGameObject);
            }
            EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnRight().Select();
        }
    }
}
