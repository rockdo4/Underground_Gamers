
using UnityEngine;

public class GameObjectFactory : IObjectFactory<GameObject>
{
    private GameObject gameObjectPrefab;
    public GameObjectFactory(GameObject gameObject)
    {
        this.gameObjectPrefab = gameObject;
    }

    public GameObject CreateObject()
    {
        return GameObject.Instantiate(gameObjectPrefab);
    }
}
