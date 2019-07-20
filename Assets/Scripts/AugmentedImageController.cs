using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;
using UnityEngine.UI;

public class AugmentedImageController : MonoBehaviour
{
    public GameObject ARCoreDevice;
    private List<AugmentedImage> currentDetectedAugementedImages = new List<AugmentedImage>();
    [SerializeField] private GameObject parentPrefab;
    private GameObject parentInstances;
    public GameObject GetPathParentClone()
    {
        if (parentInstances == null)
        { Debug.Log("경로의 부모오브젝트가 없습니다"); console4.text = "경로의 부모오브젝트가 없습니다"; }
        else
        {
            return Instantiate(new GameObject("parentInstanceClone"), parentInstances.transform.position, parentInstances.transform.rotation);
        }
        return null;
    }
    public Text console;
    public Text console2;
    public Text console4;
    public Text console8;

    private int callCount = 0;

    /// <summary>
    /// 전에 이미 인식된 이미지들의 리스트
    /// </summary>
    private List<int> dataBaseIndex = new List<int>();


    GameObject player;



    

    private void Start()
    {
#if UNITY_EDITOR
        parentInstances = Instantiate(parentPrefab, Camera.main.transform.position, Quaternion.identity);
#endif

        player = PathSpawner.Instance.player;
        

    }




#if UNITY_ANDROID
    void Update()
    {   

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
   
        Session.GetTrackables<AugmentedImage>(currentDetectedAugementedImages, TrackableQueryFilter.Updated);

        

        foreach (var image in currentDetectedAugementedImages)
        {


            //새로운 이미지가 트래킹 상태라면
            if (image.TrackingState == TrackingState.Tracking && !dataBaseIndex.Contains(image.DatabaseIndex))
            {   
                
                parentInstances = Instantiate(parentPrefab, player.transform.position, player.transform.rotation);
                parentInstances.name = "parent";
                dataBaseIndex.Add(image.DatabaseIndex);

            }
        }



    }





#endif

    public void RestartSession()
    {
        StartCoroutine(_RestartSession());
    }

    IEnumerator _RestartSession()
    {
        console4.text = "세션 재시작";
        ARCoreSession session = ARCoreDevice.GetComponent<ARCoreSession>();
        ARCoreSessionConfig myConfig = session.SessionConfig;

        //DestroyImmediate(session);
        Destroy(session);

        yield return null;

        session = ARCoreDevice.AddComponent<ARCoreSession>();
        session.SessionConfig = myConfig;
        session.enabled = true;
        dataBaseIndex.Clear();
        callCount = 0;
    }

}
