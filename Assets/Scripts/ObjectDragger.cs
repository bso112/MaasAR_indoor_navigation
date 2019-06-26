using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GoogleARCore;


public class ObjectDragger : MonoBehaviour
{
    public Text console;

    public Slider Zaxis;

    private GameObject obj;
    Color OriginalColor;

    private List<DetectedPlane> currentDetectedPlanes = new List<DetectedPlane>();

    public bool isObjSelected = false;

    private void Start()
    {
        obj = new GameObject();
        obj.AddComponent<MeshRenderer>().material.color = Color.red;
        OriginalColor = Color.gray;

    }
    // Update is called once per frame
    void Update()
    {
        Session.GetTrackables<DetectedPlane>(currentDetectedPlanes, TrackableQueryFilter.New);

        if (!Input.GetMouseButtonDown(0)) return;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //if (Input.touchCount <= 0) return;
        //Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);

        if (Frame.Raycast(ray.origin, ray.direction, out TrackableHit result))
        {
            obj.transform.position = result.Pose.position;
        }



        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.transform.tag == "pathObject")
            {
                console.text = "터치된 객체: " + hit.transform.name;

                //선택한 오브젝트가 전에 선택한 오브젝트와 다를 때
                if (hit.transform.gameObject != obj)
                {
                    obj.GetComponent<MeshRenderer>().material.color = OriginalColor;
                    obj = hit.transform.gameObject;
                    if (obj.GetComponent<MeshRenderer>().material.color != Color.green)
                        OriginalColor = obj.GetComponent<MeshRenderer>().material.color;
                }

                //선택한 오브젝트가 뭐든지 간에 실행
                if (obj.GetComponent<MeshRenderer>().material.color == Color.green)
                    obj.GetComponent<MeshRenderer>().material.color = OriginalColor;
                else
                    obj.GetComponent<MeshRenderer>().material.color = Color.green;

            }
        }
    }

    public void BackAndFoward(Slider slider)
    {
        Vector3 newPosition = obj.transform.position;
        newPosition.z += slider.value;
        obj.transform.position = newPosition;

    }

    public void Drag(string direction)
    {
        Vector3 newPosition = obj.transform.position;
        switch (direction)
        {
            case "UP":
                newPosition.y += 0.05f;
                obj.transform.position = newPosition;
                break;
            case "DOWN":
                newPosition.y -= 0.05f;
                obj.transform.position = newPosition;
                break;
            case "LEFT":
                newPosition.x -= 0.05f;
                obj.transform.position = newPosition;
                break;
            case "RIGHT":
                newPosition.x += 0.05f;
                obj.transform.position = newPosition;
                break;

        }
    }
}

