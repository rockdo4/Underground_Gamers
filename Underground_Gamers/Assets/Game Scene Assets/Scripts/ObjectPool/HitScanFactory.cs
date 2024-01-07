using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitScanFactory : IObjectFactory<HitScan>
{
    private HitScan hitScanPrefab;
    public HitScanFactory(HitScan hitScanPrefab)
    {
        this.hitScanPrefab = hitScanPrefab;
    }

    public HitScan CreateObject()
    {
        return GameObject.Instantiate(hitScanPrefab);
    }
}
