using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathObject : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "pathObject" && other.name != "Parent")
        {
            Destroy(other);
        }
    }
}
