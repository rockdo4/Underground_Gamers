using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PopupRepresentPlayers : MonoBehaviour
{
    [SerializeField]
    private Image portrait;
    [SerializeField]
    private TMP_Text charName;
    [SerializeField]
    private GameObject togglePrefab;
    [SerializeField]
    private ToggleGroup toggleGroup;
    [SerializeField]
    private Transform togglePos;
    public Sprite nullSprite;

    private List<GameObject> toggles = new List<GameObject>();
    private int currCode = -1;

    PlayerTable pt;

    private bool isInit = false;
    private void OnEnable()
    {
        MakeButtons();
    }

    private void MakeButtons()
    {
        if (!isInit) 
        {
            pt = DataTableManager.instance.Get<PlayerTable>(DataType.Player);
            isInit = true;
        }
        foreach (var toggle in toggles)
        {
            Destroy(toggle);
        }
        toggles.Clear();

        var players = DataTableManager.instance.Get<PlayerTable>(DataType.Player).playerDatabase;
        List<Player> playerList = new List<Player>();
        playerList.AddRange(GamePlayerInfo.instance.GetUsingPlayers());
        playerList.AddRange(GamePlayerInfo.instance.havePlayers);
        List<int> distinctCodeList = playerList
  .OrderByDescending(item => item.level)
  .ThenByDescending(item => item.breakthrough)
  .Select(item => item.code)
  .Distinct()
  .ToList();

        portrait.sprite = nullSprite;
        charName.text = "";
        currCode = GamePlayerInfo.instance.representativePlayer;

        foreach (var playerCode in distinctCodeList)
        {
            var go = Instantiate(togglePrefab, togglePos);
            var tog = go.GetComponent<Toggle>();
            tog.group = toggleGroup;
            if (currCode == playerCode) 
            {
                portrait.sprite = pt.GetPlayerSprite(playerCode);
                charName.text = pt.GetPlayerInfo(playerCode).name;
                tog.isOn = true;
            }
            else 
            {
                tog.isOn = false;
            }
            int code = playerCode;
            tog.onValueChanged.AddListener(value => SetPlayer(value, code));
            tog.targetGraphic.GetComponent<Image>().sprite = pt.GetPlayerSprite(code);
            toggles.Add(go);
        }
    }

    private void SetPlayer(bool on,int code)
    {
        if (!isInit)
        {
            pt = DataTableManager.instance.Get<PlayerTable>(DataType.Player);
            isInit = true;
        }
        if (on) 
        {
            portrait.sprite = pt.GetPlayerSprite(code);
            charName.text = pt.GetPlayerInfo(code).name;
            currCode = code;
        }
        else if (toggleGroup.ActiveToggles().ToArray().Length <= 0)
        {
            portrait.sprite = nullSprite;
            charName.text = "";
            currCode = -1;
        }
    }

    public void EndSelect()
    {
        GamePlayerInfo.instance.representativePlayer = currCode;
    }

}
