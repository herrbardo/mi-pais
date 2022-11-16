using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingCard : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] RectTransform Rect;
    [SerializeField] Canvas ParentCanvas;
    [SerializeField] Vector3 OffsetPosition;

    private void Update()
    {
        MoveToMouse();
    }

    void MoveToMouse()
    {
        Vector2 movePos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(ParentCanvas.transform as RectTransform, Input.mousePosition, ParentCanvas.worldCamera, out movePos);
        Vector3 mousePos = ParentCanvas.transform.TransformPoint(movePos);
        transform.position = mousePos + OffsetPosition;
    }
}
