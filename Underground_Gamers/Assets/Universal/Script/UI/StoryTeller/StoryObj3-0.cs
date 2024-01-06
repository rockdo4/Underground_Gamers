using TMPro;
using UnityEngine;

public class StoryObj3_0 : MonoBehaviour
{
    [SerializeField]
    private GameObject nextPrefab;
    [SerializeField]
    private LobbyType needLobbyType;

    private void Update()
    {
        if (LobbySceneUIManager.instance.currUIIndex == (int)needLobbyType)
        {
            Instantiate(nextPrefab, transform.parent);
            Destroy(gameObject);
        }
    }
}
