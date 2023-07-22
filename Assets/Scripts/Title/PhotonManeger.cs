using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;
using System.Reflection;
using UnityEditor;

public class PhotonManeger : MonoBehaviourPunCallbacks
{
    // 어쩔 수 없이 넣어줬다... stage가 바뀌면 쓸모없어진다.
    [Header("GamePanel")]
    public GameObject StartPanel;
    public GameObject SettingPanel;
    public GameObject MakeRoomPanel;
    public GameObject InRoomPanel;
    public GameObject WaitGamePanel;

    [Header("StartPanel")]
    public TMPro.TMP_InputField JoinRoomName;
    public TMPro.TMP_Text FindRoomStatus;

    [Header("MakeRoomPanel")]
    public TMPro.TMP_InputField RoomName;
    public TMPro.TMP_Text MakeRoomStatus;
    public GameObject CreateRoomBtn;
    private int SelectedStage;

    [Header("InRoomPanel")]
    public TMPro.TMP_Text InRoomStatus;
    public TMPro.TMP_Text InRoomName;
    public TMPro.TMP_Text InRoomStage;
    public GameObject GameStartBtn;


    static public bool Light;
    static public bool Dark;


    // 싱글톤 패턴을 이용해 여러 개의 Scene으로 관리하는 다양한 stage로 넘어가도 Pun2 서버 연결을 유지할 수 있도록 한다.
    public GameObject TitleCanvas;
    private static PhotonManeger instance;

    void Awake()
    {
        SelectedStage = 0;
        Light = false;
        Dark = false;
        PhotonNetwork.AutomaticallySyncScene = true;

        // -------------------------------------------

        // PhotonManeger 싱글톤 처리
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // 다른 Scene에서 PhotonManager가 이미 있는지 확인하고 있다면 파괴하지 않습니다.
            PhotonManeger[] managers = FindObjectsOfType<PhotonManeger>();
            if (managers.Length > 1)
            {
                // 두 개 이상의 인스턴스가 있는 경우, 첫 번째를 제외한 모든 다른 인스턴스를 파괴합니다.
                for (int i = 1; i < managers.Length; i++)
                {
                    Destroy(managers[i].gameObject);
                }
            }
        }
    }
    public void ConnectPhoton()
    {
        try
        {
            // Photon 서버에 접속
            PhotonNetwork.ConnectUsingSettings();
        }
        // 접속실패 시 console에 실패메세지 남김.
        catch (System.Exception ex)
        {
            Debug.LogError("Failed to connect to Photon Server: " + ex.Message);
        }
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("Photon Server 접속 완료");
        PhotonNetwork.JoinLobby();
    }
    public override void OnJoinedLobby()
    {
        Debug.Log("Photon Server Lobby 접속 완료");
    }
    public void CreateRoom()
    {
        // 사용자가 방이름도 입력하고 Stage도 고른 경우에만 CreateRoom이 활성화된다.
        if(RoomName.text != "" && SelectedStage != 0)
        {
            // RoomOptions. 플레이어는 무조건 두 명이 들어와야 하고 방은 공개하지 않는다.
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = 2;
            roomOptions.EmptyRoomTtl = 0; // 방이 비어있을 때의 생존 시간 (0으로 설정하면 즉시 삭제)
            roomOptions.IsVisible = false;

            // 비밀번호로 판별을 하기 위해 만들어줌. - 이 때, 방이름 자체가 비밀번호 역할을 한다.
            PhotonNetwork.CreateRoom(RoomName.text, roomOptions);
            StartPanel.SetActive(false);
            MakeRoomPanel.SetActive(false);
            InRoomPanel.SetActive(true);
            MakeRoomStatus.text = "";
        }
        else
        {
            MakeRoomStatus.text = "Please input RoomName & Choose the Stage";
        }
    }
    public override void OnCreatedRoom() => print("PhotonServer 방만들기 완료");
    public override void OnCreateRoomFailed(short returnCode, string message) => print("PhotonServer 방만들기 실패");

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(JoinRoomName.text);
    }
    public override void OnJoinedRoom()
    {
        if(SceneManager.GetActiveScene().name == "Title")
        {
            Debug.Log("Photon Server Room 접속완료");
            InRoomName.text = "RoomName : " + RoomName.text;
            InRoomStage.text = "RoomStage : " + SelectedStage;

            if (InRoomPanel.activeSelf == false) WaitGamePanel.SetActive(true);
        }
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("Photon Server Room 접속실패, 방 이름을 찾을 수 없음!");
        FindRoomStatus.text = "FindRoomStatus : Not exist room.";
    }
    
    // 플레이어가 room에서 나갔을 때 호출되는 함수. 5초가 지나면 게임이 종료되게 추후 처리해야한다.
    // 게임 내부에서는 적절히 처리해야한다.
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        // 현재 존재하는 Scene이 'Title' 일 경우, 
        if (SceneManager.GetActiveScene().name == "Title")
        {
            // 1명밖에 남지 않았다면 다시 게임 시작을 비활성화함.
            if (PhotonNetwork.PlayerList.Length == 1)
            {
                InRoomStatus.text = "Status : Waiting Your Friends...";
                GameStartBtn.SetActive(false);
                WaitGamePanel.SetActive(false);
                LeaveRoom();
            }
        }

        // 방에 남은 자신이 Master Client이고 현재 존재하는 Scene이 'Title'이 아닌 경우.
        if (PhotonNetwork.IsMasterClient && SceneManager.GetActiveScene().name != "Title")
        {
            // 1명밖에 남지 않았다면 타이틀 화면으로 되돌아간다.
            // 이 때 이 게임오브젝트 자체를 삭제한다.
            // - 포톤 연결을 끊고 다시 Title 환경에서  awake 함수로 호출되는 photon에 연결한다.
            if (PhotonNetwork.PlayerList.Length == 1)
            {
                SceneManager.LoadScene("Title");
                Disconnect();
                Destroy(gameObject);
            }
        }
    }

    public void LeaveRoom()
    {
        Debug.Log("방을 떠납니다. : ");
        PhotonNetwork.LeaveRoom();
        SelectedStage = 0;
    }
    public void Disconnect() => PhotonNetwork.Disconnect();
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Photon Server 접속 끊김");
    }

    // 스테이지를 고를 때 호출되는 함수
    public void StageSelect()
    {
        SelectedStage = 1;
    }

    // InRoomPanel에서 GameStart 버튼을 누르면 호출되는 함수
    public void GameStart()
    {
        if(SelectedStage == 1)
        {
            photonView.RPC("InActiveTitleCanvas", RpcTarget.AllBuffered);
            PhotonNetwork.LoadLevel("Stage1");
        }
    }

    // 플레이어가 새로운 Scene으로 이동할 때 실행되는 콜백
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (SceneManager.GetActiveScene().name == "Title")
        {
            base.OnPlayerEnteredRoom(newPlayer);
            Debug.Log("누군가 방에 들어옴 : " + newPlayer);
            InRoomStatus.text = "Status : Ready to Start";
            GameStartBtn.SetActive(true);
        }
    }

    [PunRPC]
    private void InActiveTitleCanvas()
    {
        StartPanel.SetActive(false);
        MakeRoomPanel.SetActive(false);
        SettingPanel.SetActive(false);
        InRoomPanel.SetActive(false);
        WaitGamePanel.SetActive(false);
        TitleCanvas.SetActive(false);
    }

    [PunRPC]
    private void ActiveTitleCanvas()
    {
        TitleCanvas.SetActive(true);
    }
}
