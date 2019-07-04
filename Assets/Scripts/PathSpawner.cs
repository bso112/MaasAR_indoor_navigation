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
public class PathSpawner : MonoBehaviour
{   
    [System.Serializable]
    public class PathData
    {   
        /// <summary>
        /// 경로 이름
        /// </summary>
        public string pathName = "noName";
        /// <summary>
        /// 자식 오브젝트들의 위치
        /// </summary>
        public List<Vector.SerializableVector3> childPositions = new List<Vector.SerializableVector3>();
    }

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
    /// <summary>
    /// 경로 리스트
    /// </summary>
    protected List<GameObject> paths = new List<GameObject>(); 

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
    public void CreatePath()
    {
        StopCoroutine("SpawnObjectPerSecond");

        
        GameObject parent = augmentedImageController.parentInstances[0];
        foreach (var pathObj in GameObject.FindGameObjectsWithTag("pathObject"))
        {
            pathObj.transform.SetParent(parent.transform);
        }

        path = parent;
        
        if(path != null)
        {
            paths.Add(path);
        }

        SavePath();
        path.SetActive(false);
        //버튼을 교체한다.
        createMapBtn.gameObject.SetActive(false);
        generateMapBtn.gameObject.SetActive(true);
    }

    /// <summary>
    /// 경로를 불러온다.
    /// </summary>
    public void LoadPath()
    {
        //Vector3[] newPositions = new Vector3[100]; //자식들의 세이브데이터에서 받아와 위치를 저장할 벡터
        //GameObject parent = augmentedImageController.parentInstances[0];

        //int length = PlayerPrefs.GetInt("length");

        //for (int i = 0; i < length; i++)
        //{
        //    newPositions[i].x = PlayerPrefs.GetFloat("x" + i);
        //    newPositions[i].y = PlayerPrefs.GetFloat("y" + i);
        //    newPositions[i].z = PlayerPrefs.GetFloat("z" + i);
        //    GameObject obj = Instantiate(pathObject);
        //    obj.transform.SetParent(parent.transform);
        //    obj.transform.localPosition = newPositions[i];
        //}


        PathData pathData = new PathData();
        GameObject parent = augmentedImageController.parentInstances[0];
        if (File.Exists(Application.persistentDataPath + "/pathData.dat")) //비어있지 않으면 로드!

        {

            var b = new BinaryFormatter(); //바이너리 포맷터

            var f = File.Open(Application.persistentDataPath + "/pathData.dat", FileMode.Open); // 파일 열기.

            pathData = (PathData)b.Deserialize(f); //스코어를 로드. 디 시리얼라이즈.

            f.Close(); //파일 닫기.

        }

        for (int i = 0; i < pathData.childPositions.Count; i++)
        {
            Debug.Log(pathData.childPositions[i].ToString());
            GameObject obj = Instantiate(pathObject);
            obj.transform.SetParent(parent.transform);
            obj.transform.localPosition = pathData.childPositions[i];
        }

    }

    //길을 세이브한다.
    public void SavePath()
    {
        //int i = 0;
        //foreach (var child in path.transform.GetComponentsInChildren<Transform>())
        //{   
        //    //부모가 아니면 실행한다.
        //    if(child.transform != path.transform)
        //    {
        //        PlayerPrefs.SetFloat("x" + i, child.localPosition.x);
        //        PlayerPrefs.SetFloat("y" + i, child.localPosition.y);
        //        PlayerPrefs.SetFloat("z" + i, child.localPosition.z);
        //        i++;
        //    }

        //}
        //PlayerPrefs.SetInt("length", i);
        //PlayerPrefs.Save();


        //파일 저장 테스트
        pathData.pathName = "테스트";
        foreach (var child in path.transform.GetComponentsInChildren<Transform>())
        {
            //부모가 아니면 실행한다.
            if (child.transform != path.transform)
            {
                pathData.childPositions.Add(child.transform.localPosition);
            }

        }

        var b = new BinaryFormatter(); //BinartFormatter를 받아옴
        var f = File.Create(Application.persistentDataPath + "/pathData.dat"); //파일을 생성.
        b.Serialize(f, pathData); // 경로정보 저장.
        console.text = Application.persistentDataPath + "에 저장되었습니다.";

        f.Close();



    }


    public void DeleteAllMap()
    {
        PlayerPrefs.DeleteAll();

    }


}
