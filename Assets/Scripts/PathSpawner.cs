using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GoogleARCore;

/// <summary>
/// 경로를 생성하는 클래스
/// </summary>
public class PathSpawner : MonoBehaviour
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
    /// 하나의 경로
    /// </summary>
    protected GameObject path; 
    /// <summary>
    /// 경로 리스트
    /// </summary>
    protected List<GameObject> paths = new List<GameObject>(); 

    public AugmentedImageController augmentedImageController;
    public Text console2;

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
        
        Vector3[] newPositions = new Vector3[100]; //자식들의 세이브데이터에서 받아와 위치를 저장할 벡터
        GameObject parent = augmentedImageController.parentInstances[0];

        int length = PlayerPrefs.GetInt("length");

        for (int i = 0; i < length; i++)
        {
            newPositions[i].x = PlayerPrefs.GetFloat("x" + i);
            newPositions[i].y = PlayerPrefs.GetFloat("y" + i);
            newPositions[i].z = PlayerPrefs.GetFloat("z" + i);
            GameObject obj = Instantiate(pathObject);
            obj.transform.SetParent(parent.transform);
            obj.transform.localPosition = newPositions[i];
        }

        
    }

    //길을 세이브한다.
    public void SavePath()
    {
        int i = 0;
        foreach (var child in path.transform.GetComponentsInChildren<Transform>())
        {   
            //부모가 아니면 실행한다.
            if(child.transform != path.transform)
            {
                PlayerPrefs.SetFloat("x" + i, child.localPosition.x);
                PlayerPrefs.SetFloat("y" + i, child.localPosition.y);
                PlayerPrefs.SetFloat("z" + i, child.localPosition.z);
                i++;
            }

        }
        PlayerPrefs.SetInt("length", i);

        
    }


}
