using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectMapBtn : MonoBehaviour
{
    private Button btn;


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
        ConfirmMapSelection.text = transform.GetComponentInChildren<Text>();
    }


}
