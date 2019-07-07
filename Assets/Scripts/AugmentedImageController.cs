using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;
using UnityEngine.UI;

public class AugmentedImageController : MonoBehaviour
{
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
    private bool flag = false;


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

        Session.GetTrackables<AugmentedImage>(currentDetectedAugementedImages, TrackableQueryFilter.Updated);

        foreach (var image in currentDetectedAugementedImages)
        {
            console.text = "Session:" + Session.Status.ToString() + image.Name + ": " + image.TrackingState.ToString();



            if (image.TrackingState == TrackingState.Tracking)
            {
                Anchor anchor = image.CreateAnchor(image.CenterPose);
                parentPrefab.transform.parent = anchor.transform;
                parentPrefab.transform.localPosition = new Vector3(0, 0, 0);
                Vector3 position = parentPrefab.transform.position;
                if (!flag)
                {
                    parentInstances = Instantiate(parentPrefab, parentPrefab.transform.position, Quaternion.identity);
                    flag = true;
                }
                console4.text = "이미지 너비 : " + image.ExtentX + "이미지 높이 : " + image.ExtentZ;
                console2.text = "부모 포지션 : " + position.ToString() + "부모 로테이션 : " + parentPrefab.transform.rotation.ToString();



            }
        }
    }
#endif
}
