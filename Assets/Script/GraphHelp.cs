using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

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
        Line (zero - xDir, zero + xDir, Color.red, 3);
        Line (zero - yDir, zero + yDir, Color.green, 3);
        Line (zero - zDir, zero + zDir, Color.blue, 3);
        // 중심점
        //Gizmos.color = Color.yellow;
        //Gizmos.DrawWireSphere (zero, 0.1f);
    }

    //==========================================================//

    private static GUIStyle labelStyle = new GUIStyle ();
    public static void Label (object label, Vector3 labelPos, Color? color = null, int size = 24)
    {
        string str = "";
        switch(label)
        {
            case int i: str = i.ToString (); break;
            case float f: str = $"{f:0.##}"; break;
            case string s: str = s; break;
            case Vector2 vec2: str = vec2.ToString (); break;
            case Vector3 vec3: str = vec3.ToString (); break;
        }

        color = color.HasValue ? color.Value : Color.white;
        labelStyle.fontSize = size;
        labelStyle.normal.textColor = color.Value;
        //labelStyle.alignment = TextAnchor.MiddleCenter;
        Handles.Label (labelPos, str, labelStyle);
    }

    //==========================================================//

    // 벡터 그리기
    public static void Line (Vector3 startPos, Vector3 endPos, Color? color = null, float thick = 1f)
    {
        Color col = color.HasValue ? color.Value : Color.white;

        var tex = new Texture2D (1, 1);
        tex.SetPixels (new[] { col });
        tex.Apply ();

        Handles.DrawAAPolyLine (tex, thick, startPos, endPos);
    }

    // 베지어 커브 그리기 (Bezier)
    // 덤으로 커브 중간 위치를 반환함
    public static Vector3 Bezier (Vector3 startPos, Vector3 endPos, Color? color = null, float strength = 1f)
    {
        // 서로 위치가 같으면 커브를 그릴 수 없음
        if(startPos == endPos)
        {
            return zero;
        }
        color = color.HasValue ? color.Value : Color.white;

        // 거리에 따른 일정한 곡선 위치 만들기
        float dist = Vector3.Distance (startPos, endPos) / 5f;
        Vector3 dPos = endPos - startPos;
        Vector3 offPos = new Vector3 (-dPos.y, dPos.x, -dPos.z) / Mathf.Sqrt (Mathf.Pow (dPos.x, 2) + Mathf.Pow (dPos.y, 2) + Mathf.Pow (dPos.z, 2)) * strength * dist;

        // 곡선이 삼각형 안쪽에 있을 경우 곡선을 뒤집음
        // 필요없으면 주석처리
        if(dPos.x <= zero.x)
        {
            offPos *= -1;
        }
        if(dPos.y <= zero.y)
        {
            offPos *= -1;
        }

        // 베지어 커브 곡선 보간 위치 만들기
        Vector3 startTarget = Vector3.Lerp (startPos, endPos, 0.25f) + offPos;
        Vector3 centerTarget = Vector3.Lerp (startPos, endPos, 0.5f) + offPos;
        Vector3 endTarget = Vector3.Lerp (startPos, endPos, 0.75f) + offPos;

        // 테스트 용도
        //Gizmos.DrawWireSphere (startTarget, 0.1f);
        //Gizmos.DrawWireSphere (endTarget, 0.1f);

        // 베지어 커브 그리기
        Handles.DrawBezier (startPos, endPos, startTarget, endTarget, color.Value, null, 1.5f);
        return centerTarget;
    }

    // 박스 그리기
    // 각 면의 노말값 반환
    public static Vector3[] Box (Vector3 pos, float width, float height, Vector2 rotate = new Vector2(), Color? color = null, float thick =1f)
    {
        float x = width / 2f;
        float y = height / 2f;
        float z = height / 2f;

        // 각 포지션, u - up, d - down, l - left, r - right, f - front, b - back
        Vector3 ful = RotateVector (new Vector3 (-x, y, -z));
        Vector3 fur = RotateVector (new Vector3 (x, y, -z));
        Vector3 bul = RotateVector (new Vector3 (-x, y, z));
        Vector3 bur = RotateVector (new Vector3 (x, y, z));
        Vector3 fdl = RotateVector (new Vector3 (-x, -y, -z));
        Vector3 fdr = RotateVector (new Vector3 (x, -y, -z));
        Vector3 bdl = RotateVector (new Vector3 (-x, -y, z));
        Vector3 bdr = RotateVector (new Vector3 (x, -y, z));

        // 각 상자면의 노말값
        Vector3 downNormal = Vector3.Cross (ful - bur, fur - bul).normalized;
        Vector3 upNormal = downNormal * -1;
        Vector3 leftNormal = Vector3.Cross (ful - bdl, fdl - bdl).normalized;
        Vector3 rightNormal = leftNormal * -1;
        Vector3 frontNormal = Vector3.Cross (ful - fdr, fur - fdl).normalized;
        Vector3 backNormal = frontNormal * -1;

        // 윗면
        Line (ful, fur, color, thick);
        Line (fur, bur, color, thick);
        Line (bur, bul, color, thick);
        Line (bul, ful, color, thick);

        // 아랫면
        Line (fdl, fdr, color, thick);
        Line (fdr, bdr, color, thick);
        Line (bdr, bdl, color, thick);
        Line (bdl, fdl, color, thick);

        // 기둥
        Line (ful, fdl, color, thick);
        Line (fur, fdr, color, thick);
        Line (bur, bdr, color, thick);
        Line (bul, bdl, color, thick);

        // 테스트 용도
        //Label ("ful", ful);
        //Label ("fur", fur);
        //Label ("bul", bul);
        //Label ("bur", bur);
        //Label ("fdl", fdl);
        //Label ("fdr", fdr);
        //Label ("bdl", bdl);
        //Label ("bdr", bdr);
        //Sphere (upNormal + pos);
        //Sphere (downNormal + pos);
        //Sphere (leftNormal + pos);
        //Sphere (rightNormal + pos);
        //Sphere (frontNormal + pos);
        //Sphere (backNormal + pos);

        // 각 사이드의 노말 반환
        return new Vector3[] { upNormal, downNormal, leftNormal, rightNormal, frontNormal, backNormal };

        // 회전
        Vector3 RotateVector (Vector3 targetPos)
        {
            var xRotate = Quaternion.AngleAxis (rotate.x, Vector3.back) * targetPos;
            var yRotate = Quaternion.AngleAxis (rotate.y, Vector3.right) * xRotate;
            return yRotate + pos;
        }
    }

    // 보조선 그리기
    public static void Sub (Vector3 drawPos, Color? color = null)
    {
        color = color.HasValue ? color.Value : Color.white;

        Vector3 xPos = new Vector3 (drawPos.x, zero.y, zero.z);
        Vector3 yPos = new Vector3 (zero.x, drawPos.y, zero.z);
        Vector3 zPos = new Vector3 (zero.x, zero.y, drawPos.z);

        Draw (drawPos.x, zero.x, xPos);
        Draw (drawPos.y, zero.y, yPos);
        Draw (drawPos.z, zero.z, zPos);
        void Draw (float posVal, float zeroVal, Vector3 labelPos)
        {
            // zero면 무시, 아니면 보조선 그리기
            if(posVal != zeroVal)
            {
                Line (labelPos, drawPos, color, 1f);
                Label (posVal, labelPos, null, 12);
            }
        }
    }

    //==========================================================//

    // 삼각형 채워서 그리기
    public static void SolidTriangle (Vector3 pos1, Vector3 pos2, Vector3 pos3, Color? color = null)
    {
        if(pos1 == pos2 || pos2 == pos3 || pos1 == pos3)
        {
            return;
        }
        Color saveColor = Handles.color;
        Color polygonColor = color.HasValue ? color.Value : Color.white;
        polygonColor.a = 0.15f;

        Handles.color = polygonColor;
        Handles.DrawAAConvexPolygon (pos1, pos2, pos3);
        Handles.color = saveColor;
    }

    // 호 채워서 그리기
    public static void SolidArc (Vector3 from, Vector3 to, Vector3 normal, Color? color = null, float radius = 1f)
    {
        // 색상
        Color saveColor = Handles.color;
        Color arcColor = color.HasValue ? color.Value : Color.white;
        arcColor.a = 0.15f;
        Handles.color = arcColor;

        // 호 그리기
        // https://answers.unity.com/questions/1290946/how-to-position-handlesdrawsolidarc.html
        Vector3 subPos = from + normal * 2f;
        float angle = Angle (from, to, normal);
        Vector3 cross = Vector3.Cross (subPos - from, to - from);
        Handles.DrawSolidArc (from, cross, subPos - from, angle, radius);

        Handles.color = saveColor;
    }

    // 각도 구하기
    public static float Angle (Vector3 from, Vector3 to, Vector3 normal)
    {
        Vector3 subPos = from + normal * 2f;
        return Vector3.Angle (subPos - from, to - from);
    }

    //==========================================================//

    // 구체 그리기
    public static void Sphere (Vector3 pos, float size = 0.1f)
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere (pos, size);
    }

}