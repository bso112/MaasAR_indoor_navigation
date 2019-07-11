using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GoogleARCore;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary; //Formatters 쓸려고..

/// <summary>
/// 경로를 생성하는 클래스
/// </summary>
public class PathSpawner : Singleton<PathSpawner>
{


    /// <summary>
    /// UI들.
    /// </summary>
    public Button generateMapBtn, createMapBtn, loadMapBtn;
    /// <summary>
    /// 경로를 표시할 오브젝트
    /// </summary>
    public GameObject pathObject;
    /// <summary>
    /// pathObject가 따라다닐 플레이어.
    /// </summary>
    public GameObject player;
    /// <summary>
    /// 부모 오브젝트
    /// </summary>
    private GameObject parent;

    public AugmentedImageController augmentedImageController;
    public Text console;
    public Text console7;

    public Text InputDeparture;
    public Text InputDestination;


    IEnumerator SpawnObjectPerSecond()
    {
        //부모 오브젝트를 생성
        parent = augmentedImageController.GetPathParentClone();
        while (true)
        {   
            Instantiate(pathObject, player.transform.position, player.transform.rotation).transform.SetParent(parent.transform); //만들어진 pathObject를 parent의 자식으로 한다.
            console7.text = "플레이어 로테이션: " + player.transform.rotation.ToString();
            yield return new WaitForSeconds(1.0f);
        }
    }
    /// <summary>
    /// 경로를 만들기 시작한다.
    /// </summary>
    public void GeneratePath()
    {
        StartCoroutine("SpawnObjectPerSecond");
        generateMapBtn.gameObject.SetActive(false);
        createMapBtn.gameObject.SetActive(true);
    }


    /// <summary>
    /// 경로 만들기를 종료하고 경로 목록에 저장한다. 
    /// </summary>
    public void CreatePath(Text InputText)
    {   
        string pathName = InputText.text;

        //로컬에 경로를 저장한다.
        SavePath(parent, pathName);

        //만든 경로를 숨긴다.
        parent.SetActive(false); 
        //버튼을 교체한다.
        createMapBtn.gameObject.SetActive(false);
        generateMapBtn.gameObject.SetActive(true);
    }

    /// <summary>
    /// 경로를 불러온다.
    /// </summary>
    public GameObject LoadPath(Text InputText)
    {

        string pathName = InputText.text;

        PathData pathData = new PathData();
        GameObject parent = augmentedImageController.GetPathParentClone();
        if (File.Exists(Application.persistentDataPath + "/" + pathName + ".dat")) //비어있지 않으면 로드!

        {

            var b = new BinaryFormatter(); //바이너리 포맷터

            var f = File.Open(Application.persistentDataPath + "/" + pathName + ".dat", FileMode.Open); // 파일 열기.

            pathData = (PathData)b.Deserialize(f); //스코어를 로드. 디 시리얼라이즈.

            f.Close(); //파일 닫기.

        }

        for (int i = 0; i < pathData.childPositions.Count; i++)
        {
            GameObject obj = Instantiate(pathObject);
            obj.name = "Loaded Path";
            obj.transform.SetParent(parent.transform);
            obj.transform.localPosition = pathData.childPositions[i];
        }

        return parent;
    }

    //길을 세이브한다.
    public void SavePath(GameObject parent, string pathName)
    {
        PathData pathData = new PathData();
        pathData.pathName = pathName;
        pathData.departure = InputDeparture.text;
        pathData.destination = InputDestination.text;

        foreach (var child in parent.transform.GetComponentsInChildren<Transform>())
        {
            //path 오브젝트가 아니고, 자식이 있는 오브젝트면 위치를 저장한다.(path 프리펩의 선글라스 같은 말단 오브젝트의 위치는 저장 안함) 
            if (child.transform != parent.transform && child.transform.childCount > 0)
            {
                pathData.childPositions.Add(child.transform.localPosition);
            }

        }

        var b = new BinaryFormatter(); //BinartFormatter를 받아옴
        var f = File.Create(Application.persistentDataPath + "/" + pathName + ".dat"); //파일을 생성.
        b.Serialize(f, pathData); // 경로정보 저장.
        console.text = Application.persistentDataPath + "에 저장되었습니다.";
        Debug.Log(Application.persistentDataPath + "에 저장되었습니다.");
        PathRouter.Instance.AddPathData(pathData, name); //라우팅테이블 갱신

        f.Close();



    }


}
