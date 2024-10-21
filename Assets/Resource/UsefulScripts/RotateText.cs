using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class RotateText : UIBehaviour, IMeshModifier
{
    public new void OnValidate()
    {
        base.OnValidate();

        var graphics = base.GetComponent<Graphic>();
        if (graphics != null)
        {
            graphics.SetVerticesDirty();
        }
    }

    public void ModifyMesh(Mesh mesh) { }
    public void ModifyMesh(VertexHelper verts)
    {
        if (!this.IsActive())
        {
            return;
        }

        List<UIVertex> vertexList = new List<UIVertex>();
        verts.GetUIVertexStream(vertexList);

        ModifyVertices(vertexList);

        verts.Clear();
        verts.AddUIVertexTriangleStream(vertexList);
    }

    void ModifyVertices(List<UIVertex> vertexList)
    {
        // memo: １テキスト６頂点
        for (int i = 0, vertexListCount = vertexList.Count; i < vertexListCount; i += 6)
        {
            var center = Vector2.Lerp(vertexList[i].position, vertexList[i + 3].position, 0.5f);
            for (int r = 0; r < 6; r++)
            {
                var element = vertexList[i + r];
                var pos = element.position - (Vector3)center;
                var newPos = new Vector2(
                    pos.x * Mathf.Cos(90 * Mathf.Deg2Rad) - pos.y * Mathf.Sin(90 * Mathf.Deg2Rad),
                    pos.x * Mathf.Sin(90 * Mathf.Deg2Rad) + pos.y * Mathf.Cos(90 * Mathf.Deg2Rad)
                );

                element.position = (Vector3)(newPos + center);
                vertexList[i + r] = element;
            }
        }
    }
}