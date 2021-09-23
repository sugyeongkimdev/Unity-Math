using UnityEngine;

public class Graph_VectorMath : Graph
{
    [Range (1, 6)] public int select = 1;

    [Space]
    public Vector3 posA = new Vector3 (3, 4, 0);
    public Vector3 posB = new Vector3 (10, 1, 0);

    [Space]
    public Vector3 rotateA;

    // 드래그 지원
    [Space]
    public DragBind dragBindA;
    public DragBind dragBindB;

    //==========================================================//

    private void Reset ()
    {
        // 드래그 초기화
        foreach(var item in GetComponents<DragBind> ())
        {
            DestroyImmediate (item);
        }
        dragBindA = gameObject.AddComponent<DragBind> ();
        dragBindB = gameObject.AddComponent<DragBind> ();
        dragBindA.ResetPos (posA);
        dragBindB.ResetPos (posB);
    }

    protected override void OnDrawGizmos ()
    {
        base.OnDrawGizmos ();
        // 드래그 업데이트
        dragBindA.UpdatePos (out posA);
        dragBindB.UpdatePos (out posB);

        // 기본 그리기
        GraphHelp.Line (zero, posA, Color.magenta, 3);
        GraphHelp.Line (zero, posB, Color.cyan, 3);
        GraphHelp.Label ("A", posA, Color.magenta);
        GraphHelp.Label ("B", posB, Color.cyan);
        GraphHelp.Sub (posA, Color.magenta);
        GraphHelp.Sub (posB, Color.cyan);

        switch(select)
        {
            case 01: DrawPlus (); break;
            case 02: DrawMinus (); break;
            case 03: DrawNormal (); break;
            case 04: DrawDot (); break;
            case 05: DrawCross (); break;
            case 06: DrawReflection (); break;
        }

    }

    //==========================================================//

    // 01. 벡터 덧셈
    private void DrawPlus ()
    {
        GraphHelp.Line (zero, posA + posB, thick: 3f);
        GraphHelp.Line (posA, posA + posB, Color.cyan, 1f);

        GraphHelp.Label ("A+B", posA + posB);

        GraphHelp.Sub (posA + posB);
    }

    // 02. 벡터 뺼셈
    private void DrawMinus ()
    {
        GraphHelp.Line (zero, posA - posB, thick: 3f);
        GraphHelp.Line (posA, posB, thick: 1f);

        GraphHelp.Label ("A-B", posA - posB);

        GraphHelp.Sub (posA - posB);
    }

    // 03. 벡터 Normal (방향의 길이 1)
    private void DrawNormal ()
    {
        // 백터의 대각선의 길이 (삼각함수)
        // √(가로² + 세로² + 높이²)
        var lengA = Mathf.Sqrt (Mathf.Pow (posA.x, 2) + Mathf.Pow (posA.y, 2) + Mathf.Pow (posA.z, 2));
        var lengB = Mathf.Sqrt (Mathf.Pow (posB.x, 2) + Mathf.Pow (posB.y, 2) + Mathf.Pow (posB.z, 2));

        // 방향 / 길이 = 노말 
        GraphHelp.Label ("A Normal", (posA / lengA), size: 12);
        GraphHelp.Label ("B Normal", (posB / lengB), size: 12);
        GraphHelp.Sub (posA / lengA);
        GraphHelp.Sub (posB / lengB);

        // 삼각형 채우기
        GraphHelp.SolidTriangle (zero, posA, new Vector3 (posA.x, zero.y, posA.z), Color.magenta);
        GraphHelp.SolidTriangle (zero, posB, new Vector3 (posB.x, zero.y, posB.z), Color.cyan);

        // Vector (1, 1)의 경우
        // 대각선의 길이  = √(1² + 1²) = √2
        // 노말값        = Vector (1/√2, 1/√2)

        // 삼각함수로 노말값을 계산해보면 길이 1이 나온다
        // (1/√2)² + (1/√2)² = 1/2 + 1/2 = 1 
    }

    // 04. 벡터 내적 계산(Dot Product)
    private void DrawDot ()
    {
        // 내적계산이란 두 벡터간의 스칼라값(각도) 계산
        // 값이 0이면 두 벡터의 각이 90도, 0보다 크면 90도보다 작고, 0보다 작으면 90도보다 크다
        // A·B == |A||B|cosθ
        // A·B == Vector3.Dot(A, B) == (A.x * B.x) + (A.y * B.y) + (A.z * B.z)

        Vector2 boxSize = new Vector2 (2, 2);
        Vector3[] normalArr = GraphHelp.Box (posA, boxSize.x, boxSize.y, rotateA);

        // 상자의 사이드 위치
        Vector3 posUp = normalArr[0] * boxSize.y / 2f;
        Vector3 posDown = normalArr[1] * boxSize.y / 2f;
        Vector3 posLeft = normalArr[2] * boxSize.x / 2f;
        Vector3 posRight = normalArr[3] * boxSize.x / 2f;
        Vector3 posFront = normalArr[4] * boxSize.x / 2f;
        Vector3 posBack = normalArr[5] * boxSize.x / 2f;

        // 각 노말에서 B까지의 선 그리기
        GraphHelp.Line (posA + posUp, posB, Color.blue);
        GraphHelp.Line (posA + posDown, posB, Color.blue);
        GraphHelp.Line (posA + posLeft, posB, Color.blue);
        GraphHelp.Line (posA + posRight, posB, Color.blue);
        GraphHelp.Line (posA + posFront, posB, Color.blue);
        GraphHelp.Line (posA + posBack, posB, Color.blue);

        // 호 그리기
        DrawArc (posUp, normalArr[0]);
        DrawArc (posDown, normalArr[1]);
        DrawArc (posLeft, normalArr[2]);
        DrawArc (posRight, normalArr[3]);
        DrawArc (posFront, normalArr[4]);
        DrawArc (posBack, normalArr[5]);
        void DrawArc (Vector3 pos, Vector3 normal)
        {
            float angle = GraphHelp.Angle (posA + pos, posB, normal);
            Color color = angle > 90 ? Color.red : Color.green;
            GraphHelp.SolidArc (posA + pos, posB, normal, color);
        }

        //===//

        // 내적 계산식 (A·B)
        var ab = posB - posA;
        var abUp = ab - posUp;
        var abBack = ab - posBack;

        var dotUp = Vector3.Dot (posUp, ab - posUp);
        var dotDown = Vector3.Dot (posDown, ab - posDown);
        var dotLeft = Vector3.Dot (posLeft, ab - posLeft);
        var dotRight = Vector3.Dot (posRight, ab - posRight);
        var dotFront = Vector3.Dot (posFront, ab - posFront);
        var dotBack = Vector3.Dot (posBack, ab - posBack);

        // 위와 같은 내적(dot) 계산식
        var dotUp2 = (posUp.x * abUp.x) + (posUp.y * abUp.y) + (posUp.z * abUp.z);
        var dotBack2 = (posBack.x * abBack.x) + (posBack.y * abBack.y) + (posBack.z * abBack.z);

        // 위 값을 정규화하여 내적계산 (-1 ~ 0 ~ 1)
        var dotUp3 = Vector3.Dot (normalArr[0], abUp.normalized);
        var dotBack3 = Vector3.Dot (normalArr[5], abBack.normalized);

        // 내적 값 표시
        GraphHelp.Label (dotUp, posA + posUp, size: 12);
        GraphHelp.Label (dotDown, posA + posDown, size: 12);
        GraphHelp.Label (dotLeft, posA + posLeft, size: 12);
        GraphHelp.Label (dotRight, posA + posRight, size: 12);
        GraphHelp.Label (dotFront, posA + posFront, size: 12);
        GraphHelp.Label (dotBack, posA + posBack, size: 12);
    }

    // 05. 벡터 외적 계산(Cross Product)
    private void DrawCross ()
    {
        // 외적계산은 두 벡터의 수직을 계산
        // AXB == n|A||B|sinθ (normal * A * B * sinAB)
        // AXB == Vector3.Cross(A, B) == new Vector3 (
        //      A.y * B.z - A.z * B.y,
        //      A.z * B.x - A.x * B.z,
        //      A.x * B.y - A.y * B.x)

        // 벡터 외적 계산
        var posCross1 = Vector3.Cross(posA, posB);
        // 위와 같은 결과의 벡터 외적 계산식
        var posCross2 = new Vector3 (
              posA.y * posB.z - posA.z * posB.y,
              posA.z * posB.x - posA.x * posB.z,
              posA.x * posB.y - posA.y * posB.x);

        GraphHelp.Line (zero, posCross1, thick:3);
    }

    // 06. 벡터 내적을 이용한 반사 계산 (vector direction reflection)
    // http://lab.gamecodi.com/board/zboard.php?id=GAMECODILAB_Lecture_series&no=125
    private void DrawReflection ()
    {
        // reflect = p + 2n ( -p * n )
        // reflect = velocity + 2 normal + Vector3.Dot(-velocity, normal);

        Vector3 vel =  posB - posA;
        Vector3 nor = Vector3.up;
        Vector3 dot2 = 2 * nor * Vector3.Dot (-vel, nor);
        Vector3 reflectionA1 = vel + dot2;

        GraphHelp.Line (posB, posB + dot2);
        GraphHelp.Line (posB + dot2, posB + reflectionA1);

        GraphHelp.Line (posB, posA, Color.magenta);
        GraphHelp.Line (posB, posB + reflectionA1,Color.magenta);
    }


    //==========================================================//

}