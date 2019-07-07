using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System;

/// <summary>
/// PathList의 항목을 업데이트를 해주는 클래스
/// </summary>
public class PathListUpdater : MonoBehaviour
{

    /// <summary>
    /// 경로 리스트에서 경로 버튼이 들어갈 컨텐트
    /// </summary>
    public GameObject pathContent;
    /// <summary>
    /// 경로 컨텐트에 들어갈 경로 버튼 프리팹
    /// </summary>
    public Button pathButtonPrefab;

    /// <summary>
    /// 로컬에 저장된 경로들
    /// </summary>
    private string[] files;

    /// <summary>
    /// 오류를 띄워줄 콘솔
    /// </summary>
    public Text console;

    /// <summary>
    /// 사용자가 선택한 경로이름(경로 합치기를 위해 2개까지 선택 가능)
    /// </summary>
    [HideInInspector] public static Text[] selectedPathText = new Text[2];


    private void Start()
    {
        files = Directory.GetFiles(Application.persistentDataPath, $"*.dat");
    }
    /// <summary>
    /// 경로 리스트를 업데이트한다. 맵 불러오기 버튼을 누르면 실행한다. 
    /// </summary>
    public void UpdatePathList()
    {
        files = Directory.GetFiles(Application.persistentDataPath, $"*.dat");

        if (files != null)
        {
            //현재 컨텐트에 있는 목록을 받아온다.
            Text[] _texts = pathContent.transform.GetComponentsInChildren<Text>();
            List<string> texts = new List<string>();
            for (int i = 0; i < _texts.Length; i++)
            {
                texts.Add(_texts[i].text);
            }

            //컨텐트를 탐색해 name과 같은 게 없으면 버튼 프리팹을 맵 컨텐트에 추가하고, 버튼 밑 텍스트의 내용을 fileName으로 한다
            foreach (var name in files)
            {
                string fileName = name.Replace(Application.persistentDataPath + @"\", "");
                fileName = fileName.Replace(".dat", "");

                if (pathContent.transform.childCount <= 0)
                    Instantiate(pathButtonPrefab, pathContent.transform).transform.GetComponentInChildren<Text>().text = fileName;
                else if (!texts.Contains(fileName))
                {
                    Instantiate(pathButtonPrefab, pathContent.transform).transform.GetComponentInChildren<Text>().text = fileName;
                }



            }
        }
    }

    public void JoinTwoPathAndSave(Text inputText)
    {
        if(selectedPathText.Length >= 2)
        {
            GameObject parentA = PathSpawner.Instance.LoadPath(selectedPathText[0]);
            GameObject parentB = PathSpawner.Instance.LoadPath(selectedPathText[1]);
            PathRouter.Instance.JoinAndSavePath(parentA, parentB, inputText.text);
        }
        
    }

    public void DeleteAllPaths()
    {
        if (files != null)
        {
            foreach (var file in files)
            {
                File.Delete(file);

            }
        }

        if (pathContent.transform.childCount > 0)
        {
            for (int i = 0; i < pathContent.transform.childCount; i++)
            {
                Destroy(pathContent.transform.GetChild(i).gameObject); //모든 버튼을 파괴한다.
            }
        }



    }

    public void DeletePath()
    {
        if (selectedPathText != null)
        {
            string pathName = selectedPathText[0].text;

            try
            {
                string file = Application.persistentDataPath + "/" + pathName + ".dat";
                File.Delete(file);
            }
            catch (Exception e)
            {
                console.text = e.Message;
                Debug.Log(e.Message);
            }

            Destroy(selectedPathText[0].transform.parent.gameObject); //해당 버튼을 파괴한다.
        }

    }

    public void ConfirmPathSelection()
    {
        if (selectedPathText != null)
            PathSpawner.Instance.LoadPath(selectedPathText[0]);
    }

}
