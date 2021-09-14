using UnityEditor;
using UnityEngine;

public static class GraphHelp
{
    public static Vector3 zero;

    // 그래프 그리기
    public static void DrawGraph ()
    {
        // 방향
        Vector3 xDir = Vector3.right * 100f;
        Vector3 yDir = Vector3.up * 100f;
        Vector3 zDir = Vector3.forward * 100f;
        // 기준
        Line (zero - xDir, zero + xDir, Color.red);
        Line (zero - yDir, zero + yDir, Color.green);
        Line (zero - zDir, zero + zDir, Color.blue);
        // 중심점
        //Gizmos.color = Color.yellow;
        //Gizmos.DrawWireSphere (zero, 0.1f);
    }

    //==========================================================//

    // 벡터 그리기
    public static void Line (Vector3 startPos, Vector3 endPos, Color? color = null, float thick = 5f)
    {
        Color col = color.HasValue ? color.Value : Color.white;
        Handles.DrawBezier (startPos, endPos, startPos, endPos, col, null, thick);
    }

    // 라벨 및 보조선 그리기
    private static GUIStyle labelStyle = new GUIStyle ();
    public static void Sub (string content, Vector3 drawPos, Color color)
    {
        Vector3 xPos = new Vector3 (drawPos.x, zero.y, zero.z);
        Vector3 yPos = new Vector3 (zero.x, drawPos.y, zero.z);
        Vector3 zPos = new Vector3 (zero.x, zero.y, drawPos.z);

        //Color color = Color.white;
        labelStyle.fontSize = 24;
        labelStyle.normal.textColor = color;

        Handles.Label (drawPos, $"{content}", labelStyle);

        Draw (drawPos.x, zero.x, xPos);
        Draw (drawPos.y, zero.y, yPos);
        Draw (drawPos.z, zero.z, zPos);
        void Draw (float posVal, float zeroVal, Vector3 labelPos)
        {
            // zero면 무시
            if(posVal != zeroVal)
            {
                // 라벨 그리기
                Handles.Label (labelPos, $"{posVal:0.##}", labelStyle);
                Line (labelPos, drawPos, color, 1.5f);
            }
        }
    }

    // 삼각형 채워서 그리기
    public static void SolidTriangle (Vector3 pos1, Vector3 pos2, Vector3 pos3, Color color)
    {
        if(pos1 == pos2 || pos2 == pos3 || pos1 == pos3)
        {
            return;
        }
        float frame = 0.01f;
        float thick = 10f;
        color.a = 0.15f;
        for(float i = 0f; i <= 1f; i += frame)
        {
            Vector3 sPos = Vector3.Lerp (pos1, pos2, i);
            Vector3 ePos = Vector3.Lerp (pos1, pos3, i);
            Line (sPos, ePos, color, thick);
        }
        //Line (pos1, pos2, color, thick);
        //Line (pos2, pos3, color, thick);
        Line (pos3, pos1, color, thick);
    }
}
