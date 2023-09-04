using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropHandler : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        DragHandler item = eventData.pointerDrag.GetComponent<DragHandler>();
        if (item && transform.childCount == 0)
        {
            item.startParent = transform;
        }
    }
}
