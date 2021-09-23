using UnityEngine;

// 삼각함수
public class Graph_TrigonometricFunction : Graph
{
    public Vector3 pos = new Vector3 (3, 4, 0);
    public Vector3 windowPos = new Vector3 (6, 2, 0);

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

    [Range (1, 2)] public int select = 1;

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
        dragBindPoint.ResetPos (pos);
        dragBindWindow.ResetPos (windowPos);
    }

    protected override void OnDrawGizmos ()
    {
        base.OnDrawGizmos ();
        // 드래그 업데이트
        dragBindPoint.UpdatePos (out pos);
        dragBindWindow.UpdatePos (out windowPos);

        Calculate ();

        switch(select)
        {
            case 01: DrawTriangle (); break;
            case 02: DrawTimeGraphRect (); break;
        }
    }

    //==========================================================//

    // 계산
    private void Calculate ()
    {
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

        // 계산식
        // h = √2, a = 1, b = 1
        // h = √(1²+1²)

        // sin = 직각일 경우 빗변과 높이의 비율
        // sin(45도) = 0.707... = 1/√2 == 즉 높이는 빗변의 0.707...만큼 작다
        // 빗변(√2) * sin45(0.707...) ≒ 높이(1)

        // cos = 직각일 경우 빗변과 밑변의 비율
        // sin(45도) = 0.707... = 1/√2 == 즉 밑변은 빗변의 0.707...만큼 작다 
        // 빗변(√2) * cos45(0.707...) ≒ 밑변(1)

        // tan = 직각일 경우 밑변과 높이의 비율
        // tan(45도) = 1 = √2/√2 == 즉 밑변과 높이가 같다
        // 밑변(√2) * tan45(1) = 높이(√2)

        //===//

        // sin, cos를 이용한 tan 계산
        // tan == (sin/cos) == (a/h)/(b/h) == (a/b)

        //===//

        // 코시컨트(cosecant/csc), 시컨트(secant/cec), 코탄젠트(cotangent/cot)
        csc = 1f / sin;
        cec = 1f / cos;
        cot = 1f / tan;

        // 또는 역수를 이용한 정의
        // csc == (1/sin) == (h/a)
        // cec == (1/cos) == (h/b)
        // cot == (1/tan) == (b/a)

        //===//

    }

    // 삼각형 그리기
    private void DrawTriangle ()
    {
        GraphHelp.Line (zero, pos, thick: 3);

        Vector3 labelOff = new Vector3 (0.1f, 0.2f, 0);

        GraphHelp.Label ("A", posA + labelOff, Color.magenta);
        GraphHelp.Label ("B", posB + labelOff, Color.magenta);
        GraphHelp.Label ("C", posC + labelOff, Color.magenta);

        var hypotenuse = GraphHelp.Bezier (posA, posB); // 빗변 h
        var opposite = GraphHelp.Bezier (posB, posC);   // 높이 a
        var adjacent = GraphHelp.Bezier (posC, posA);   // 밑변 b

        GraphHelp.Label ($"h({h:0.##})", hypotenuse, size: 12);
        GraphHelp.Label ($"a({a:0.##})", opposite, size: 12);
        GraphHelp.Label ($"b({b:0.##})", adjacent, size: 12);

        GraphHelp.Sub (pos);
        GraphHelp.SolidTriangle (posA, posB, posC);
    }

    // 시간의 흐름 그래프 그리기
    float time;
    private void DrawTimeGraphRect ()
    {
        // ※ 작성중
        time += Time.deltaTime;
        time %= 1f;

        float size = 4f;
        GraphHelp.Box (windowPos, size, size);
        GraphHelp.WireSphere (zero, size:2);

        var circlePos = new Vector3 (Mathf.Sin (time * Mathf.PI * 2) * (size/2f), Mathf.Cos (time * Mathf.PI * 2)* (size / 2f), 0);
        dragBindPoint.ResetPos (circlePos);
        GraphHelp.Sphere (circlePos);


    }

    //==========================================================//


}
