using UnityEngine;

public class RotateBind : MonoBehaviour
{
    // 드래그 위치
    public Vector3 pos;
    public Quaternion rotate;

    // 드래그 위치 업데이트
    public void UpdateRotate (Vector3 updatePos, Quaternion updateRotate)
    {
        pos = updatePos;
        rotate = updateRotate;
    }

}
