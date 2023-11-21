using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using static UnityEditor.Progress;

[CreateAssetMenu(fileName = "PlayerDatabase", menuName = "Custom/PlayerDatabase", order = 1)]
public class PlayerDatabase : ScriptableObject
{
    public List<PlayerInfo> players = new List<PlayerInfo>();

    public void GetPlayerSpriteAsync(int playerCode, System.Action<Sprite> onSpriteLoaded)
    {
        PlayerInfo player = players.Find(i => i.playerCode == playerCode);
        if (player.name != "")
        {
            string spriteAddress = player.playerCode.ToString();
            Addressables.LoadAssetAsync<Sprite>(spriteAddress).Completed += (handle) =>
            {
                onSpriteLoaded?.Invoke(handle.Result);
            };
        }
        else
        {
            onSpriteLoaded?.Invoke(null);
        }
    }
}
