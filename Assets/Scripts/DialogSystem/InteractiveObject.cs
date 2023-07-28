using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveObject : MonoBehaviour
{
    public int objectType;          //상호작용하는 npc를 구분하기 위한 태그.
    private moveSetOrigin player;   //플레이어 정보를 저장
    [SerializeField]private GameObject QustionMark;

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
        Debug.Log("상호작용");
        if (objectType == 1)
        {
            Debug.Log("상호작용 1 작동");
        }
    }
}
