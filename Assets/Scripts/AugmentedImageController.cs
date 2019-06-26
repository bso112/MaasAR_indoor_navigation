using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;
using UnityEngine.UI;

public class AugmentedImageController : MonoBehaviour
{
    private List<AugmentedImage> currentDetectedAugementedImages = new List<AugmentedImage>();
    [SerializeField] private GameObject parent;
    [HideInInspector] public List<GameObject> parentInstances = new List<GameObject>();

    public Text console;
    public Text console2;
    public Text console4;
    private bool flag = false;


    private void Start()
    {
#if UNITY_EDITOR
        parentInstances.Add(Instantiate(parent, Camera.main.transform.position, Quaternion.identity));
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


            if (image.TrackingState == TrackingState.Tracking && !flag)
            {
                console4.text = "이미지 너비 : " + image.ExtentX + "이미지 높이 : " + image.ExtentZ;

                Anchor anchor = image.CreateAnchor(image.CenterPose);
                parent.transform.parent = anchor.transform;
                parent.transform.localPosition = new Vector3(0, 0, 0);
                Vector3 position = parent.transform.position;

                console2.text = "부모 포지션 : " + position.ToString() + "부모 로테이션 : " + parent.transform.rotation.ToString();

                parentInstances.Add(Instantiate(parent, parent.transform.position, Quaternion.identity));

                flag = true;
            }
        }
    }
#endif
}
