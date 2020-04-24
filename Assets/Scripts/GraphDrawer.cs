using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public delegate void DrawEdgeLengthDelegate(int length);

public class GraphDrawer : MonoBehaviour
{
    [SerializeField]
    private GameObject verticesTexture;
    [SerializeField]
    private GameObject linePrefab;
    [SerializeField]
    private Text edgeLengthText;//TODO MoveToUIManager

    private List<GameObject> vertexList = new List<GameObject>();
    public void DrawEdgeLength(int length)
    {
        edgeLengthText.text = "Путь равен: " + length;
    }
    private void CreateObjects(int n)
    {
        int R = 20;//TODO autosize
        int a = 0;
        int x = 0;
        int y = 0;

        for (int i = 1; i < n * 2 + 2; i++)
        {
            if (i % 2 == 0)
            {
                Vector2 cur;
                cur.x = (float)(x + R * Math.Cos(a * Math.PI / 180));
                cur.y = (float)(y - R * Math.Sin(a * Math.PI / 180));
                GameObject curVertexObject = Instantiate(verticesTexture, cur, transform.rotation);
                curVertexObject.name = "Vertex " + "(" + cur.x + "; " + cur.y + ")";
                Vertex curVertex = curVertexObject.AddComponent<Vertex>();
                curVertex.color = new Color(
                    UnityEngine.Random.Range(0f, 1f),
                    UnityEngine.Random.Range(0f, 1f),
                    UnityEngine.Random.Range(0f, 1f),
                    1
                );
                curVertexObject.GetComponent<SpriteRenderer>().color = curVertex.color;
                vertexList.Add(curVertexObject);
            }
            a += 180 / n;
        }
    }

    private void CreateEdges(GraphManager graph)
    {
        int[,] array = graph.GetGraph();
        int size = graph.GetSize();

        for (int i = 0; i < size; i++)
        {
            GameObject curVertexObject = vertexList[i];
            Vertex curVertex = curVertexObject.GetComponent<Vertex>();
            curVertex.edges = new List<edge_t>();
            for (int j = 0; j < size; j++)
            {
                if (array[i, j] != graph.inf)
                    curVertex.edges.Add(new edge_t(vertexList[j], array[i, j]));
            }
        }
    }

    private void CreateEdgesLine()
    {
        foreach (GameObject curVertexObject in vertexList)
        {
            Vertex curVertex = curVertexObject.GetComponent<Vertex>();
            curVertex.lines = new List<line_t>();
            foreach (edge_t curEdge in curVertex.edges)
            {
                GameObject curLineObject = Instantiate(linePrefab, new Vector2(0, 0), transform.rotation);
                LineRenderer curLine = curLineObject.GetComponent<LineRenderer>();

                curLine.SetPosition(0, curVertexObject.transform.position);
                curLine.SetPosition(1, curEdge.vertex.transform.position);
                curLineObject.transform.parent = curVertexObject.transform;

                Gradient gradient = new Gradient();
                GradientColorKey[] colorKey;
                GradientAlphaKey[] alphaKey;

                colorKey = new GradientColorKey[2];
                colorKey[0].color = curVertex.color;
                colorKey[0].time = 0.0f;

                colorKey[1].color = curEdge.vertex.GetComponent<Vertex>().color;
                colorKey[1].time = 1.0f;

                alphaKey = new GradientAlphaKey[2];
                alphaKey[0].alpha = 1.0f;
                alphaKey[0].time = 0.0f;
                alphaKey[1].alpha = 1.0f;
                alphaKey[1].time = 1.0f;

                gradient.SetKeys(colorKey, alphaKey);
                curLine.colorGradient = gradient;

                GameObject col = GenerateCollider(curLineObject, curVertexObject.transform.position, curEdge.vertex.transform.position);
                Edge edge = col.AddComponent<Edge>();
                edge.Init(DrawEdgeLength, curEdge.length);

                curVertex.lines.Add(new line_t(curLineObject, col));
            }
        }
    }

    private GameObject GenerateCollider(GameObject line, Vector2 startPos, Vector2 endPos)
    {
        GameObject colliderObject = new GameObject("Collider");
        BoxCollider2D col = colliderObject.AddComponent<BoxCollider2D>();
        col.transform.parent = line.transform;

        float lineLength = Vector2.Distance(startPos, endPos);
        col.size = new Vector2(lineLength, 1f);
        Vector2 midPoint = (startPos + endPos) / 2;
        col.transform.position = midPoint;

        float angle = (Mathf.Abs(startPos.y - endPos.y) / Mathf.Abs(startPos.x - endPos.x));
        if ((startPos.y < endPos.y && startPos.x > endPos.x) || (endPos.y < startPos.y && endPos.x > startPos.x))
        {
            angle *= -1;
        }
        angle = Mathf.Rad2Deg * Mathf.Atan(angle);
        col.transform.Rotate(0, 0, angle);
        return colliderObject;
    }
    public void UpdateGraph(GraphManager graph)
    {
        for (int i = 0; i < vertexList.Count; i++)
        {
            Vertex curVertex = vertexList[i].GetComponent<Vertex>();
            foreach(line_t curLine in curVertex.lines)
            {
                Destroy(curLine.line);
                Destroy(curLine.collider);
            }
            curVertex.lines.Clear();
            curVertex.edges.Clear();
        }
       
        CreateEdges(graph);
        CreateEdgesLine();
    }
    public void Draw(GraphManager graph)
    {
        if (vertexList.Count == 0)
        {
            CreateObjects(graph.GetSize());
            CreateEdges(graph);
            CreateEdgesLine();
        }
        else if (vertexList.Count == graph.GetSize())
            UpdateGraph(graph);
        else
        {
            Clear();
            Draw(graph);
        }
    }
    public void Clear()
    {
        for (int i = 0; i < vertexList.Count; i++)
        {
            Destroy(vertexList[i]);
        }
        vertexList.Clear();
    }
}
