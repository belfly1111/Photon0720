using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using KoreanTyper;
using TMPro;

// 포톤 환경에서의 namespace이다.
using Photon.Pun;
using Photon.Realtime;
using System.Reflection;
using UnityEditor;

// 07.28 새로 작성. 기존에 존재하던 StageUI_1.cs의 역할을 대체함.

// Stage 1으로 이동 후 발생하는 오프닝 이벤트를 관리함. 아래와 같은 역할을 함.
// 아래 3가지 역할을 완수하면 스스로 파괴됨.
//
// 1. 인트로 대사 재생
// 2. 플레이어 역할 선택. - 2명이 모두 선택할 때까지 캐릭터는 나오지 않음.
// 3. 플레이어 역할에 따른 다른 오프닝 재생. - 아직 없음.
// 4. 페이드인 효과가 발생되며 게임 시작.

public class OpeningEvent : MonoBehaviourPun
{
    string[] OriginText = new string[3];
    [SerializeField] TMP_Text OpeningText;
    [SerializeField] int dialogNum;

    [SerializeField] Image BlackImg;
    [SerializeField] Image NextIcon;
    [SerializeField] Image Light_image;
    [SerializeField] Image Shadow_image;
    [SerializeField] TMP_Text Light_Profile;
    [SerializeField] TMP_Text Shadow_Profile;


    [SerializeField] GameObject SelectLightBtn;
    [SerializeField] GameObject SelectShadowBtn;
    [SerializeField] GameObject Skillmaneger_Stage_1;


    bool LightReady;
    bool DarkReady;
    bool isDialoging;
    bool StartFadeIn;
    float elapsedTime;


    private void Awake()
    {
        dialogNum = 0;
        elapsedTime = 0f;
        LightReady = false;
        DarkReady = false;
        isDialoging = false;
        StartFadeIn = false;

        // 일러스트와 소개문구 초기화.
        Light_image.enabled = false;
        Shadow_image.enabled = false;
        Light_Profile.enabled = false;
        Shadow_Profile.enabled = false;

        // 오프닝 대화는 직접 초기화함.
        OpeningText.text = "...";
        OriginText[0] = "...야...!";
        OriginText[1] = "...야...일어...!";
        OriginText[2] = "야! 일어나봐!";
    }

    private void Update()
    {
        // 모든 플레이어의 역할 선택이 끝나면 각자 개인 추가 컷신을 보여주고
        // Skillmaeger_Stage_1을 활성화함.
        // 07.28 현재는 개인 추가 컷신이 없으므로 페이드인 효과만 추가함.

        // 1. 스페이스를 눌러 오프닝 대화를 진행함.
        if (Input.GetKey(KeyCode.Space) && !isDialoging && dialogNum <= 3 && PhotonManeger.instance.LocalPlayerRule == -1)
        {
            if (dialogNum > 2)
            {
                NextIcon.enabled = false;
                OpeningText.enabled = false;

                // 일러스트와 소개문구 표시
                Light_image.enabled = true;
                Shadow_image.enabled = true;
                Light_Profile.enabled = true;
                Shadow_Profile.enabled = true;

                if (!LightReady)
                {
                    SelectLightBtn.SetActive(true);
                }
                if (!DarkReady)
                {
                    SelectShadowBtn.SetActive(true);
                }
                return;
            }

            StartCoroutine(TypingRoutine(dialogNum));
            dialogNum++;
            OpeningText.text = "";
        }

        // 2. 역할군을 선택함. 역할군을 선택한 뒤에는 되돌릴 수 없음.
        if (LightReady && DarkReady && !isDialoging && PhotonManeger.instance.LocalPlayerRule != -1 && !StartFadeIn)
        {
            if (PhotonManeger.instance.LocalPlayerRule == 1)
            {
                PhotonNetwork.Instantiate("Light", new Vector3(-26, 0.5f, 0), Quaternion.identity);
            }
            else if (PhotonManeger.instance.LocalPlayerRule == 0)
            {
                PhotonNetwork.Instantiate("Dark", new Vector3(-25, 0.5f, 0), Quaternion.identity);
            }

            Skillmaneger_Stage_1.SetActive(true);

            // 일러스트와 소개문구 표시
            Light_image.enabled = false;
            Shadow_image.enabled = false;
            Light_Profile.enabled = false;
            Shadow_Profile.enabled = false;

            SelectLightBtn.SetActive(false);
            SelectShadowBtn.SetActive(false);
            StartFadeIn = true;
        }

        // 3. 페이드인 연출을 시작함. 연출이 종료되면 페이드인 효과가 발동함.
        if (StartFadeIn && !isDialoging)
        {
            elapsedTime += Time.deltaTime;
            float normalizedTime = elapsedTime / 3.0f;
            BlackImg.color = Color.Lerp(BlackImg.color, new Color(0, 0, 0, 0), normalizedTime);

            if (normalizedTime >= 4f)
            {
                BlackImg.enabled = false;
                StartFadeIn = false;
                Destroy(gameObject);
            }
        }
    }

    IEnumerator TypingRoutine(int dialogNum)
    {
        AudioSource audio = GetComponent<AudioSource>();
        isDialoging = true;
        int typingLength = OriginText[dialogNum].GetTypingLength();

        for(int index = 0; index <= typingLength; index++)
        {
            audio.Play();
            OpeningText.text = OriginText[dialogNum].Typing(index);
            yield return new WaitForSeconds(0.05f);
        }
        isDialoging = false;
    }


    public void setLight()
    {
        SelectLightBtn.SetActive(false);
        SelectShadowBtn.SetActive(false);

        // '1' 인 경우 'Light'
        PhotonManeger.instance.LocalPlayerRule = 1;

        Debug.Log("Light를 선택하셨습니다.");

        photonView.RPC("SetInActiveBtn", RpcTarget.OthersBuffered, PhotonManeger.instance.LocalPlayerRule);
        photonView.RPC("setLightReady", RpcTarget.AllBuffered);

    }

    public void setDark()
    {
        SelectLightBtn.SetActive(false);
        SelectShadowBtn.SetActive(false);

        // '0' 인 경우 'Dark'
        PhotonManeger.instance.LocalPlayerRule = 0;

        Debug.Log("Dark를 선택하셨습니다.");

        photonView.RPC("SetInActiveBtn", RpcTarget.OthersBuffered, PhotonManeger.instance.LocalPlayerRule);
        photonView.RPC("setDarkReady", RpcTarget.AllBuffered);
    }

    [PunRPC]
    private void SetInActiveBtn(int AnotherSideRule)
    {
        if (AnotherSideRule == 1) SelectLightBtn.SetActive(false);
        else if (AnotherSideRule == 0) SelectShadowBtn.SetActive(false);
    }

    [PunRPC]
    void setLightReady()
    {
        LightReady = true;
    }

    [PunRPC]
    void setDarkReady()
    {
        DarkReady = true;
    }
}
