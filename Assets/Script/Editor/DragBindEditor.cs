using UnityEditor;
using UnityEngine;

// 드래그 연결
[CustomEditor (typeof (DragBind))]
public class DragBindEditor : Editor
{
    public void OnSceneGUI ()
    {
        if(target is DragBind help)
        {
            Undo.RecordObject (help, nameof (DragBind));
            help.ResetPos (Handles.PositionHandle (help.dragPos, Quaternion.identity));
        }
    }
}
