using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GraphDrawer drawer;
    [SerializeField]
    private GraphManager graph;
    [SerializeField]
    private Text numberOfNodesText;
    [SerializeField]
    private Slider randomFactorSlider;
    [SerializeField]
    private GameObject errorPanel;
    [SerializeField]
    private Text errorText;

    public void ApplyFloydWarshell()
    {
        if (graph.GetSize() != 0)
        {
            graph.FloydWarshell();
            drawer.Draw(graph);
        }
        else
        {
            DrawError("Граф не найден!");
        }
    }
    public void FillRandom()
    {
        if (graph.GetSize() != 0)
        {
            graph.FillRandom(1, 100, (int)randomFactorSlider.value);
            drawer.Draw(graph);
        }
        else
        {
            DrawError("Граф не найден!");
        }
    }
    public void CreateGraph()
    {
        int size;
        if(int.TryParse(numberOfNodesText.text, out size))
        {
            if ((size >= 2) && (size <= 20))
            {
                graph.InitGraph(size);
                drawer.Draw(graph);
            }
            else
            {
                DrawError("Колличество узлов может быть от 2 до 20!");
            }
        }
        else
        {
            DrawError("Введите число!");
        }
    }
    public void ErrorOKButton()
    {
        errorPanel.SetActive(false);
    }
    public void DrawError(string errText)
    {
        errorPanel.SetActive(true);
        errorText.text = errText;
    }
}
