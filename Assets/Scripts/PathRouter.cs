using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.UI;

public class PathRouter : Singleton<PathRouter>
{
    private List<PathData> pathDatas = new List<PathData>();
    /// <summary>
    /// pathDatas(라우팅 테이블)에 요소를 추가한다.
    /// </summary>
    /// <param name="pathData"></param>
    /// <param name="callerName">메서드를 호출하는 스크립트 이름</param>
    public void AddPathData(PathData pathData, string caller)
    {
        Debug.Log(caller + "가 pathData를 호출했습니다.");
        pathDatas.Add(pathData);
    }

    public Text console;


    // pathDatas(라우팅 테이블)을 초기화한다.
    void Start()
    {
        //폴더에 있는 경로들 가져오기
        string[] files = Directory.GetFiles(Application.persistentDataPath, $"*.dat");

        BinaryFormatter b = new BinaryFormatter();
        FileStream f;
        foreach (var file in files)
        {
            if (File.Exists(file))
            {
                Debug.Log("처음 불러온 경로목록: " + file);
                f = File.Open(file, FileMode.Open);
                pathDatas.Add((PathData)b.Deserialize(f));
                f.Close();
            }
        }


    }


    /// <summary>
    /// 두 개의 PathData를 더하고 그 결과를 저장한다.
    /// </summary>
    /// <param name="pathA"></param>
    /// <param name="pathB"></param>
    /// <param name="pathName"></param>
    public void JoinAndSavePath(GameObject parentA, GameObject parentB, string pathName)
    {
        GameObject parent = JoinTwoPathData(parentA, parentB);
        PathSpawner.Instance.SavePath(parent, pathName);
    }


    /// <summary>
    /// 두 개의 PathData를 더한다.
    /// </summary>
    /// <param name="pathA"></param>
    /// <param name="pathB"></param>
    /// <returns></returns>
    public GameObject JoinTwoPathData(GameObject parentA, GameObject parentB)
    {
        parentB.transform.SetParent(parentA.transform.GetChild(parentA.transform.childCount - 1)); // parentA 경로의 맨 마지막 오브젝트를 parntB의 부모로 삼는다.
        parentB.transform.localPosition = Vector3.zero;
        parentB.transform.rotation = Quaternion.identity;
        foreach (var child in parentB.transform.GetComponentsInChildren<Transform>())
        {
            if (child != parentB.transform && child.transform.childCount > 0)
            {
                child.SetParent(parentA.transform);
            }

        }
        return parentA;
    }

    public Graph<string> DrawGraph(List<PathData> pathDatas)
    {   

        //init graph
        Graph<string> graph = new Graph<string>();
        GraphNode<string> nodeA = new GraphNode<string>();
        GraphNode<string> nodeB = new GraphNode<string>();
        List<string> nodes = new List<string>();

        string temp = ""; //반복문에서 pathData.departure가 중복되는지 확인하기 위한 임시변수. 
        foreach (var pathData in pathDatas)
        {
            string departure = pathData.departure;
            string destination = pathData.destination;

            if (!nodes.Contains(departure))
            {
                nodeA = graph.AddNode(departure);
                nodes.Add(nodeA.Data);
            }
            if(!nodes.Contains(destination))
            {
                nodeB = graph.AddNode(destination);
                nodes.Add(nodeB.Data);
            }

            graph.AddEdge(nodeA, nodeB, true, pathData.childPositions.Count);
        }
        //그래프를 출력한다.
        graph.DebugPrintLinks();

        return graph;
    }

    public void Djikstra(string departure, string destination, Graph<string> graph)
    {   
        int[] dist = new int[graph.NodeCount()];
        List<GraphNode<string>> found = new List<GraphNode<string>>();
        int[] path = new int[graph.NodeCount()];
        GraphNode<string> depNode = graph.SearchNode(departure);

        int minIndex = Choos_vertex(depNode);

        int i, u, w;

        for(i =0; i<graph.NodeCount(); i++)
        {
        }
            
    }

    /// <summary>
    /// 해당 노드의 이웃으로 가는 경로 중 가장 가중치가 낮은 경로의 인덱스를 반환한다. 찾지못하면 -1을 반환한다.
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    private int Choos_vertex(GraphNode<string> node)
    {
        int min = int.MaxValue;
        int minIndex = -1;
        for(int i = 0; i<node.Neighbors.Count; i++)
        {
            if(node.Weights[i] < min)
            {
                min = node.Weights[i];
            }
        }
        return minIndex;
    }

}
