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

    /// <summary>
    /// 이 조건 안에 들어오는게 전에 실행 됬었는가(조건문 안의 코드를 한번만 실행하기 위한 단순 플래그)
    /// </summary>
    private bool isExBefore = false;

    private void Start()
    {
#if UNITY_EDITOR
        parentInstances = Instantiate(parentPrefab, Camera.main.transform.position, Quaternion.identity);
#endif
    }

    int callCount = 0;
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
            console.text = "Session:" + Session.Status.ToString() + image.Name + ": " + image.TrackingState.ToString();



            if (image.TrackingState == TrackingState.Tracking && !isExBefore)
            {

                console2.text = "이미지 인식 횟수: " + callCount++;
                parentInstances = Instantiate(parentPrefab);
                Anchor anchor = image.CreateAnchor(image.CenterPose);
                parentInstances.transform.parent = anchor.transform;
                parentInstances.transform.localPosition = new Vector3(0, 0, 0);
                parentInstances.transform.rotation = Quaternion.identity;
                isExBefore = true; //실행 됬다.
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
        isExBefore = false;

    }
}
