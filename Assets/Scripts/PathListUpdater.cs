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
    /// 사용자가 선택한 경로이름
    /// </summary>
    [HideInInspector] public static Text SelectedPathText;


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
        if (SelectedPathText != null)
        {
            string pathName = SelectedPathText.text;

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

            Destroy(SelectedPathText.transform.parent.gameObject); //해당 버튼을 파괴한다.
        }

    }

    public void ConfirmPathSelection()
    {
        if (SelectedPathText != null)
            PathSpawner.Instance.LoadPath(SelectedPathText);
    }

}
