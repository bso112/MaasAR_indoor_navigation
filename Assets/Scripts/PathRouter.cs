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

    private Graph<PathData> graph;


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


    public void RoutePath(Text departure)
    {
        GraphNode<PathData> start = new GraphNode<PathData>();
        foreach(var node in graph.nodeList)
        {
            if(node.Data.departure == departure.text)
            {
                start = node;
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

    //public int[,] DrawGraph(List<PathData> pathDatas)
    //{
    //    MatrixGraph graph = new MatrixGraph();
    //    graph.init_graph();
    //    List<string> nodes = new List<string>();

    //    foreach (var pathData in pathDatas)
    //    {
    //        string departure = pathData.departure;
    //        string destination = pathData.destination;

    //        if (!nodes.Contains(departure))
    //        {
    //            nodes.Add(departure);
    //            graph.insert_vertex(pathData);
    //        }
    //        if (!nodes.Contains(destination))
    //        {
    //            nodes.Add(destination);
    //            graph.insert_vertex(pathData);
    //        }
    //    }


    //    for (int i = 0; i < nodes.Count; i++)
    //    {
    //        for (int i = 0; i < nodes.Count; i++)
    //        {
    //            graph.insert_edge2
    //        }
    //    }
    //}

    //public Graph<PathData> DrawGraph(List<PathData> pathDatas)
    //{

    //    Graph<PathData> graph = new Graph<PathData>();
    //    //pathData에서 노드를 솎아내기 위한 리스트
    //    List<string> nodes = new List<string>();

    //    GraphNode<PathData> nodeA = new GraphNode<PathData>();
    //    GraphNode<PathData> nodeB = new GraphNode<PathData>();

    //    foreach (var pathData in pathDatas)
    //    {
    //        string departure = pathData.departure;
    //        string destination = pathData.destination;

    //        if (!nodes.Contains(departure))
    //        {
    //            nodeA.Data = pathData;
    //            nodes.Add(departure);
    //        }
    //        if (!nodes.Contains(destination))
    //        {
    //            nodes.Add(destination);
    //        }



    //        graph.AddEdge(nodeA, nodeB, true, pathData.childPositions.Count);
    //    }

    //    foreach(var node in nodes)
    //    {

    //    Debug.Log(node);
    //    }
    //    //그래프를 출력한다.
    //    graph.DebugPrintLinksPath(graph.nodeList);

    //    return graph;
    //}

    //public void Djikstra(GraphNode<PathData> start,  Graph<PathData> graph)
    //{
    //    int[] dist = new int[graph.NodeCount()];
    //    List<GraphNode<PathData>> found = new List<GraphNode<PathData>>();

    //    //노드리스트
    //    List<GraphNode<PathData>> nodeList = graph.nodeList;

    //    Dictionary<string, int> nodeNumber = new Dictionary<string, int>();
    //    for (int i = 0; i < nodeList.Count; i++)
    //    {
    //        nodeNumber.Add(nodeList[i].Data.departure, i);
    //    }

    //    //dist 초기화 (dist를 무한대로 초기화하고 출발지점과 연결되 있는 노드의 가중치를 업데이트한다.)
    //    for (int i = 0; i < dist.Length; i++)
    //    {
    //        dist[i] = int.MaxValue;
    //    }
    //    for(int i = 0; i<start.Neighbors.Count; i++)
    //    {
    //        int number = -1;
    //        nodeNumber.TryGetValue(start.Neighbors[i].Data.departure ,out number);
    //        dist[number] = start.Weights[i];
    //    }

    //    int min = int.MaxValue;
    //    while (found.Count >= nodeList.Count)
    //    {
    //        //가장 가중치가 낮은 노드를 찾아 min에 넣는다.
    //        GraphNode<PathData> nearestNode = new GraphNode<PathData>();
    //        for (int i = 0; i < dist.Length; i++)
    //        {
    //            if(dist[i] < min)
    //            {
    //                min = dist[i];
    //                nearestNode = nodeList[i];
    //            }

    //        }
    //        //찾았다고 표시한다.
    //        found.Add(nearestNode);
    //        //갱신한다. 
    //        for (int i = 0; i < nearestNode.Neighbors.Count; i++)
    //        {
    //            if (start.Weights[start.Neighbors.IndexOf(nearestNode)] + nearestNode.Weights[i] < start.Weights[start.Neighbors.IndexOf(nearestNode.Neighbors[i])])
    //            {
    //                dist[i] = start.Weights[start.Neighbors.IndexOf(nearestNode)] + nearestNode.Weights[i];
    //            }
    //        }
    //    }

    //    foreach(var d in dist)
    //    {
    //        Debug.Log(start.Data.departure + "로 부터 각 노드까지의 최단거리 :" + d);
    //    }
    //}

    

}
