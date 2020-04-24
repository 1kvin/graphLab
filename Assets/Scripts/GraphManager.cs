using System;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class GraphManager : MonoBehaviour
{
    private int[,] array;
    private int size = 0; // 2-20
    public readonly int inf = -1;


    public void InitGraph(Text UIText)
    {
        int size = int.Parse(UIText.text);
        array = new int[size, size];
        this.size = size;
    }

    public void InitGraph(int size)
    {
        array = new int[size, size];
        this.size = size;
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                array[i, j] = inf;
            }
        }
    }
    public int GetSize()
    {
        return size;
    }
    public int[,] GetGraph()
    {
        return array;
    }
    public void PrintArrayToConsole()
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                sb.Append(array[i, j]);
                sb.Append(" ");
            }
            sb.Append("\n");
        }
        Debug.Log(sb.ToString());
    }
    public void FillRandom(int min = 1, int max = 100, int infFactor = 2)
    {
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                if (i == j)
                {
                    array[i, j] = inf;
                    continue;
                }

                if (UnityEngine.Random.Range(0, infFactor) == 1)
                    array[i, j] = UnityEngine.Random.Range(min, max);
                else
                    array[i, j] = inf;
            }
        }
    }
    public void FloydWarshell()
    {
        for (int k = 0; k < size; k++)
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (i != j && array[i, k] != inf && array[k, j] != inf)
                    {
                        if (array[i, j] == inf)
                            array[i, j] = array[i, k] + array[k, j];
                        else
                            array[i, j] = Math.Min(array[i, j], array[i, k] + array[k, j]);
                    }
                }
            }
        }
    }
}
