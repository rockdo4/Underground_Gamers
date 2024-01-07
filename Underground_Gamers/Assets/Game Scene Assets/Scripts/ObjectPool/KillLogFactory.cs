using UnityEngine;

public class KillLogFactory : IObjectFactory<KillLog>
{
    private KillLog killLogPrefab;
    public KillLogFactory(KillLog killLogPrefab)
    {
        this.killLogPrefab = killLogPrefab;
    }

    public KillLog CreateObject()
    {
        return GameObject.Instantiate(killLogPrefab);
    }
}
