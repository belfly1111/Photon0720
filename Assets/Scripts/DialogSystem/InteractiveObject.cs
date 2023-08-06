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
    string[] NPC_6 = new string[2];
    string[] NPC_7 = new string[3];
    string[] NPC_8 = new string[2];
    string[] NPC_9 = new string[3];
    string[] NPC_10 = new string[6];
    string[] NPC_11 = new string[2];
    string[] NPC_12 = new string[4];


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

        NPC_5[0] = "[루즈]\n저 안개는 뭐야! 저런 안개는 숲에 원래 없었는데?";
        NPC_5[1] = "[옥스]\n그, 그래? 저 발판이랑 연관이 있지 않을까…?";
        NPC_5[2] = "[루즈]\n그래, 한 번 해보지 뭐.";

        //3-3 벽통과 
        NPC_6[0] = "[???]\n'명심하라. 보이는 것이 전부가 아니다.'";
        NPC_6[1] = "[옥스]\n(여기 이상한 말이 적혀 있어…! 루즈에게 알려줄까?)";

        //3-4 열쇠 기믹 대화 - Light
        NPC_7[0] = "[???]\n'명심하라. 동료와 함께 움직여야 빛날 수 있을 것이다.'";
        NPC_7[1] = "[루즈]\n(하아... 역시 누가 숲에 못된 장난을 치고 간게 틀림없어.)";
        NPC_7[2] = "[루즈]\n(옥스가 따로 뭔갈 할 때까지 기다리면 되는걸까?)";


        //3-4 열쇠 기믹 대화 - Shadow
        NPC_8[0] = "[???]\n'명심하라. 열쇠가 있어야 길이 열릴 것이다.";
        NPC_8[1] = "[옥스]\n(또 그 낙서야…! 저기 보이는 열쇠와 연관이 있는걸까?)";


        // 4. 아래로 떨어지고 나서.
        NPC_9[0] = "[루즈]\n누가 숲에다 이런 못된 장난을 친거야! 전부 처음보는 것들 뿐이야!";
        NPC_9[1] = "[옥스]\n진, 진정해 루즈. 일단 여기를 벗어나는게 낫지 않을까? 곧 '어둠'이 쫓아올꺼야…";
        NPC_9[2] = "[루즈]\n그래! 우리 동네에 빨리 가서 누가 이런 짓을 했는지 물어봐야겠어!";

        //5-1 점프맵 설명
        NPC_10[0] = "[루즈]\n누군지 모르겠지만 정말 대단하다... 방범안개까지 작동시키고 갔네...";
        NPC_10[1] = "[옥스]\n어떤 방범 안개…?";
        NPC_10[2] = "[루즈]\n동네로 가는 길을 막는 안개야. 높은 곳에 있는 2개의 스위치를 동시에 눌러야 안개가 사라져.";
        NPC_10[3] = "[루즈]\n원래 전부 하얀색 벽이지만, 일부가 검은색으로 바뀌었네. 아마 오늘 숲을 이상하게 만든 범인의 짓이겠지.";
        NPC_10[4] = "[옥스]\n그럼 나혼자 검은색 벽을 올라가야 한다고…? 내, 내가 할 수 있을까?";
        NPC_10[5] = "[루즈]\n할 수 밖에 없어! 빨리 풀어버리고 지나가자!";
         
        //5-2 가시와 종유석 기믹 & OrGate 설명
        NPC_11[0] = "[옥스]\n저 종유석하고 가시는 위험해보여...! 집, 집에 가고 싶어… 엄마아…";
        NPC_11[1] = "[루즈]\n지금까지 잘해왔잖아! 정신차려! 곧 숲이 어두워 진다고! 빨리 벗어나야해!";


        //6-1 마지막 열쇠 설명
        NPC_12[0] = "[루즈]\n다왔다! 열쇠가 어디있더라...? 헉! 설마 왼쪽 연못 안에 두고 왔나?";
        NPC_12[1] = "[옥스]\n열쇠는 나에게 맡겨줘…! 왼쪽으로 가면 되는거지!";
        NPC_12[2] = "[루즈]\n오, 이제 적극적으로 변했구나! 응응! 왼쪽으로 가면 있어!";
        NPC_12[3] = "[옥스]\n(으으…빨리 벗어나서 안전한 곳으로 갈꺼야! 이런 경험은 한 번이면 족해...)";
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
        else if (objectType == 1)
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
        else if (objectType == 2)
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
        else if (objectType == 3)
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
        else if (objectType == 4)
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
        else if (objectType == 5)
        {
            if (curTextNum == 0)
            {
                DialogImg.SetActive(true);

                chageNPCImage(true, false);
                StartCoroutine(TypingRoutine(NPC_5[curTextNum]));
                curTextNum++;
            }
            else if (curTextNum == 1)
            {
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
            else if (curTextNum == 3)
            {
                curTextNum = 0;
                DialogImg.SetActive(false);
                moveSetOrigin.inEvent = false;
            }
        }
        else if (objectType == 6)
        {
            if (curTextNum == 0)
            {
                DialogImg.SetActive(true);

                chageNPCImage(false, false);
                StartCoroutine(TypingRoutine(NPC_6[curTextNum]));
                curTextNum++;
            }
            else if (curTextNum == 1)
            {
                chageNPCImage(false, true);
                StartCoroutine(TypingRoutine(NPC_6[curTextNum]));
                curTextNum++;
            }
            else if (curTextNum == 2)
            {
                curTextNum = 0;
                DialogImg.SetActive(false);
                moveSetOrigin.inEvent = false;
            }
        }
        else if (objectType == 7)
        {
            if (curTextNum == 0)
            {
                DialogImg.SetActive(true);

                chageNPCImage(false, false);
                StartCoroutine(TypingRoutine(NPC_7[curTextNum]));
                curTextNum++;
            }
            else if (curTextNum == 1)
            {
                chageNPCImage(true, false);
                StartCoroutine(TypingRoutine(NPC_7[curTextNum]));
                curTextNum++;
            }
            else if (curTextNum == 2)
            {
                chageNPCImage(true, false);
                StartCoroutine(TypingRoutine(NPC_7[curTextNum]));
                curTextNum++;
            }
            else if (curTextNum == 3)
            {
                curTextNum = 0;
                DialogImg.SetActive(false);
                moveSetOrigin.inEvent = false;
            }
        }
        else if (objectType == 8)
        {
            if (curTextNum == 0)
            {
                DialogImg.SetActive(true);

                chageNPCImage(false, false);
                StartCoroutine(TypingRoutine(NPC_8[curTextNum]));
                curTextNum++;
            }
            else if (curTextNum == 1)
            {
                chageNPCImage(false, true);
                StartCoroutine(TypingRoutine(NPC_8[curTextNum]));
                curTextNum++;
            }
            else if (curTextNum == 2)
            {
                curTextNum = 0;
                DialogImg.SetActive(false);
                moveSetOrigin.inEvent = false;
            }
        }
        else if (objectType == 9)
        {
            if (curTextNum == 0)
            {
                DialogImg.SetActive(true);

                chageNPCImage(true, false);
                StartCoroutine(TypingRoutine(NPC_9[curTextNum]));
                curTextNum++;
            }
            else if (curTextNum == 1)
            {
                chageNPCImage(false, true);
                StartCoroutine(TypingRoutine(NPC_9[curTextNum]));
                curTextNum++;
            }
            else if (curTextNum == 2)
            {
                chageNPCImage(true, false);
                StartCoroutine(TypingRoutine(NPC_9[curTextNum]));
                curTextNum++;
            }
            else if (curTextNum == 3)
            {
                curTextNum = 0;
                DialogImg.SetActive(false);
                moveSetOrigin.inEvent = false;
            }
        }
        else if (objectType == 10)
        {
            if (curTextNum == 0)
            {
                DialogImg.SetActive(true);

                chageNPCImage(true, false);
                StartCoroutine(TypingRoutine(NPC_10[curTextNum]));
                curTextNum++;
            }
            else if (curTextNum == 1)
            {
                chageNPCImage(false, true);
                StartCoroutine(TypingRoutine(NPC_10[curTextNum]));
                curTextNum++;
            }
            else if (curTextNum == 2)
            {
                chageNPCImage(true, false);
                StartCoroutine(TypingRoutine(NPC_10[curTextNum]));
                curTextNum++;
            }
            else if (curTextNum == 3)
            {
                chageNPCImage(true, false);
                StartCoroutine(TypingRoutine(NPC_10[curTextNum]));
                curTextNum++;
            }
            else if (curTextNum == 4)
            {
                chageNPCImage(false, true);
                StartCoroutine(TypingRoutine(NPC_10[curTextNum]));
                curTextNum++;
            }
            else if (curTextNum == 5)
            {
                chageNPCImage(true, false);
                StartCoroutine(TypingRoutine(NPC_10[curTextNum]));
                curTextNum++;
            }
            else if (curTextNum == 6)
            {
                curTextNum = 0;
                DialogImg.SetActive(false);
                moveSetOrigin.inEvent = false;
            }
        }
        else if (objectType == 11)
        {
            if (curTextNum == 0)
            {
                DialogImg.SetActive(true);

                chageNPCImage(false, true);
                StartCoroutine(TypingRoutine(NPC_11[curTextNum]));
                curTextNum++;
            }
            else if(curTextNum == 1)
            {
                chageNPCImage(true, false);
                StartCoroutine(TypingRoutine(NPC_11[curTextNum]));
                curTextNum++;
            }
            else if (curTextNum == 2)
            {
                curTextNum = 0;
                DialogImg.SetActive(false);
                moveSetOrigin.inEvent = false;
            }
        }
        else if (objectType == 12)
        {
            if (curTextNum == 0)
            {
                DialogImg.SetActive(true);

                chageNPCImage(true, false);
                StartCoroutine(TypingRoutine(NPC_12[curTextNum]));
                curTextNum++;
            }
            else if(curTextNum == 1)
            {
                chageNPCImage(false, true);
                StartCoroutine(TypingRoutine(NPC_12[curTextNum]));
                curTextNum++;
            }
            else if(curTextNum == 2)
            {
                chageNPCImage(true, false);
                StartCoroutine(TypingRoutine(NPC_12[curTextNum]));
                curTextNum++;
            }
            else if(curTextNum == 3)
            {
                chageNPCImage(false, true);
                StartCoroutine(TypingRoutine(NPC_12[curTextNum]));
                curTextNum++;
            }
            else if (curTextNum == 4)
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
