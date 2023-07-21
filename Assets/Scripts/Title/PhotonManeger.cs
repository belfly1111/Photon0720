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
    // ��¿ �� ���� �־����... stage�� �ٲ�� �����������.
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


    // �̱��� ������ �̿��� ���� ���� Scene���� �����ϴ� �پ��� stage�� �Ѿ�� Pun2 ���� ������ ������ �� �ֵ��� �Ѵ�.
    public GameObject TitleCanvas;
    private static PhotonManeger instance;

    void Awake()
    {
        SelectedStage = 0;
        Light = false;
        Dark = false;
        PhotonNetwork.AutomaticallySyncScene = true;

        // -------------------------------------------

        // PhotonManeger �̱��� ó��
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // �ٸ� Scene���� PhotonManager�� �̹� �ִ��� Ȯ���ϰ� �ִٸ� �ı����� �ʽ��ϴ�.
            PhotonManeger[] managers = FindObjectsOfType<PhotonManeger>();
            if (managers.Length > 1)
            {
                // �� �� �̻��� �ν��Ͻ��� �ִ� ���, ù ��°�� ������ ��� �ٸ� �ν��Ͻ��� �ı��մϴ�.
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
            // Photon ������ ����
            PhotonNetwork.ConnectUsingSettings();
        }
        // ���ӽ��� �� console�� ���и޼��� ����.
        catch (System.Exception ex)
        {
            Debug.LogError("Failed to connect to Photon Server: " + ex.Message);
        }
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("Photon Server ���� �Ϸ�");
        PhotonNetwork.JoinLobby();
    }
    public override void OnJoinedLobby()
    {
        Debug.Log("Photon Server Lobby ���� �Ϸ�");
    }
    public void CreateRoom()
    {
        // ����ڰ� ���̸��� �Է��ϰ� Stage�� �� ��쿡�� CreateRoom�� Ȱ��ȭ�ȴ�.
        if(RoomName.text != "" && SelectedStage != 0)
        {
            // RoomOptions. �÷��̾�� ������ �� ���� ���;� �ϰ� ���� �������� �ʴ´�.
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = 2;
            roomOptions.EmptyRoomTtl = 0; // ���� ������� ���� ���� �ð� (0���� �����ϸ� ��� ����)
            roomOptions.IsVisible = false;

            // ��й�ȣ�� �Ǻ��� �ϱ� ���� �������. - �� ��, ���̸� ��ü�� ��й�ȣ ������ �Ѵ�.
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
    public override void OnCreatedRoom() => print("PhotonServer �游��� �Ϸ�");
    public override void OnCreateRoomFailed(short returnCode, string message) => print("PhotonServer �游��� ����");

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(JoinRoomName.text);
    }
    public override void OnJoinedRoom()
    {
        if(SceneManager.GetActiveScene().name == "Title")
        {
            Debug.Log("Photon Server Room ���ӿϷ�");
            InRoomName.text = "RoomName : " + RoomName.text;
            InRoomStage.text = "RoomStage : " + SelectedStage;

            if (InRoomPanel.activeSelf == false) WaitGamePanel.SetActive(true);
        }
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("Photon Server Room ���ӽ���, �� �̸��� ã�� �� ����!");
        FindRoomStatus.text = "FindRoomStatus : Not exist room.";
    }
    
    // �÷��̾ room���� ������ �� ȣ��Ǵ� �Լ�. 5�ʰ� ������ ������ ����ǰ� ���� ó���ؾ��Ѵ�.
    // ���� ���ο����� ������ ó���ؾ��Ѵ�.
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        // ���� �����ϴ� Scene�� 'Title' �� ���, 
        if (SceneManager.GetActiveScene().name == "Title")
        {
            // 1��ۿ� ���� �ʾҴٸ� �ٽ� ���� ������ ��Ȱ��ȭ��.
            if (PhotonNetwork.PlayerList.Length == 1)
            {
                InRoomStatus.text = "Status : Waiting Your Friends...";
                GameStartBtn.SetActive(false);
                WaitGamePanel.SetActive(false);
                LeaveRoom();
            }
        }

        // �濡 ���� �ڽ��� Master Client�̰� ���� �����ϴ� Scene�� 'Title'�� �ƴ� ���.
        if (PhotonNetwork.IsMasterClient && SceneManager.GetActiveScene().name != "Title")
        {
            // 1��ۿ� ���� �ʾҴٸ� Ÿ��Ʋ ȭ������ �ǵ��ư���.
            // �� �� �� ���ӿ�����Ʈ ��ü�� �����Ѵ�.
            // - ���� ������ ���� �ٽ� Title ȯ�濡��  awake �Լ��� ȣ��Ǵ� photon�� �����Ѵ�.
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
        Debug.Log("���� �����ϴ�. : ");
        PhotonNetwork.LeaveRoom();
        SelectedStage = 0;
    }
    public void Disconnect() => PhotonNetwork.Disconnect();
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Photon Server ���� ����");
    }

    // ���������� �� �� ȣ��Ǵ� �Լ�
    public void StageSelect()
    {
        SelectedStage = 1;
    }

    // InRoomPanel���� GameStart ��ư�� ������ ȣ��Ǵ� �Լ�
    public void GameStart()
    {
        if(SelectedStage == 1)
        {
            photonView.RPC("InActiveTitleCanvas", RpcTarget.AllBuffered);
            PhotonNetwork.LoadLevel("Stage1");
        }
    }

    // �÷��̾ ���ο� Scene���� �̵��� �� ����Ǵ� �ݹ�
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (SceneManager.GetActiveScene().name == "Title")
        {
            base.OnPlayerEnteredRoom(newPlayer);
            Debug.Log("������ �濡 ���� : " + newPlayer);
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

