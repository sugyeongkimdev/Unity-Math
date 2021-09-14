using UnityEngine;

public class Graph_VectorMath : Graph
{
    public Vector3 posA = new Vector3 (3, 4, 0);
    public Vector3 posB = new Vector3 (10, 1, 0);

    [Range (1, 3)] public int select = 1;

    //==========================================================//

    protected override void OnDrawGizmos ()
    {
        base.OnDrawGizmos ();

        switch(select)
        {
            case 01: DrawPlus (); break;
            case 02: DrawMinus (); break;
            case 03: DrawNormal (); break;
        }
    }

    //==========================================================//

    // 01. 벡터 덧셈
    private void DrawPlus ()
    {
        Vector3 vecA = zero + posA;
        Vector3 vecB = zero + posB;

        GraphHelp.Line (zero, vecA, Color.magenta);
        GraphHelp.Line (zero, vecB, Color.cyan);
        GraphHelp.Line (zero, vecA + vecB - zero, thick: 3f);
        GraphHelp.Line (vecA, vecA + vecB - zero, Color.cyan, 1f);

        GraphHelp.Sub ("A", vecA, Color.magenta);
        GraphHelp.Sub ("B", vecB, Color.cyan);
        GraphHelp.Sub ("A+B", vecA + vecB - zero, Color.white);
    }

    // 02. 벡터 뺼셈
    private void DrawMinus ()
    {
        Vector3 vecA = zero + posA;
        Vector3 vecB = zero + posB;

        GraphHelp.Line (zero, vecA, Color.magenta);
        GraphHelp.Line (zero, vecB, Color.cyan);
        GraphHelp.Line (zero, vecA - vecB + zero, thick: 3f);
        GraphHelp.Line (vecA, vecB, thick: 1f);

        GraphHelp.Sub ("A", vecA, Color.magenta);
        GraphHelp.Sub ("B", vecB, Color.cyan);
        GraphHelp.Sub ("A-B", vecA - vecB + zero, Color.white);
    }

    // 03. 벡터 Normal (방향의 길이 1)
    private void DrawNormal()
    {
        Vector3 vecA = zero + posA;
        Vector3 vecB = zero + posB;

        GraphHelp.Line (zero, vecA, Color.magenta);
        GraphHelp.Line (zero, vecB, Color.cyan);

        GraphHelp.Sub ("A", vecA, Color.magenta);
        GraphHelp.Sub ("B", vecB, Color.cyan);

        // 백터의 대각선의 길이 (삼각함수)
        // √(가로² + 세로² + 높이²)
        var lengA = Mathf.Sqrt (Mathf.Pow (vecA.x, 2) + Mathf.Pow (vecA.y, 2) + Mathf.Pow (vecA.z, 2));
        var lengB = Mathf.Sqrt (Mathf.Pow (vecB.x, 2) + Mathf.Pow (vecB.y, 2) + Mathf.Pow (vecB.z, 2));
        
        // 방향 / 길이 = 노말 
        GraphHelp.Sub ("A Normal", (vecA / lengA) + zero, Color.white);
        GraphHelp.Sub ("B Normal", (vecB / lengB) + zero, Color.white);

        // 삼각형 채우기
        GraphHelp.SolidTriangle (zero, vecA, new Vector3 (vecA.x, 0, vecA.z), Color.magenta);
        GraphHelp.SolidTriangle (zero, vecB, new Vector3 (vecB.x, 0, vecB.z), Color.cyan);

        // Vector (1, 1)의 경우
        // 대각선의 길이  = √(1² + 1²) = √2
        // 노말값        = Vector (1/√2, 1/√2)

        // 삼각함수로 노말값을 계산해보면 길이 1이 나온다
        // (1/√2)² + (1/√2)² = 1/2 + 1/2 = 1 
    }

    //==========================================================//

}
