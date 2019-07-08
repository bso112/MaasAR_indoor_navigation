using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectPathBtn : MonoBehaviour
{
    private Button btn;
    private static int selectionCount = 0;

    private void Start()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(SetSeletedButtonInfo);
    }



    /// <summary>
    /// 자기자신이 선택되면 PathListUpdate에 자기자신에 대한 정보를 넘겨준다.
    /// </summary>
    public void SetSeletedButtonInfo()
    {
        PathListUpdater.selectedPathText[selectionCount++] = transform.GetComponentInChildren<Text>();

    }



}
