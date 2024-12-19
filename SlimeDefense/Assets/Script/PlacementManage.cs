using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementManage : MonoBehaviour
{
    public SlimePlacement prefabPlacement;
    private List<SlimePlacement> slimePlacements = new List<SlimePlacement>();
    void Start()
    {
        CreateSlimePlacement();
    }

    void CreateSlimePlacement()
    {
        if (prefabPlacement == null)
        {
            return;
        }

        int count = 75;
        GameObject placement;
        int x, z = 0;
        for (int i = 0; i < count; i++)
        {
            placement = Instantiate<GameObject>(prefabPlacement.gameObject, this.transform);
            int xSum = i / 5;
            int zSum = i % 5;
            x = xSum * 2;
            z = zSum * (-2);

            placement.transform.localPosition = new Vector3(x, placement.transform.localPosition.y, z);
            placement.SetActive(true);
        }
    }
}
