using TMPro;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager Instance { get; private set; }

    public GameObject portraitPrefab;
    public HitScan pcHitScanPrefab;
    public HitScan npcHitScanPrefab;
    public Projectile projectilePrefab;
    public TextMeshPro textPrefab;

    public ObjectPool<GameObject> gameObjectPool;
    public ObjectPool<HitScan> pcHitScanPool;
    public ObjectPool<HitScan> npcHitScanPool;
    public ObjectPool<Projectile> projectilePool;
    public ObjectPool<TextMeshPro> textPool;

    private void Awake()
    {
        Instance = this;

        IObjectFactory<HitScan> pcHitScanFactory = new HitScanFactory(pcHitScanPrefab);
        pcHitScanPool = new ObjectPool<HitScan>(pcHitScanFactory, 20);

        IObjectFactory<HitScan> npcHitScanFactory = new HitScanFactory(npcHitScanPrefab);
        npcHitScanPool = new ObjectPool<HitScan>(npcHitScanFactory, 20);

        IObjectFactory<Projectile> projectileFactory = new ProjectileFactory(projectilePrefab);
        projectilePool = new ObjectPool<Projectile>(projectileFactory, 20);

        IObjectFactory<TextMeshPro> textFactory = new TextFactory(textPrefab);
        textPool = new ObjectPool<TextMeshPro>(textFactory, 50);
    }

    public void SetPortraitPool(GameObject gameObject)
    {
        portraitPrefab = gameObject;
        IObjectFactory<GameObject> gameObjectFactory = new GameObjectFactory(portraitPrefab);
        gameObjectPool = new ObjectPool<GameObject>(gameObjectFactory, 20);
    }
}
