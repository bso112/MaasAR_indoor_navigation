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
<<<<<<< Updated upstream
public class PathSpawner : MonoBehaviour
{

    public static PathSpawner Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        Instance = this;

    }



=======
public class PathSpawner : Singleton<PathSpawner>
{

>>>>>>> Stashed changes
    /// <summary>
    /// 경로 하나에 대한 정보
    /// </summary>
    public PathData pathData = new PathData();
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
    /// 하나의 경로
    /// </summary>
    protected GameObject path;
<<<<<<< Updated upstream
    /// <summary>
    /// 경로 리스트
    /// </summary>
    protected List<GameObject> paths = new List<GameObject>();
=======
>>>>>>> Stashed changes

    public AugmentedImageController augmentedImageController;
    public Text console;

    IEnumerator SpawnObjectPerSecond()
    {
        while (true)
        {
            Instantiate(pathObject, player.transform.position, Quaternion.identity);
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

<<<<<<< Updated upstream
        GameObject parent = augmentedImageController.parentInstances[0];
=======
        GameObject parent = augmentedImageController.GetPathParent()[0];
>>>>>>> Stashed changes
        foreach (var pathObj in GameObject.FindGameObjectsWithTag("pathObject"))
        {
            pathObj.transform.SetParent(parent.transform);
        }
        //경로의 부모(파란 경로 오브젝트)를 포함한 경로를 path에 넣는다.
        path = parent;
<<<<<<< Updated upstream

        if (path != null)
        {
            paths.Add(path);
        }
=======
        
        //라우팅 테이블을 실시간으로 업데이트한다.(경로 생성 될때마다)
        PathRouter.Instance.UpdatePathList(pathName, path);
>>>>>>> Stashed changes

        //로컬에 경로를 저장한다.
        SavePath(pathName);
        //현재 만든 경로를 숨긴다.
        path.SetActive(false);
        //버튼을 교체한다.
        createMapBtn.gameObject.SetActive(false);
        generateMapBtn.gameObject.SetActive(true);
    }

    /// <summary>
    /// 경로를 불러온다.
    /// </summary>
    public void LoadPath(Text InputText)
    {
        //저장하고 (게임 다시시작 안하고) 바로 불러오기 할 경우, 단지 숨겼던 경로를 보여준다.
        if (path != null)
        {
            path.SetActive(true);
            return;
        }
        string pathName = InputText.text;

        PathData pathData = new PathData();
        GameObject parent = augmentedImageController.GetPathParent()[0];
        if (File.Exists(Application.persistentDataPath + "/" + pathName + ".dat")) //비어있지 않으면 로드!

        {

            var b = new BinaryFormatter(); //바이너리 포맷터

            var f = File.Open(Application.persistentDataPath + "/" + pathName + ".dat", FileMode.Open); // 파일 열기.

            pathData = (PathData)b.Deserialize(f); //스코어를 로드. 디 시리얼라이즈.

            f.Close(); //파일 닫기.

        }

        for (int i = 0; i < pathData.childPositions.Count; i++)
        {
            Debug.Log(pathData.childPositions[i].ToString());
            GameObject obj = Instantiate(pathObject);
            obj.name = "Loaded Path";
            obj.transform.SetParent(parent.transform);
            obj.transform.localPosition = pathData.childPositions[i];
        }

    }

    //길을 세이브한다.
    public void SavePath(string pathName)
    {

        //pathData.path = path;
        pathData.pathName = pathName;
        foreach (var child in path.transform.GetComponentsInChildren<Transform>())
        {
            //parent 오브젝트가 아니고, 자식이 있는 오브젝트면 위치를 저장한다.(path 프리펩의 선글라스 위치까지 저장 안하기 위해) 
            if (child.transform != path.transform && child.transform.childCount > 0)
            {
                pathData.childPositions.Add(child.transform.localPosition);
            }

        }

        var b = new BinaryFormatter(); //BinartFormatter를 받아옴
        var f = File.Create(Application.persistentDataPath + "/" + pathName + ".dat"); //파일을 생성.
        b.Serialize(f, pathData); // 경로정보 저장.
        console.text = Application.persistentDataPath + "에 저장되었습니다.";



        f.Close();



    }


}