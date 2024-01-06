using TMPro;
using UnityEngine;

public class StoryObj3_2 : MonoBehaviour
{
    [SerializeField]
    private GameObject nextPrefab;
    [SerializeField]
    private ScheduleType needLobbyType;

    private void Update()
    {
        if (ScheduleUIManager.instance.currUIIndex == (int)needLobbyType)
        {
            Instantiate(nextPrefab, transform.parent);
            Destroy(gameObject);
        }
    }
}
