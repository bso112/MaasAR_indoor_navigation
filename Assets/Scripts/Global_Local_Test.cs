using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global_Local_Test : MonoBehaviour
{
    public GameObject parent, parent2, children;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("월드포지션 : " + children.transform.position + "로컬포지션: " + children.transform.localPosition);
             
    }

    public void MoveLocal()
    {
        GameObject children = Instantiate(this.children);
        children.transform.SetParent(parent2.transform);
        children.transform.position = this.children.transform.localPosition;
        
    }

    public void MoveWorld()
    {
        GameObject children = Instantiate(this.children, this.children.transform.position, this.children.transform.rotation);
        children.transform.SetParent(parent2.transform);
    }
    
}
