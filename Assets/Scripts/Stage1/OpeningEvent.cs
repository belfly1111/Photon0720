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
// 2. 페이드아웃 / 페이드인 기능 수행.
// 3. 플레이어 역할 선택. - 2명이 모두 선택할 때까지 캐릭터는 나오지 않음.
// 4. 플레이어 역할에 따른 다른 오프닝 재생. - 아직 없음.

public class OpeningEvent : MonoBehaviourPun
{
    string[] OriginText = new string[3];
    [SerializeField] TMP_Text OpeningText;
    [SerializeField] int dialogNum;

    [SerializeField] Image BlackImg;
    [SerializeField] Image NextIcon;

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

        // 오프닝 대화는 DialogManager에서 관리하지 않고 직접 초기화함.
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

        // 스페이스를 눌러 오프닝 대화를 진행함.
        if (Input.GetKey(KeyCode.Space) && !isDialoging && dialogNum <= 3)
        {
            if (dialogNum > 2)
            {
                NextIcon.enabled = false;
                OpeningText.enabled = false;
                StartFadeIn = true;
                return;
            }

            StartCoroutine(TypingRoutine(dialogNum));
            dialogNum++;
            OpeningText.text = "";
        }

        // 페이드인 연출을 시작함. 연출이 종료되면 역할 선택 버튼을 활성화함.
        if (StartFadeIn && !isDialoging)
        {
            elapsedTime += Time.deltaTime;
            float normalizedTime = elapsedTime / 1.0f;
            BlackImg.color = Color.Lerp(BlackImg.color, new Color(0, 0, 0, 0), normalizedTime);

            if (normalizedTime >= 1f)
            {
                BlackImg.enabled = false;
                StartFadeIn = false;
                SelectLightBtn.SetActive(true);
                SelectShadowBtn.SetActive(true);
            }
        }


        // 마지막으로 실행됨. 모든 이벤트가 끝나고 게임이 시작됨. 
        // 이제부터 스킬 사용이 가능해짐. 또한 여기서 개인별 인트로를 재생할 수도 있음.
        if (LightReady && DarkReady && !isDialoging)
        {
            if(PhotonManeger.LocalPlayerRule == 1)
            {
                PhotonNetwork.Instantiate("Light", new Vector3(-1, 1, 0), Quaternion.identity);
            }
            else if(PhotonManeger.LocalPlayerRule == 0)
            {
                PhotonNetwork.Instantiate("Dark", new Vector3(1, 1, 0), Quaternion.identity);
            }
            Skillmaneger_Stage_1.SetActive(true);
            SelectLightBtn.SetActive(false);
            SelectShadowBtn.SetActive(false);
            Destroy(gameObject);
        }
    }

    IEnumerator TypingRoutine(int dialogNum)
    {
        isDialoging = true;
        int typingLength = OriginText[dialogNum].GetTypingLength();

        for(int index = 0; index <= typingLength; index++)
        {
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
        PhotonManeger.LocalPlayerRule = 1;

        Debug.Log("Light를 선택하셨습니다.");

        photonView.RPC("SetInActiveBtn", RpcTarget.OthersBuffered, PhotonManeger.LocalPlayerRule);
        photonView.RPC("setLightReady", RpcTarget.AllBuffered);

    }

    public void setDark()
    {
        SelectLightBtn.SetActive(false);
        SelectShadowBtn.SetActive(false);

        // '0' 인 경우 'Dark'
        PhotonManeger.LocalPlayerRule = 0;

        Debug.Log("Dark를 선택하셨습니다.");

        photonView.RPC("SetInActiveBtn", RpcTarget.OthersBuffered, PhotonManeger.LocalPlayerRule);
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
