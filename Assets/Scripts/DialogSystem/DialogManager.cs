using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogManager : MonoBehaviour
{
    [SerializeField] GameObject Dialog_Panel;
    [SerializeField] TMP_Text Dialog_Text;
    [SerializeField] GameObject scanObject;
    [SerializeField] bool isAction;


    public void Action(GameObject ScanObj)
    {
        if(isAction) isAction = false;
        else
        {
            isAction = true;
            scanObject = ScanObj;
            Dialog_Text.text = "";
        }
        Dialog_Panel.SetActive(isAction);
    }



}
