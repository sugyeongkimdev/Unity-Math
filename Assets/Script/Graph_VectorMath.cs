using UnityEngine;

public class Graph_VectorMath : Graph
{
    [Range (1, 4)] public int select = 1;

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
        dragBindA.UpdatePos (posA);
        dragBindB.UpdatePos (posB);
    }

    protected override void OnDrawGizmos ()
    {
        base.OnDrawGizmos ();

        if(!dragBindA || !dragBindB)
        {
            return;
        }

        // 드래그 값
        posA = dragBindA.dragPos;
        posB = dragBindB.dragPos;

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
        }

        // 드래그 마지막 값 업데이트
        dragBindA.UpdatePos (posA);
        dragBindB.UpdatePos (posB);
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

    // 04. 벡터 내적 계산(Dot)
    private void DrawDot ()
    {
        // ※ 작성중

        // 내적계산이란 두 벡터간의 각도계산을 뜻함
        // A·B == |A| |B| cosθ
        // A·B = Vector3.Dot(A, B) = (A.x * B.x) + (A.y * B.y) + (A.z * B.z)

        Vector2 boxSize = new Vector2 (2, 2);
        Vector3[] normalArr = GraphHelp.Box (posA, boxSize.x, boxSize.y, rotateA);

        // 상자의 사이드 노말(방향)
        Vector3 dirUp = normalArr[0];
        Vector3 dirDown = normalArr[1];
        Vector3 dirLeft = normalArr[2];
        Vector3 dirRight = normalArr[3];

        // 상자의 사이드 위치
        Vector3 posUp = posA + dirUp * boxSize.y / 2f;
        Vector3 posDown = posA + dirDown * boxSize.y / 2f;
        Vector3 posLeft = posA + dirLeft * boxSize.x / 2f; ;
        Vector3 posRight = posA + dirRight * boxSize.x / 2f; ;

        // 노말 그리기
        GraphHelp.Line (posUp, posUp + dirUp, Color.yellow);
        GraphHelp.Line (posDown, posDown + dirDown, Color.yellow);
        GraphHelp.Line (posLeft, posLeft + dirLeft, Color.yellow);
        GraphHelp.Line (posRight, posRight + dirRight, Color.yellow);
        GraphHelp.Line (posB, posB + (posA - posB).normalized, Color.yellow);
        // 각 노말에서 B까지의 선 그리기
        GraphHelp.Line (posUp, posB, Color.blue);
        GraphHelp.Line (posDown, posB, Color.blue);
        GraphHelp.Line (posLeft, posB, Color.blue);
        GraphHelp.Line (posRight, posB, Color.blue);

        // 호 그리기
        DrawArc (posUp, dirUp);
        DrawArc (posDown, dirDown);
        DrawArc (posLeft, dirLeft);
        DrawArc (posRight, dirRight);
        void DrawArc (Vector3 pos, Vector3 normal)
        {
            float angle = GraphHelp.Angle (pos, posB, normal);
            Color color = angle > 90 ? Color.red : Color.green;
            GraphHelp.SolidArc (pos, posB, normal, color);
        }

        //===//

        var dotUp = Vector3.Dot ((posB - posUp).normalized, dirUp);
        var dotDown = Vector3.Dot ((posB - posDown).normalized, dirDown);
        var dotLeft = Vector3.Dot ((posB - posLeft).normalized, dirLeft);
        var dotRight = Vector3.Dot ((posB - posRight).normalized, dirRight);

        GraphHelp.Label (dotUp, posUp);
        GraphHelp.Label (dotDown, posDown);
        GraphHelp.Label (dotLeft, posLeft);
        GraphHelp.Label (dotRight, posRight);

        // ※ 내적 계산 작성중
        // ※ 작성중
        //var dotUp2 =  ((posB - posUp).x * dirUp.x) + ((posB - posUp).y * dirUp.y) + ((posB - posUp).z * dirUp.z);
        //var dotDown2 = (posUp.x * posB.x) + (posUp.y * posB.y) + (posUp.z * posB.z);
        //var dotLeft2 = (posUp.x * posB.x) + (posUp.y * posB.y) + (posUp.z * posB.z);
        //var dotRight2 = (posUp.x * posB.x) + (posUp.y * posB.y) + (posUp.z * posB.z);


    }


    //==========================================================//

}