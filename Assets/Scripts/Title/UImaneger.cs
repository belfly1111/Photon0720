using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UImaneger : MonoBehaviour
{
    [Header("GamePanel")]
    public GameObject StartPanel;
    public GameObject SettingPanel;
    public GameObject MakeRoomPanel;
    public GameObject InRoomPanel;
    public GameObject WaitGamePanel;

    [Header("StartPanel")]
    public TMPro.TMP_Text FindRoomStatus;

    [Header("MakeRoomPanel")]
    public TMPro.TMP_Text MakeRoomStatus;

    [Header("InRoomPanel")]
    public Image LoadingImg_1;

    [Header("WaitGamePanel")]
    public Image LoadingImg_2;

    [SerializeField] int RotationSpeed;

    private void Awake()
    {

        RotationSpeed = 90;
    }

    void Update()
    {
        if(InRoomPanel.activeSelf)
        {
            LoadingImg_1.transform.Rotate(Vector3.forward * RotationSpeed * Time.deltaTime);
        }
        if (WaitGamePanel.activeSelf)
        {
            LoadingImg_2.transform.Rotate(Vector3.forward * RotationSpeed * Time.deltaTime);
        }
    }

    public void GameStart()
    {
        FindRoomStatus.text = "FindRoomStatus";

        StartPanel.SetActive(true);
    }
    public void Setting()
    {
        SettingPanel.SetActive(true);
    }
    public void MakeRoom()
    {
        MakeRoomStatus.text = "";
        MakeRoomPanel.SetActive(true);
    }
    public void InRoom()
    {
        FindRoomStatus.text = "FindRoomStatus";
        InRoomPanel.SetActive(true);
    }
    public void WaitGame()
    {
        WaitGamePanel.SetActive(true);
    }

    public void GameQuit()
    {
        Application.Quit();
    }
    public void FalsePanel()
    {
        if(InRoomPanel.activeSelf)
        {
            InRoomPanel.SetActive(false);
            return;
        }
        if(MakeRoomPanel.activeSelf)
        {
            MakeRoomPanel.SetActive(false);
            return;
        }
        if (WaitGamePanel.activeSelf)
        {
            WaitGamePanel.SetActive(false);
            return;
        }
        if (StartPanel.activeSelf)
        {
            StartPanel.SetActive(false);
            return;
        }
        if (SettingPanel.activeSelf)
        {
            SettingPanel.SetActive(false);
            return;
        }
    }
}
