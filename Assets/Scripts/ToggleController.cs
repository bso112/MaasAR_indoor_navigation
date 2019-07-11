using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleController : MonoBehaviour
{
    public List<Toggle> toggles;
    public int selectionCount = 0;

    public void GetSelection()
    {
        foreach(var toggle in toggles)
        {
            if (selectionCount > 1)
            {   
                Debug.Log("2개 이상은 선택할 수 없습니다.");
                selectionCount = 0;
                return;
            }
            else if (toggle.isOn)
            {
                PathListUpdater.selectedPathText[selectionCount++] = toggle.transform.GetComponentInParent<Text>();
            }
                
            
        }
    }

    public void ClearToggles()
    {
        foreach(var toggle in toggles)
        {
            toggle.isOn = false;
        }
    }
}
