using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class PathRouter : Singleton<PathRouter>
{

    public Dictionary<string, GameObject> paths = new Dictionary<string, GameObject>();


    private string[] files;

    // Start is called before the first frame update
    void Start()
    {   
        //폴더에 있는 경로들 가져오기
        files = Directory.GetFiles(Application.persistentDataPath, $"*.dat");
        List<PathData> pathDatas = new List<PathData>();

        BinaryFormatter b = new BinaryFormatter();
        FileStream f;
        foreach (var file in files)
        {
            if(File.Exists(file))
            {   
                Debug.Log("처음 불러온 경로목록: " + file);
                f = File.Open(file, FileMode.Open);
                pathDatas.Add((PathData)b.Deserialize(f));
                f.Close();
            }
        }

    }

    public void UpdatePathList(string pathName, GameObject path)
    {
        paths.Add(pathName, path);
    }

}
