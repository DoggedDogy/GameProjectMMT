using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] RectTransform[] craftSlots;
    [SerializeField] PlayerController pc;

    public void Craft()
    {
        if (craftSlots[0].childCount == 0) return;
        int blockId = craftSlots[0].GetChild(0).GetComponent<ItemID>().id;
        foreach (RectTransform s in craftSlots)
        {
            if (s.childCount == 0 || blockId != s.GetChild(0).GetComponent<ItemID>().id) return;
        }
        pc.ChangeMats(blockId, -4);
        pc.ChangeBlocks(blockId, 1);
        print("crafted");
        CleaCraft();
    }
    public void CleaCraft()
    {
        foreach(RectTransform cs in craftSlots)
        {
            if (cs.childCount > 0)
            {
                Destroy(cs.GetChild(0));
            }
        }
    }
}
