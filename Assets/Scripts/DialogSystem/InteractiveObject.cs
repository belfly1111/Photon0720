using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InteractiveObject : MonoBehaviour
{
    public int objectType;  // 상호작용하는 npc를 구분하기 위한 태그.
    public int previousObjectType; // 지난 번 상호작용했던 npc를 구분하는 태그.
    public int curTextNum; // 현재 몇 번째 문장을 읽고 있는지 판별하는 변수.
    private moveSetOrigin player;

    [SerializeField] private GameObject QustionMark;
    [SerializeField] private GameObject DialogImg;
    [SerializeField] private TMP_Text DialogTxt;
    [SerializeField] private Image LightImg;
    [SerializeField] private Image DarkImg;

    
    private void Awake()
    {
        curTextNum = 0;
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
        if (objectType == 1)
        {
            if(curTextNum == 0)
            {
                DialogImg.SetActive(true);
                DialogTxt.text = "정신이 좀 들어요?";
                LightImg.enabled = true;
                DarkImg.enabled = false;
                curTextNum++;
            }
            else if(curTextNum == 1)
            {
                DialogTxt.text = "네...네!";
                LightImg.enabled = false;
                DarkImg.enabled = true;
                curTextNum++;

            }
            else if(curTextNum == 2)
            {
                DialogTxt.text = "그럼 계속 이동해요!";
                DarkImg.enabled = false;
                LightImg.enabled = true;
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
}
