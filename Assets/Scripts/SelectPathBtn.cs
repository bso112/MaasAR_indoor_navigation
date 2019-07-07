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
    /// 맵 불러오기 - 맵 리스트 - 맵 선택
    /// </summary>
    public void SetSeletedButtonInfo()
    {
        PathListUpdater.selectedPathText[selectionCount++] = transform.GetComponentInChildren<Text>();

    }



}
