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

    [Header("Tutorial")]
    [SerializeField] private GameObject TutorialDialog;
    [SerializeField] private TMP_Text TutorialDialogTxt;

    string[] NPC_0 = new string[11];
    string[] NPC_1 = new string[6];
    string[] NPC_2 = new string[8];
    string[] NPC_3 = new string[2];
    string[] NPC_4 = new string[3];
    string[] NPC_5 = new string[3];

    private void Awake()
    {
        isDialoging = false;
        curTextNum = 0;

        NPC_0[0] = "[???]\n...여기가 어디지?";
        NPC_0[1] = "[???]\n그림자가 지하에 떨어지다니, 오래살고 볼 일이네.";
        NPC_0[2] = "[???]\n넌 누구야?";
        NPC_0[3] = "[루즈]\n자기소개부터 먼저 해야하는거 아냐? …루즈라고 불러.";
        NPC_0[4] = "[옥스]\n아, 미안… 나는 옥스라고 해.\n저, 저기… 여기서 빠져나가는 방법, 알아?";
        NPC_0[5] = "[루즈]\n여기라니, 그래도 우리 숲속이야. 빛이 살아가는 곳이라고.";
        NPC_0[6] = "[옥스]\n아아..! 여기가 그, 빛 동네였구나… 이런 곳은 처음이라…";
        NPC_0[7] = "[루즈]\n뭐, 아주 가끔 여기에 그림자가 떨어진다고는 들었는데. 나도 자세히는 몰라.";
        NPC_0[8] = "[옥스]\n그럼 어떡하지…";
        NPC_0[9] = "[루즈]\n…일단은 우리집으로 가는 거 어때. 오래 있다가는 어둠에 잡아먹히고 말걸.";
        NPC_0[10] = "[옥스]\n으응..!";

        NPC_1[0] = "[루즈]\n자, 이 숲은 지형이 험난해서 점프를 해야 지나갈 수 있어.";
        NPC_1[1] = "[옥스]\n그렇구나…";
        NPC_1[2] = "[루즈]\n이 빛에 닿으면, 난 좀 더 빠르게 갈 수도 있지. 우리 가족들은 ‘대쉬’라고 해.";
        NPC_1[3] = "[옥스]\n아, 우리도 그런게 있었어. 대쉬는 아니지만 특정한 땅에 발을 대면 ‘순간이동’을 할 수 있는데…";
        NPC_1[4] = "[루즈]\n지금 자랑하는거야?";
        NPC_1[5] = "[옥스]\n아, 아니야! 일단 점프, 이해했어. 앗 루즈, 같이 가!";

        NPC_2[0] = "[루즈]\n못 보던 벽이 있네.";
        NPC_2[1] = "[옥스]\n어…! 루즈, 나는 이 흰 벽은 통과되는 것 같아.";
        NPC_2[2] = "[루즈]\n흠.. 그래? 까짓거, 난 능력을 쓰면 되니까.";
        NPC_2[3] = "[옥스]\n바로 앞에는 검은색 벽이 있어.";
        NPC_2[4] = "[루즈]\n이건 내가 지나갈 수 있는걸? 신기한 벽이네.";
        NPC_2[5] = "[옥스]\n나는 어떻게 지나가지…?";
        NPC_2[6] = "[루즈]\n네가 아까 말한 순간이동인가 뭔가, 그걸 써봐.";
        NPC_2[7] = "[옥스]\n아..!";

        NPC_3[0] = "[옥스]\n이곳도 유리가 있네. 아까랑은 다른, 납작한 유리…";
        NPC_3[1] = "[루즈]\n이제 뭐 해야할지 알겠지?";

        //넣을 위치(npc)필요.
        NPC_4[0] = "[루즈]\n이상하네, 집으로 가는 길이 이렇게 멀었나?";
        NPC_4[1] = "[옥스]\n루즈, 혹시 길을 잃은게 아닌...";
        NPC_4[2] = "[루즈]\n아니! 모르는 길이면 찾아가면 되지.";

        NPC_5[0] = "[루즈]\n여기 발판이 있네.";
        NPC_5[1] = "[옥스]\n이 발판을 밟으면.. 벽이 열리는 걸까?";
        NPC_5[2] = "[루즈]\n한 번 해보지 뭐.";
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
      //  Debug.Log("objectty : " + objectType + " , curtex : " + curTextNum + " , text : " + NPC_0[curTextNum]);

        if(objectType == 0)
        {
            if(curTextNum == 0)
            {
                DialogImg.SetActive(true);

                chageNPCImage(false, true);
                StartCoroutine(TypingRoutine(NPC_0[curTextNum]));
                curTextNum++;
            }
            else if (curTextNum == 1)
            {
                chageNPCImage(true, false);
                StartCoroutine(TypingRoutine(NPC_0[curTextNum]));
                curTextNum++;
            }
            else if(curTextNum == 2)
            {
                chageNPCImage(false, true);
                StartCoroutine(TypingRoutine(NPC_0[curTextNum]));
                curTextNum++;
            }
            else if (curTextNum == 3)
            {
                chageNPCImage(true, false);
                StartCoroutine(TypingRoutine(NPC_0[curTextNum]));
                curTextNum++;
            }
            else if (curTextNum == 4)
            {
                chageNPCImage(false, true);
                StartCoroutine(TypingRoutine(NPC_0[curTextNum]));
                curTextNum++;
            }
            else if (curTextNum == 5)
            {
                chageNPCImage(true, false);
                StartCoroutine(TypingRoutine(NPC_0[curTextNum]));
                curTextNum++;
            }
            else if (curTextNum == 6)
            {
                chageNPCImage(false, true);
                StartCoroutine(TypingRoutine(NPC_0[curTextNum]));
                curTextNum++;
            }
            else if (curTextNum == 7)
            {
                chageNPCImage(true, false);
                StartCoroutine(TypingRoutine(NPC_0[curTextNum]));
                curTextNum++;
            }
            else if (curTextNum == 8)
            {
                chageNPCImage(false, true);
                StartCoroutine(TypingRoutine(NPC_0[curTextNum]));
                curTextNum++;
            }
            else if (curTextNum == 9)
            {
                chageNPCImage(true, false);
                StartCoroutine(TypingRoutine(NPC_0[curTextNum]));
                curTextNum++;
            }
            else if (curTextNum == 10)
            {
                chageNPCImage(false, true);
                StartCoroutine(TypingRoutine(NPC_0[curTextNum]));
                curTextNum++;
            }
            else if (curTextNum == 11)
            {
                curTextNum = 0;
                DialogImg.SetActive(false);
                moveSetOrigin.inEvent = false;
            }
        }

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
            else if (curTextNum == 3)
            {
                chageNPCImage(false, true);
                StartCoroutine(TypingRoutine(NPC_1[curTextNum]));
                curTextNum++;
            }
            else if (curTextNum == 4)
            {
                chageNPCImage(true, false);
                StartCoroutine(TypingRoutine(NPC_1[curTextNum]));
                curTextNum++;
            }
            else if(curTextNum == 5)
            {
                chageNPCImage(false, true);
                StartCoroutine(TypingRoutine(NPC_1[curTextNum]));
                curTextNum++;
            }
            else if (curTextNum == 6)
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

                chageNPCImage(true, false);
                StartCoroutine(TypingRoutine(NPC_2[curTextNum]));
                curTextNum++;
            }
            else if (curTextNum == 1)
            {
                chageNPCImage(false, true);
                StartCoroutine(TypingRoutine(NPC_2[curTextNum]));
                curTextNum++;
            }
            else if (curTextNum == 2)
            {
                chageNPCImage(true, false);
                StartCoroutine(TypingRoutine(NPC_2[curTextNum]));
                curTextNum++;
            }
            else if (curTextNum == 3)
            {
                chageNPCImage(false, true);
                StartCoroutine(TypingRoutine(NPC_2[curTextNum]));
                curTextNum++;
            }
            else if (curTextNum == 4)
            {
                chageNPCImage(true, false);
                StartCoroutine(TypingRoutine(NPC_2[curTextNum]));
                curTextNum++;
            }
            else if (curTextNum == 5)
            {
                chageNPCImage(false, true);
                StartCoroutine(TypingRoutine(NPC_2[curTextNum]));
                curTextNum++;
            }
            else if (curTextNum == 6)
            {
                chageNPCImage(true, false);
                StartCoroutine(TypingRoutine(NPC_2[curTextNum]));
                curTextNum++;
            }
            else if (curTextNum == 7)
            {
                chageNPCImage(false, true);
                StartCoroutine(TypingRoutine(NPC_2[curTextNum]));
                curTextNum++;
            }
            else if (curTextNum == 8)
            {
                curTextNum = 0;
                DialogImg.SetActive(false);
                moveSetOrigin.inEvent = false;
            }
        }
        if (objectType == 3)
        {
            if (curTextNum == 0)
            {
                DialogImg.SetActive(true);

                chageNPCImage(true, false);
                StartCoroutine(TypingRoutine(NPC_3[curTextNum]));
                curTextNum++;
            }
            else if (curTextNum == 1)
            {
                chageNPCImage(false, true);
                StartCoroutine(TypingRoutine(NPC_3[curTextNum]));
                curTextNum++;
            }
            else if (curTextNum == 2)
            {
                curTextNum = 0;
                DialogImg.SetActive(false);
                moveSetOrigin.inEvent = false;
            }
        }
        if (objectType == 4)
        {
            if (curTextNum == 0)
            {
                DialogImg.SetActive(true);

                chageNPCImage(true, false);
                StartCoroutine(TypingRoutine(NPC_4[curTextNum]));
                curTextNum++;
            }
            else if (curTextNum == 1)
            {
                chageNPCImage(false, true);
                StartCoroutine(TypingRoutine(NPC_4[curTextNum]));
                curTextNum++;
            }
            else if (curTextNum == 2)
            {
                chageNPCImage(true, false);
                StartCoroutine(TypingRoutine(NPC_4[curTextNum]));
                curTextNum++;
            }
            else if (curTextNum == 3)
            {
                curTextNum = 0;
                DialogImg.SetActive(false);
                moveSetOrigin.inEvent = false;
            }
        }
        if (objectType == 5)
        {
            if (curTextNum == 0)
            {
                DialogImg.SetActive(true);

                chageNPCImage(false, true);
                StartCoroutine(TypingRoutine(NPC_5[curTextNum]));
                curTextNum++;
            }
            else if (curTextNum == 2)
            {
                chageNPCImage(true, false);
                StartCoroutine(TypingRoutine(NPC_5[curTextNum]));
                curTextNum++;
            }
            else if (curTextNum == 0)
            {
                chageNPCImage(false, true);
                StartCoroutine(TypingRoutine(NPC_5[curTextNum]));
                curTextNum++;
            }
            else if (curTextNum == 3)
            {
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
