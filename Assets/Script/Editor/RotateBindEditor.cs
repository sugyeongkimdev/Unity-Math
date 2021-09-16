using UnityEditor;
using UnityEngine;

// 드래그 연결
[CustomEditor (typeof (RotateBind))]
public class RotateBindEditor : Editor
{
    public void OnSceneGUI ()
    {
        if(target is RotateBind help)
        {
            Undo.RecordObject (help, nameof (RotateBind));
            help.rotate = Handles.RotationHandle (help.rotate, help.pos);
        }
    }
}
