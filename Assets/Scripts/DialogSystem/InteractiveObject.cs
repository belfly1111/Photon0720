using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using KoreanTyper;
using TMPro;


public class InteractiveObject : MonoBehaviour
{
    public int objectType;  // 상호작용하는 npc를 구분하기 위한 태그.
    public int previousObjectType; // 지난 번 상호작용했던 npc를 구분하는 태그.
    public int curTextNum; // 현재 몇 번째 문장을 읽고 있는지 판별하는 변수.
    private bool isDialoging;
    private moveSetOrigin player;

    [SerializeField] private GameObject QustionMark;
    [SerializeField] private GameObject DialogImg;
    [SerializeField] private TMP_Text DialogTxt;
    [SerializeField] private Image LightImg;
    [SerializeField] private Image DarkImg;



    string[] NPC_1 = new string[3];

    private void Awake()
    {
        isDialoging = false;
        curTextNum = 0;
        NPC_1[0] = "[Light]\n괜찮아?";
        NPC_1[1] = "[Shadow]\n(으... 머리 아파...) 도와주셔서 감사합니다... 혹시 여기가 어딘가요?";
        NPC_1[2] = "[Light]\n(여기? )";

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //플레이어 캐릭터와 충돌 시 movesetorigin의 interactive object를 자신으로 설정.
            player = collision.gameObject.GetComponent<moveSetOrigin>();
            if(player != null)
            {
                player.InteractiveObject = this;
            }
            QustionMark.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //충돌에서 벗어날 시 힌트패널 비활성화
            QustionMark.SetActive(false);

            //movesetorigin에서 정보를 지움
            if (player != null)
            {
                player.InteractiveObject = null;
                player = null;
            }
        }
    }

    //대화창이나 퀘스트 수락은 여기서 작업하면 된다.
    public void Interaction()
    {
        if (isDialoging) return;

        if (objectType == 1)
        {
            if(curTextNum == 0)
            {
                DialogImg.SetActive(true);

                chageNPCImage(true, false);
                StartCoroutine(TypingRoutine(NPC_1[curTextNum]));
                curTextNum++;
            }
            else if(curTextNum == 1)
            {
                chageNPCImage(false, true);
                StartCoroutine(TypingRoutine(NPC_1[curTextNum]));
                curTextNum++;
            }
            else if(curTextNum == 2)
            {
                chageNPCImage(true, false);
                StartCoroutine(TypingRoutine(NPC_1[curTextNum]));
                curTextNum++;
            }
            else if(curTextNum == 3)
            {
                curTextNum = 0;
                DialogImg.SetActive(false);
                moveSetOrigin.inEvent = false; 
            }
        }

        if(objectType == 2)
        {
            if (curTextNum == 0)
            {
                DialogImg.SetActive(true);
                DialogTxt.text = "'뭐야? 별 것 아니였네'";
                LightImg.enabled = true;
                DarkImg.enabled = false;
                curTextNum++;
            }
            else if(curTextNum == 1)
            {
                DialogTxt.text = "이렇게 장애물을 피해가면 되요! 쉽죠?";

                curTextNum = 0;
                DialogImg.SetActive(false);
                moveSetOrigin.inEvent = false;
            }
        }
    }

    // 들어가는 변수에 따라서 NPC의 image가 활성 / 비활성화됨.
    void chageNPCImage(bool Light, bool Shadow)
    {
        if(Light) LightImg.enabled = true;
        else if(!Light) LightImg.enabled = false;

        if (Shadow) DarkImg.enabled = true;
        else if (!Shadow) DarkImg.enabled = false;
    }


    IEnumerator TypingRoutine(string curText)
    {
        AudioSource audio = GetComponent<AudioSource>();
        isDialoging = true;
        int typingLength = curText.GetTypingLength();
        
        for (int index = 0; index <= typingLength; index++)
        {
            DialogTxt.text = curText.Typing(index);
            yield return new WaitForSeconds(0.025f);
            if (!audio.isPlaying)
            {
                audio.Play();
            }
        }
        isDialoging = false;
    }


}
