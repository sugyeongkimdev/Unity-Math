using UnityEngine;

public class DragBind : MonoBehaviour
{
    // 드래그 위치
    public Vector3 dragPos;

    // 드래그 위치 초기화
    public void ResetPos (Vector3 resetPos)
    {
        dragPos = resetPos;
    }
    // 드래그 위치 업데이트
    public void UpdatePos (out Vector3 updatePos)
    {
        updatePos = dragPos;
        dragPos = updatePos;
    }
}
