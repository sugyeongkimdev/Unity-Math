using UnityEngine;

public class DragBind : MonoBehaviour
{
    // 드래그 위치
    public Vector3 dragPos;

    // 드래그 위치 업데이트
    public void UpdatePos (Vector3 updatePos)
    {
        dragPos = updatePos;
    }
}
