using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using KoreanTyper;
using TMPro;

public class OpeningEvent : MonoBehaviour
{
    string OriginText;
    public TMPro.TMP_Text OpeningText;

    void Start()
    {
        OriginText = "이봐...! 일어나봐...!";
        OpeningText.text = OriginText;
        OpeningText.text = "";

        

        StartCoroutine(TypingRoutine());
    }

    IEnumerator TypingRoutine()
    {
        int typingLength = OriginText.GetTypingLength();

        for(int index = 0; index <= typingLength; index++)
        {
            OpeningText.text = OriginText.Typing(index);
            yield return new WaitForSeconds(0.05f);
        }
    }
}
