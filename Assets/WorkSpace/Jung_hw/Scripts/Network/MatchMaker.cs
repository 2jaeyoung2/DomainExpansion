using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MatchMaker : MonoBehaviourPunCallbacks
{
    [SerializeField] MapMaker mapMaker;
    [SerializeField] TMP_Text nicknameText;

    [SerializeField] GameObject matchingPopUp;
    [SerializeField] GameObject matchFoundPopUp;

    [SerializeField] TMP_Text matchingTimeText;
    [SerializeField] TMP_Text matchFoundCountText;

    Coroutine matchingCor;
    Coroutine matchFoundCor;

    int matchSec = 0;
    int matchMin = 0;
    int matchFoundCountdown = 5;
    public string myMapInfo = "";

    private void Start()
    {
        matchFoundPopUp.SetActive(false);
        InitTimes();
        PhotonNetwork.JoinLobby();
        nicknameText.text = PhotonNetwork.NickName;
        Debug.Log(photonView);
    }

    void InitTimes()
    {
        matchSec = 0;
        matchMin = 0;
        matchFoundCountdown = 5;
        SetMatchingTime();
    }

    void SetMatchingTime()
    {
        matchingTimeText.text = $"{matchMin:00}:{matchSec:00}"; // num:00 == num.ToString("00")
    }

    public void StartMatch()
    {
        InitTimes();
        matchingPopUp.SetActive(true);
        matchingCor = StartCoroutine(MatchingTimeCor());
        PhotonNetwork.JoinRandomRoom();
    }

    IEnumerator MatchingTimeCor()
    {
        while(true)
        {
            yield return new WaitForSeconds(1);
            matchSec++;
            if (matchSec >= 60)
            {
                matchSec = 0;
                matchMin++;
            }
            SetMatchingTime();
        }
    }

    public void CancenMatch()
    {
        matchingPopUp.SetActive(false);
        PhotonNetwork.LeaveRoom();
        StopCoroutine(matchingCor);
        matchingCor = null;
    }

    [PunRPC]
    public void FoundMatch()
    {
        matchFoundPopUp.SetActive(true);
        photonView.RPC("SendMapData", RpcTarget.AllBufferedViaServer, PhotonNetwork.LocalPlayer.ActorNumber, myMapInfo);
        photonView.RPC("SendSpawnPoint", RpcTarget.AllBufferedViaServer, PhotonNetwork.LocalPlayer.ActorNumber, mapMaker.spawnLoc);
        if(PhotonNetwork.IsMasterClient)
        {
            foreach (var player in PhotonNetwork.CurrentRoom.Players)
            {
                photonView.RPC("SetTeam", RpcTarget.AllBufferedViaServer, PhotonNetwork.LocalPlayer.ActorNumber);
            }
        }
        matchFoundCor = StartCoroutine(MatchCountDownCor());
    }

    IEnumerator MatchCountDownCor()
    {
        while(matchFoundCountdown > 0)
        {
            yield return new WaitForSeconds(1);
            matchFoundCountdown--;
            matchFoundCountText.text = matchFoundCountdown.ToString();
        }

        matchFoundCor = null;
        StopCoroutine(matchingCor);
        matchingCor = null;

        if(PhotonNetwork.IsMasterClient)
        {
            StartGame();
        }
    }

    [PunRPC]
    public void SendMapData(int id, string map)
    {
        GameManager.Instance.SetMapDict(id, map);
    }

    [PunRPC] 
    public void SendSpawnPoint(int id, int index)
    {
        GameManager.Instance.SetSpawnPoint(id, index);
    }

    [PunRPC]
    public void SetTeam(int id)
    {
        GameManager.Instance.SetId(id);
    }

    public void StartGame()
    {
        PhotonNetwork.LoadLevel("DefaltMap");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 2 });
    }

    public override void OnJoinedRoom()
    {
        if(PhotonNetwork.CurrentRoom.PlayerCount >= 2)
        {
            Debug.Log(photonView);
            photonView.RPC("FoundMatch", RpcTarget.AllBuffered);
        }
    }


}
