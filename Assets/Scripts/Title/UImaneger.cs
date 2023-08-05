using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UImaneger : MonoBehaviour
{
    new AudioSource audio;
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
        audio = GetComponent<AudioSource>();
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
        audio.Play();
        StartPanel.SetActive(true);
    }
    public void Setting()
    {
        audio.Play();
        SettingPanel.SetActive(true);
    }
    public void MakeRoom()
    {
        audio.Play();
        MakeRoomStatus.text = "";
        MakeRoomPanel.SetActive(true);
    }
    public void InRoom()
    {
        audio.Play();
        FindRoomStatus.text = "FindRoomStatus";
        InRoomPanel.SetActive(true);
    }
    public void WaitGame()
    {
        audio.Play();
        WaitGamePanel.SetActive(true);
    }

    public void GameQuit()
    {
        audio.Play();
        Application.Quit();
    }
    public void FalsePanel()
    {
        if(InRoomPanel.activeSelf)
        {
            audio.Play();
            InRoomPanel.SetActive(false);
            return;
        }
        if(MakeRoomPanel.activeSelf)
        {
            audio.Play();
            MakeRoomPanel.SetActive(false);
            return;
        }
        if (WaitGamePanel.activeSelf)
        {
            audio.Play();
            WaitGamePanel.SetActive(false);
            return;
        }
        if (StartPanel.activeSelf)
        {
            audio.Play();
            StartPanel.SetActive(false);
            return;
        }
        if (SettingPanel.activeSelf)
        {
            audio.Play();
            SettingPanel.SetActive(false);
            return;
        }
    }


}
