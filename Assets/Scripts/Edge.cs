using UnityEngine;

public class Edge : MonoBehaviour
{
    private DrawEdgeLengthDelegate draw;
    private int length;

    public void Init(DrawEdgeLengthDelegate drawDelegate, int length)
    {
        draw = drawDelegate;
        this.length = length;
    }

    private void OnMouseDown()
    {
        draw?.Invoke(length);
    }
}
