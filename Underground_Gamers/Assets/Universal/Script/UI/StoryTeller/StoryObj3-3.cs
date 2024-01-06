using TMPro;
using UnityEngine;

public class StoryObj3_3 : MonoBehaviour
{
    [SerializeField]
    private GameObject nextPrefab;

    private void Update()
    {
        if (LobbyUIManager.instance.playerList.activeSelf)
        {
            Instantiate(nextPrefab, transform.parent);
            Destroy(gameObject);
        }
    }
}
