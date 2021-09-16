using UnityEngine;

// 삼각함수
public class Graph_TrigonometricFunction : Graph
{
    public Vector3 pos = new Vector3 (3, 4, 0);
    public Vector3 windowPos = new Vector3 (6.5f, 2.5f, 0);

    private Vector3 posA;
    private Vector3 posB;
    private Vector3 posC;

    private float h; // 빗번
    private float a; // 높이
    private float b; // 밑변

    private float sin; // 사인
    private float cos; // 코사인
    private float tan; // 탄젠트

    private float csc; // 코시컨트(cosecant)
    private float cec; // 시컨트(secant)
    private float cot; // 코탄젠트(cotangent)

    [Range (1, 3)] public int select = 1;

    // 드래그 지원
    [Space]
    public DragBind dragBindPoint;
    public DragBind dragBindWindow;

    //==========================================================//

    private void Reset ()
    {
        // 드래그 초기화
        foreach(var item in GetComponents<DragBind> ())
        {
            DestroyImmediate (item);
        }

        dragBindPoint = gameObject.AddComponent<DragBind> ();
        dragBindWindow = gameObject.AddComponent<DragBind> ();

        dragBindPoint.UpdatePos (pos);
        dragBindWindow.UpdatePos (windowPos);
    }

    protected override void OnDrawGizmos ()
    {
        base.OnDrawGizmos ();

        if(!dragBindPoint)
        {
            return;
        }

        // 드래그 값
        pos = dragBindPoint.dragPos;
        windowPos = dragBindWindow.dragPos;

        // A, B, C 위치 입력
        posA = zero;
        posB = pos;
        posC = new Vector3 (pos.x, zero.y, pos.z);

        // Vector3.Magnitude
        // 각 변의 길이 구하기
        var ab = posB - posA;
        var bc = posC - posB;
        var ac = posC - posA;
        h = Mathf.Sqrt (Mathf.Pow (ab.x, 2) + Mathf.Pow (ab.y, 2) + Mathf.Pow (ab.z, 2));
        a = Mathf.Sqrt (Mathf.Pow (bc.x, 2) + Mathf.Pow (bc.y, 2) + Mathf.Pow (bc.z, 2));
        b = Mathf.Sqrt (Mathf.Pow (ac.x, 2) + Mathf.Pow (ac.y, 2) + Mathf.Pow (ac.z, 2));

        // 사인, 코사인, 탄젠트
        sin = a / h;
        cos = b / h;
        tan = a / b;

        // sin, cos를 이용한 tan 계산
        // tan == (sin/cos) == (a/h)/(b/h) == (a/b)

        // 코시컨트(cosecant/csc), 시컨트(secant/cec), 코탄젠트(cotangent/cot)
        csc = 1f / sin;
        cec = 1f / cos;
        cot = 1f / tan;

        // 또는 역수를 이용한 정의
        // csc == (1/sin) == (h/a)
        // cec == (1/cos) == (h/b)
        // cot == (1/tan) == (b/a)


        DrawTriangle ();
        DrawTimeGraphRect ();

        // 드래그 마지막 값 업데이트
        dragBindPoint.UpdatePos (pos);
        dragBindWindow.UpdatePos (windowPos);
    }

    //==========================================================//

    // 삼각형 그리기
    private void DrawTriangle ()
    {
        GraphHelp.Line (zero, pos, thick: 3);

        Vector3 labelOff = new Vector3 (0.25f, 0.5f, 0);

        GraphHelp.Label ("A", posA + labelOff);
        GraphHelp.Label ("B", posB + labelOff);
        GraphHelp.Label ("C", posC + labelOff);

        var hypotenuse = GraphHelp.Bezier (posA, posB); // 빗변 h
        var opposite = GraphHelp.Bezier (posB, posC);   // 높이 a
        var adjacent = GraphHelp.Bezier (posC, posA);   // 밑변 b

        GraphHelp.Label ($"h({h:0.##})", hypotenuse);
        GraphHelp.Label ($"a({a:0.##})", opposite);
        GraphHelp.Label ($"b({b:0.##})", adjacent);

        GraphHelp.Sub (pos);
        GraphHelp.SolidTriangle (posA, posB, posC);
    }

    // 시간의 흐름 그래프 그리기
    private void DrawTimeGraphRect ()
    {
        // ※ 작성중

        Vector2 size = new Vector2 (3, 3);
        GraphHelp.Box (windowPos, size.x, size.y);
        GraphHelp.Sphere (windowPos + new Vector3 (sin, 0), 0.05f);
    }

    //==========================================================//


}
