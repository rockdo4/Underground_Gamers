using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerChanger : MonoBehaviour
{
    public GameObject playerButtons;

    public GameObject havePlayerSpace;
    public GameObject havePlayerListSpace;

    public List<UIPlayerSlots> usingSlots;

    public List<Player> haveList;
    public List<Player> usingList;

    [HideInInspector]
    public int currentSlotIndex = 0;
    private LobbyUIManager lobbyUIManager;

    [HideInInspector]
    public List<GameObject> olds = new List<GameObject>();
    [HideInInspector]
    public List<GameObject> oldsPlayerList = new List<GameObject>();
    [SerializeField]
    private TMP_Text playerCounter;

    public static PlayerChanger instance
    {
        get
        {
            if (playerChanger == null)
            {
                playerChanger = FindObjectOfType<PlayerChanger>();
            }
            return playerChanger;
        }
    }

    private static PlayerChanger playerChanger;
    private PlayerTable pt;
    private StringTable st;

    private void Awake()
    {
        lobbyUIManager = GetComponent<LobbyUIManager>();
    }

    private void Start()
    {
        pt = DataTableManager.instance.Get<PlayerTable>(DataType.Player);
        st = DataTableManager.instance.Get<StringTable>(DataType.String);
    }
    public void SlotChecker()
    {
        GamePlayerInfo.instance.LoadPreset();
        usingList = GamePlayerInfo.instance.usingPlayers;
        for (int i = 0; i < 8; i++)
        {
            UIPlayerSlots slot = usingSlots[i];
            if (usingList[i].code < 0)
            {
                slot.FrontPanel.gameObject.SetActive(true);
                slot.image.sprite = null;
            }
            else
            {
                Player player = usingList[i];
                slot.FrontPanel.gameObject.SetActive(false);
                slot.image.sprite = pt.playerSprites[pt.PlayerIndexSearch(player.code)];
                if (!slot.isSpare)
                {
                    slot.typeText.text = st.Get($"type{player.type}");
                    slot.levelText.text = $"Lv.{player.level}";
                    slot.nameText.text = st.Get($"playerName{player.code}");
                    slot.skillNameText.text = st.Get($"skillName{player.gearCode}");
                    slot.skillLevelText.text = $"Lv.{player.gearLevel}";
                    slot.xpGauge.value = player.xp / player.maxXp;
                }
                slot.star.sprite = player.grade switch
                {
                    3 => pt.starsSprites[0],
                    4 => pt.starsSprites[1],
                    5 => pt.starsSprites[2],
                    _ => pt.starsSprites[0],
                };
            }
        }

        if (IsFullSquad())
        {
            LobbySceneUIManager.instance.lobbyTopMenu.gameStartButton.interactable = true;
            LobbySceneUIManager.instance.lobbyTopMenu.officialGameStartButton.interactable = true;
            LobbySceneUIManager.instance.lobbyTopMenu.scrimmageGameStartButton.interactable = true;
        }
        else
        {
            LobbySceneUIManager.instance.lobbyTopMenu.gameStartButton.interactable = false;
            LobbySceneUIManager.instance.lobbyTopMenu.officialGameStartButton.interactable = false;
            LobbySceneUIManager.instance.lobbyTopMenu.scrimmageGameStartButton.interactable = false;
        }
    }

    public void StartChange(int slotIndex)
    {
        currentSlotIndex = slotIndex;
        GamePlayerInfo.instance.SortPlayersWithLevel(true);
        StartChange();
    }

    public void StartChange()
    {
        haveList = GamePlayerInfo.instance.havePlayers;
        usingList = GamePlayerInfo.instance.usingPlayers;

        List<int> dupleBllockCodes = new List<int>();
        foreach (var item in usingList)
        {
            if (item.code != -1)
            {
                dupleBllockCodes.Add(item.code);
            }
        }
        if (dupleBllockCodes.Count > 0)
        {
            dupleBllockCodes = dupleBllockCodes.Distinct().ToList();
            dupleBllockCodes.Remove(usingList[currentSlotIndex].code);
        }
        

        foreach (var old in olds)
        {
            Destroy(old.gameObject);
        }
        olds.Clear();


        foreach (var player in haveList)
        {
            int currIndex = haveList.IndexOf(player);
            var bt = Instantiate(playerButtons, havePlayerSpace.transform);
            var pb = bt.GetComponent<PlayerButtons>();
            pb.SetImage(pt.playerSprites[pt.PlayerIndexSearch(player.code)]);
            pb.GetComponent<Button>().onClick.AddListener(() => ToUse(currIndex));
            pb.GetComponent<Button>().onClick.AddListener(() => lobbyUIManager.ActivePlayerSlotSet(false));
            pb.index = currIndex;
            pb.playerNameCard.text = st.Get($"playerName{player.code}");
            pb.Level.text = $"Lv.{player.level}";
            pb.typeIcon.sprite = Resources.Load<Sprite>(Path.Combine("PlayerType", player.type.ToString()));
            pb.isUsing.gameObject.SetActive(false);
            pb.stars.sprite = player.grade switch
            {
                3 => pt.starsSprites[0],
                4 => pt.starsSprites[1],
                5 => pt.starsSprites[2],
                _ => pt.starsSprites[0],
            };
            if (player.breakthrough <= 0)
            {
                pb.breakImage.gameObject.SetActive(false);
            }
            else
            {
                pb.breakImage.gameObject.SetActive(true);
                pb.breakImage.sprite = player.breakthrough switch
                {
                    1 => pt.berakSprites[0],
                    2 => pt.berakSprites[1],
                    3 => pt.berakSprites[2],
                    _ => pt.berakSprites[0],
                };
            }
            if (dupleBllockCodes.Count > 0 && dupleBllockCodes.Contains(player.code))
            {
                pb.gameObject.SetActive(false);
            }
            olds.Add(bt);
        }

    }

    public void OpenPlayers()
    {
        haveList = GamePlayerInfo.instance.havePlayers;
        usingList = GamePlayerInfo.instance.usingPlayers;

        foreach (var old in oldsPlayerList)
        {
            Destroy(old.gameObject);
        }
        oldsPlayerList.Clear();


        int index = 0;
        foreach (var player in usingList)
        {
            if (player.code != -1)
            {
                int currIndex = index;
                var bt = Instantiate(playerButtons, havePlayerListSpace.transform);
                var pb = bt.GetComponent<PlayerButtons>();
                pb.SetImage(pt.playerSprites[pt.PlayerIndexSearch(player.code)]);
                pb.GetComponent<Button>().onClick.AddListener(() => PlayerListButtonFunc(true, currIndex, true, pb));
                pb.index = index++;
                pb.playerNameCard.text = st.Get($"playerName{player.code}");
                pb.Level.text = $"Lv.{player.level}";
                pb.isUsing.color = UnityEngine.Color.green;
                pb.ID = player.ID;
                pb.typeIcon.sprite = Resources.Load<Sprite>(Path.Combine("PlayerType", player.type.ToString()));
                pb.stars.sprite = player.grade switch
                {
                    3 => pt.starsSprites[0],
                    4 => pt.starsSprites[1],
                    5 => pt.starsSprites[2],
                    _ => pt.starsSprites[0],
                };
                if (player.breakthrough <= 0)
                {
                    pb.breakImage.gameObject.SetActive(false);
                }
                else
                {
                    pb.breakImage.gameObject.SetActive(true);
                    pb.breakImage.sprite = player.breakthrough switch
                    {
                        1 => pt.berakSprites[0],
                        2 => pt.berakSprites[1],
                        3 => pt.berakSprites[2],
                        _ => pt.berakSprites[0],
                    };
                }
                oldsPlayerList.Add(bt);
            }
        }

        index = 0;
        foreach (var player in haveList)
        {
            int currIndex = index;
            var bt = Instantiate(playerButtons, havePlayerListSpace.transform);
            var pb = bt.GetComponent<PlayerButtons>();
            pb.SetImage(pt.playerSprites[pt.PlayerIndexSearch(player.code)]);
            pb.GetComponent<Button>().onClick.AddListener(() => PlayerListButtonFunc(true, currIndex, false, pb));
            pb.index = index++;
            pb.playerNameCard.text = st.Get($"playerName{player.code}");
            pb.Level.text = $"Lv.{player.level}";
            pb.isUsing.color = UnityEngine.Color.red;
            pb.ID = player.ID;
            pb.typeIcon.sprite = Resources.Load<Sprite>(Path.Combine("PlayerType", player.type.ToString()));
            pb.stars.sprite = player.grade switch
            {
                3 => pt.starsSprites[0],
                4 => pt.starsSprites[1],
                5 => pt.starsSprites[2],
                _ => pt.starsSprites[0],
            };
            if (player.breakthrough <= 0)
            {
                pb.breakImage.gameObject.SetActive(false);
            }
            else
            {
                pb.breakImage.gameObject.SetActive(true);
                pb.breakImage.sprite = player.breakthrough switch
                {
                    1 => pt.berakSprites[0],
                    2 => pt.berakSprites[1],
                    3 => pt.berakSprites[2],
                    _ => pt.berakSprites[0],
                };
            }
            oldsPlayerList.Add(bt);
        }
        playerCounter.text = $"{oldsPlayerList.Count}<color=#91AAB9><size=22> / 200</size></color>";
    }

    public void ToUse(int index)
    {
        if (usingList[currentSlotIndex].code >= 0)
        {
            haveList.Add(usingList[currentSlotIndex]);
            GamePlayerInfo.instance.RemoveUsePlayer(currentSlotIndex);
        }
        usingList[currentSlotIndex] = haveList[index];
        haveList.Remove(haveList[index]);
        SlotChecker();
    }

    public void ToHave()
    {
        if (usingList[currentSlotIndex].code >= 0)
        {
            haveList.Add(usingList[currentSlotIndex]);
            GamePlayerInfo.instance.RemoveUsePlayer(currentSlotIndex);
        }
        SlotChecker();
    }

    public bool IsFullSquad()
    {
        usingList = GamePlayerInfo.instance.usingPlayers;
        for (int i = 0; i < 5; i++)
        {
            if (usingList[i].code == -1)
            {
                return false;
            }
        }
        return true;
    }

    //Release
    public void PlayerListButtonFunc(bool on, int index, bool isOnUsingPlayer, PlayerButtons self)
    {
        if (PlayerReleaser.instance.isReleaseMod)
        {
            PlayerReleaser.instance.SelectForReleaseFunc(self);
        }
        else
        {
            lobbyUIManager.ActivePlayerInfo(on, index, isOnUsingPlayer);
        }
    }
}
