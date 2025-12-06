using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasRenderer))]
public class UILineRenderer : Graphic
{
    public Vector2[] points = new Vector2[2];
    public float lineWidth = 5f;

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear(); 

        if (points.Length < 2) return;

        for (int i = 0; i < points.Length - 1; i++)
        {
            AddLineSegment(vh, points[i], points[i + 1]);
        }
    }

    private void AddLineSegment(VertexHelper vh, Vector2 start, Vector2 end)
    {
        Vector2 dir = (end - start).normalized;
        Vector2 perpendicular = new Vector2(-dir.y, dir.x) * lineWidth / 2;

        UIVertex vertex = UIVertex.simpleVert;
        vertex.color = color;

        int startIndex = vh.currentVertCount;

        vertex.position = start + perpendicular - (Vector2)rectTransform.position;
        vh.AddVert(vertex);

        vertex.position = start - perpendicular - (Vector2)rectTransform.position;
        vh.AddVert(vertex);

        vertex.position = end + perpendicular - (Vector2)rectTransform.position;
        vh.AddVert(vertex);

        vertex.position = end - perpendicular - (Vector2)rectTransform.position;
        vh.AddVert(vertex);

        vh.AddTriangle(startIndex + 0, startIndex + 1, startIndex + 2);
        vh.AddTriangle(startIndex + 2, startIndex + 1, startIndex + 3);
    }

    public void UpdatePoints(Vector2[] newPoints)
    {
        points = newPoints;

        SetVerticesDirty(); 
    }
}