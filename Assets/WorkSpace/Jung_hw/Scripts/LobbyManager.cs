using Firebase.Auth;
using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;

public class LobbyManager : MonoBehaviour
{
    [SerializeField] Animation uiAnim;
    [SerializeField] MapMaker mapMaker;
    DatabaseReference dbRef;
    FirebaseUser user;
    public List<string> anims;

    private void Start()
    {
        anims = new List<string>();
        uiAnim.wrapMode = WrapMode.Once;
        dbRef = FirebaseAuthManager.dbRef;
        user = FirebaseAuthManager.user;
        Debug.Log(dbRef);
        foreach(AnimationState state in uiAnim)
        {
            anims.Add(state.name);
        }
        GetMapFromDB();
    }
    public void SetEditMap(bool expand)
    {
        if(expand == true)
        {
            uiAnim.Play(anims[0]);
        }
        else
        {
            uiAnim.Play(anims[1]);
            SaveMapToDB(mapMaker.GetTileList());
        }
        mapMaker.placeMode = expand;
    }

    public void SaveMapToDB(string tiles)
    {
        StartCoroutine(PostMap(tiles));
    }

    IEnumerator PostMap(string tiles)
    {
        Debug.Log(tiles);

        var task = dbRef.Child("users").Child(user.UserId).Child("map").SetValueAsync(tiles);

        yield return new WaitUntil(predicate: () => task.IsCompleted);

        if(task.Exception != null)
        {
            Debug.LogWarning("Update Failed: " + task.Exception);
        }
    }

    public void GetMapFromDB()
    {
        StartCoroutine(GetMap());
    }

    IEnumerator GetMap()
    {
        var task = dbRef.Child("users").Child(user.UserId).GetValueAsync();

        yield return new WaitUntil(() => task.IsCompleted);

        if(task.Exception != null)
        {
            Debug.LogWarning("Update Failed: " + task.Exception);
        }
        else if(task.Result.Value == null)
        {
            Debug.Log("No data");
        }
        else
        {
            DataSnapshot snapshot = task.Result;
            string toMap = snapshot.Child("map").Exists ? snapshot.Child("map").Value.ToString() : "";
            mapMaker.SetTileList(toMap);
        }
    }
}
