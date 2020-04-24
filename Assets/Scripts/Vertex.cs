using System.Collections.Generic;
using UnityEngine;

public struct edge_t
{
    public edge_t(GameObject vertex, int length)
    {
        this.vertex = vertex;
        this.length = length;
    }
    public GameObject vertex;
    public int length;
}
public struct line_t
{
    public line_t(GameObject line, GameObject collider)
    {
        this.line = line;
        this.collider = collider;
    }
    public GameObject line;
    public GameObject collider;
}
public class Vertex : MonoBehaviour
{
    public List<line_t> lines;
    public List<edge_t> edges;
    public Color color;
}
