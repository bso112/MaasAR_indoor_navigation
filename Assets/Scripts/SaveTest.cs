using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveTest : MonoBehaviour
{
    public GameObject spwanObj, player;

    public void Save()
    {
        Vector3 playerPosition = player.transform.position;
        Vector3 initPosition = new Vector3(playerPosition.x, playerPosition.y, playerPosition.z + 0.5f);
        GameObject original = Instantiate(spwanObj, initPosition, Quaternion.identity);
        Vector3 playerVector = original.transform.position;
        PlayerPrefs.SetFloat("x", playerVector.x);
        PlayerPrefs.SetFloat("y", playerVector.y);
        PlayerPrefs.SetFloat("z", playerVector.z);

    }

    public void Load()
    {
        Vector3 newPosition = new Vector3
        {
            x = PlayerPrefs.GetFloat("x"),
            y = PlayerPrefs.GetFloat("y"),
            z = PlayerPrefs.GetFloat("z")
        };

        Instantiate(spwanObj, newPosition, Quaternion.identity);

    }  
}
