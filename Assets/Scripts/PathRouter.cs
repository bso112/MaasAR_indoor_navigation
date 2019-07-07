using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.UI;

public class PathRouter : Singleton<PathRouter>
{
    private List<PathData> pathDatas = new List<PathData>();
    /// <summary>
    /// pathDatas(라우팅 테이블)에 요소를 추가한다.
    /// </summary>
    /// <param name="pathData"></param>
    /// <param name="callerName">메서드를 호출하는 스크립트 이름</param>
    public void AddPathData(PathData pathData, string caller)
    {
        Debug.Log(caller + "가 pathData를 호출했습니다.");
        pathDatas.Add(pathData);
    }
    
    public Text console;


    // pathDatas(라우팅 테이블)을 초기화한다.
    void Start()
    {
        //폴더에 있는 경로들 가져오기
        string[] files = Directory.GetFiles(Application.persistentDataPath, $"*.dat");

        BinaryFormatter b = new BinaryFormatter();
        FileStream f;
        foreach (var file in files)
        {
            if(File.Exists(file))
            {   
                Debug.Log("처음 불러온 경로목록: " + file);
                f = File.Open(file, FileMode.Open);
                pathDatas.Add((PathData)b.Deserialize(f));
                f.Close();
            }
        }


    }
    

    /// <summary>
    /// 두 개의 PathData를 더하고 그 결과를 저장한다.
    /// </summary>
    /// <param name="pathA"></param>
    /// <param name="pathB"></param>
    /// <param name="pathName"></param>
    public void JoinAndSavePath(GameObject parentA, GameObject parentB, string pathName)
    {
        GameObject parent = JoinTwoPathData(parentA, parentB);
        PathSpawner.Instance.SavePath(parent, pathName);
    }


    /// <summary>
    /// 두 개의 PathData를 더한다.
    /// </summary>
    /// <param name="pathA"></param>
    /// <param name="pathB"></param>
    /// <returns></returns>
    public GameObject JoinTwoPathData(GameObject parentA, GameObject parentB)
    {
        parentB.transform.SetParent(parentA.transform.GetChild(parentA.transform.childCount-1)); // parentA 경로의 맨 마지막 오브젝트를 parntB의 부모로 삼는다.
        parentB.transform.localPosition = Vector3.zero;
        parentB.transform.rotation = Quaternion.identity;
        foreach(var child in parentB.transform.GetComponentsInChildren<Transform>())
        {   
            if(child != parentB.transform && child.transform.childCount > 0)
            {
                child.SetParent(parentA.transform);
            }
            
        }
        return parentA;
    }

}
