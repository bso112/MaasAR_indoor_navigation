using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
/// <typeparam name="T"> 노드의 데이터타입</typeparam>
public class Graph<T> : MonoBehaviour
{
    private List<GraphNode<T>> _nodeList;
    

    public GraphNode<T> SearchNode(T data)
    {
        foreach(var node in _nodeList)
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
        _nodeList = new List<GraphNode<T>>();
    }
    
    public int NodeCount()
    {
        return _nodeList.Count;
    }
    public GraphNode<T> AddNode(T data)
    {
        GraphNode<T> n = new GraphNode<T>(data);
        _nodeList.Add(n);
        return n;
    }

    public GraphNode<T> AddNode(GraphNode<T> node)
    {
        _nodeList.Add(node);
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
        foreach (GraphNode<T> graphNode in _nodeList)
        {
            foreach (var n in graphNode.Neighbors)
            {
                string s = graphNode.Data + " - " + n.Data;
                Debug.Log(s);
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
