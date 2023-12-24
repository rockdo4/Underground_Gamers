using TMPro;
using UnityEngine;

public class GiveUpWarning : MonoBehaviour
{
    public GameManager gameManager;

    public TextMeshProUGUI giveUpWarningText;
    public TextMeshProUGUI cancelButtonText;
    public TextMeshProUGUI okButtonText;

    void Start()
    {
        giveUpWarningText.text = gameManager.str.Get("give up warning");
        cancelButtonText.text = gameManager.str.Get("cancel");
        okButtonText.text = gameManager.str.Get("ok");
    }
}
