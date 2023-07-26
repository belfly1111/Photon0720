using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using KoreanTyper;
using TMPro;

public class OpeningEvent : MonoBehaviour
{
    string[] OriginText = new string[3];
    public TMPro.TMP_Text OpeningText;
    [SerializeField] int dialogNum = 0;
    [SerializeField] Image BlackImg;

    bool isDialoging = false;


    void Start()
    {
        OpeningText.text = "";
        OriginText[0] = "...야...!";
        OriginText[1] = "...야...일어...!";
        OriginText[2] = "야! 일어나봐!";
    }

    private void Update()
    {
        // 스페이스를 눌러 대화 진행.
        if(Input.GetKey(KeyCode.Space) && !isDialoging)
        {
            if (dialogNum > 2)
            {
                Destroy(BlackImg);
                gameObject.SetActive(false);
            }
                StartCoroutine(TypingRoutine(dialogNum));
            dialogNum++;
            OpeningText.text = "";
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
}
