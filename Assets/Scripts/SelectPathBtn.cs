using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary; //Formatters 쓸려고..

public class SelectPathBtn : MonoBehaviour
{
    private Button btn;
    public GameObject pathInfoPanel;
    private PathData pathData = new PathData();

    private void Start()
    {
        string pathName = transform.GetComponentInChildren<Text>().text;

        if (File.Exists(Application.persistentDataPath + "/" + pathName + ".dat")) //비어있지 않으면 로드!

        {

            var b = new BinaryFormatter(); //바이너리 포맷터

            var f = File.Open(Application.persistentDataPath + "/" + pathName + ".dat", FileMode.Open); // 파일 열기.

            pathData = (PathData)b.Deserialize(f); //스코어를 로드. 디 시리얼라이즈.

            f.Close(); //파일 닫기.

        }

        btn = GetComponent<Button>();
        btn.onClick.AddListener(SetSeletedButtonInfo);
        btn.onClick.AddListener(ShowButtonInfo);
        //자신의 토글을 토글컨트롤러에 등록한다.
        ToggleController toggleController = GameObject.Find("ToggleController").GetComponent<ToggleController>();
        toggleController.toggles.Add(transform.GetComponentInChildren<Toggle>());

    }

    


    /// <summary>
    /// 자기자신이 선택되면 PathListUpdate에 자기자신에 대한 정보를 넘겨준다.
    /// </summary>
    public void SetSeletedButtonInfo()
    {   
        PathListUpdater.selectedPathText[0] = transform.GetComponentInChildren<Text>();
    }

    public void ShowButtonInfo()
    {
        StartCoroutine(_ShowButtonInfo());
    }

    IEnumerator _ShowButtonInfo()
    {
        pathInfoPanel.GetComponentInChildren<Text>().text = "출발지 :" + pathData.departure + "   도착지 :" + pathData.destination;
        pathInfoPanel.SetActive(true);
        yield return new WaitForSeconds(3); //3초 동안 정보 보여주고 숨김.
        pathInfoPanel.SetActive(false);
    }



}
