using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmMapSelection : MonoBehaviour
{
    public static Text text;
    private Button btn;

    private void Start()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(ConfirmSelection);
    }

    /// <summary>
    /// 맵 불러오기 - 맵 리스트 - 확인버튼 클릭
    /// </summary>
    public void ConfirmSelection()
    {   
        if(text != null)
            PathSpawner.Instance.LoadPath(text);
    }
}
