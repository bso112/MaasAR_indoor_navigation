using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionSender : MonoBehaviour
{
    public static int selectionCount = 0;
    private GameObject[] consoles;
    private Text console;

    private void Start()
    {
        consoles = GameObject.FindGameObjectsWithTag("console");
        foreach(var console in consoles)
        {
            if(console.name == "Console9")
            {
                this.console = console.GetComponent<Text>();
            }
        }
    }
    public void SendSelection()
    {   

        if(selectionCount>1)
        {
            console.text = "선택이 초기화 됩니다.";
            selectionCount = 0;
            return;
        }
        if(transform.GetComponent<Toggle>().isOn)
        {
            PathListUpdater.selectedPathText[selectionCount++] = transform.GetComponentInParent<Text>();
            if (PathListUpdater.selectedPathText[0] != null && PathListUpdater.selectedPathText[1] != null)
                console.text = "첫번째 선택 : " + PathListUpdater.selectedPathText[0].text + "두번째 선택 :" + PathListUpdater.selectedPathText[1].text;

        }

    }
}
