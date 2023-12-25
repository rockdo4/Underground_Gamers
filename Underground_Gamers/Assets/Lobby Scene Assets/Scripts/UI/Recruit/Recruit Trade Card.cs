using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecruitTradeCard : MonoBehaviour
{
    public int cost = 100;
    public Image stars;
    public Image image;
    public GameObject cover;
    public TMP_Text costText;

    public GameObject effectRare;
    public GameObject effectUnique;

    public Image typeIcon;
    public TMP_Text name;

    private Button bt;

    public void Awake()
    {
        bt = GetComponent<Button>();
    }
    public void SetCards(int playerID)
    {
        if (bt == null)
        {
            bt = GetComponent<Button>();
        }
        bt.interactable = true;
        cover.SetActive(false);
        PlayerTable pt = DataTableManager.instance.Get<PlayerTable>(DataType.Player);
        StringTable st = DataTableManager.instance.Get<StringTable>(DataType.String);
        var pi = pt.GetPlayerInfo(playerID);
        switch (pi.grade)
        {
            case 3:
                cost = 100;
                effectRare.SetActive(false);
                effectUnique.SetActive(false);
                break;
            case 4:
                cost = 150;
                effectRare.SetActive(true);
                effectUnique.SetActive(false);
                break;
            case 5:
                cost = 200;
                effectRare.SetActive(false);
                effectUnique.SetActive(true);
                break;
            default:
                cost = 300;
                effectRare.SetActive(false);
                effectUnique.SetActive(false);
                break;
        }
        costText.text = cost.ToString();
        image.sprite = pt.GetPlayerSprite(playerID);
        stars.sprite = pi.grade switch
        {
            3 => pt.starsSprites[0],
            4 => pt.starsSprites[1],
            5 => pt.starsSprites[2],
            _ => pt.starsSprites[0],
        };

        typeIcon.sprite = pt.playerTypeSprites[pi.type - 1];
        name.text = st.Get($"playerName{pi.code}");
    }

    public void CloseCards()
    {
        if (bt == null)
        {
            bt = GetComponent<Button>();
        }
        bt.interactable = false;
        cover.SetActive(true);
    }
}
