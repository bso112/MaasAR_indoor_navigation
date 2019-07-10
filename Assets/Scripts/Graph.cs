using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
/// <typeparam name="T"> 노드의 데이터타입</typeparam>
public class Graph<T> : MonoBehaviour
{
    public List<GraphNode<T>> nodeList;
    

    public GraphNode<T> SearchNode(T data)
    {
        foreach(var node in nodeList)
        {
            if(node.Data.Equals(data))
            {
                return node;
            }
        }
        return null;
    }


    public Graph()
    {
        nodeList = new List<GraphNode<T>>();
    }
    
    public int NodeCount()
    {
        return nodeList.Count;
    }
    public GraphNode<T> AddNode(T data)
    {
        GraphNode<T> n = new GraphNode<T>(data);
        nodeList.Add(n);
        return n;
    }

    public GraphNode<T> AddNode(GraphNode<T> node)
    {
        nodeList.Add(node);
        return node;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <param name="oneway"> 편도인가?</param>
    /// <param name="weight"></param>
    public void AddEdge(GraphNode<T> from, GraphNode<T> to, bool oneway = true, int weight = 0)
    {
        from.Neighbors.Add(to);
        from.Weights.Add(weight);

        if (!oneway)
        {
            to.Neighbors.Add(from);
            to.Weights.Add(weight);
        }
    }

    internal void DebugPrintLinks()
    {
        foreach (GraphNode<T> graphNode in nodeList)
        {
            foreach (var n in graphNode.Neighbors)
            {
                string s = graphNode.Data + " - " + n.Data;
                Debug.Log(s);
            }
        }
    }

    internal void DebugPrintLinksPath(List<GraphNode<PathData>> nodeList)
    {
        foreach (GraphNode<PathData> graphNode in nodeList)
        {
            Debug.Log(graphNode.Data.departure);
            foreach (var n in graphNode.Neighbors)
            {
            }
        }
    }



}
// GraphNode 클래스
public class GraphNode<T>
{
    private List<GraphNode<T>> _neighbors;
    private List<int> _weights;

    public T Data { get; set; }

    public GraphNode()
    {
    }

    public GraphNode(T value)
    {
        this.Data = value;
    }

    public List<GraphNode<T>> Neighbors
    {
        get
        {
            _neighbors = _neighbors ?? new List<GraphNode<T>>();
            return _neighbors;
        }
    }

    public List<int> Weights
    {
        get
        {
            _weights = _weights ?? new List<int>();
            return _weights;
        }
    }
}

public class MatrixGraph
{

    int MAX_VTXS = 256;
    PathData[] vdata = new PathData[256];
    int[,] adj = new int[256,256];
    int vsize;

    public bool is_empty_graph() { return vsize == 0; }
    public bool is_full_graph() { return vsize >= MAX_VTXS; }
    public int GetGraphSize() { return vsize; }
    public void init_graph()
    {
        int i, j;
        vsize = 0;
        for(i=0; i<MAX_VTXS; i++)
        {
            for (j = 0; j < MAX_VTXS; j++)
                adj[i,j] = 0;
        }
    }

    public void insert_vertex(PathData name)
    {
        if (is_full_graph())
            Debug.Log("꽉찬 그래프입니다.");
        else
            vdata[vsize++] = name;
    }

    public void insert_edge(int u, int v, int val)
    {
        adj[u, v] = val;
    }

    public void insert_edge2(int u, int v, int val)
    {
        adj[u, v] = adj[v, u] = val;
    }

    public void print_graph(string msg)
    {
        int i, j;
        Debug.Log(msg);
        Debug.Log( vsize+"\n");
        for (i = 0; i < vsize; i++)
        {
            Debug.Log(vdata[i] + " ");
            for (j = 0; j < vsize; j++)
            {
                Debug.Log(adj[i, j]);
                Debug.Log("\n");
            }
        }

    }


}

// Graph 테스트
internal class Program
{
    private static void Main(string[] args)
    {
        Graph<int> g = new Graph<int>();
        var n1 = g.AddNode(10);
        var n2 = g.AddNode(20);
        var n3 = g.AddNode(30);
        var n4 = g.AddNode(40);
        var n5 = g.AddNode(50);

        g.AddEdge(n1, n3);
        g.AddEdge(n2, n4);
        g.AddEdge(n3, n4);
        g.AddEdge(n3, n5);
        g.DebugPrintLinks();
    }
}
