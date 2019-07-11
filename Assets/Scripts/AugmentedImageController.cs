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

    private void Start()
    {
#if UNITY_EDITOR
        parentInstances = Instantiate(parentPrefab, Camera.main.transform.position, Quaternion.identity);
#endif

    }

#if UNITY_ANDROID
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        //TrackableQueryFilter.Updated는 해당 프레임에 위치, 상태 등이 갱신된 증강이미지들이 들어가는데, 갱신되는 순서는 랜덤이니 랜덤하게 currentDetectedAugementedImages에 들어감.
        Session.GetTrackables<AugmentedImage>(currentDetectedAugementedImages, TrackableQueryFilter.Updated);

        foreach (var image in currentDetectedAugementedImages)
        {
            console.text = "Session:" + Session.Status.ToString() + image.Name + ": " + image.TrackingState.ToString();

            //새로운 이미지가 트래킹 상태라면
            if (image.TrackingState == TrackingState.Tracking && !dataBaseIndex.Contains(image.DatabaseIndex))
            {
                parentInstances = Instantiate(parentPrefab);
                console2.text = "이미지 인식 횟수: " + callCount++;
                parentInstances.name = "parent" + callCount;
                Anchor anchor = image.CreateAnchor(image.CenterPose);
                parentInstances.transform.parent = anchor.transform;
                parentInstances.transform.localPosition = new Vector3(0, 0, 0);
                parentInstances.transform.rotation = Quaternion.identity;
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
