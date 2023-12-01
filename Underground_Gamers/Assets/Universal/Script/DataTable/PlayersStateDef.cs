using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayersStateDefList.Asset", menuName = "PlayersStateDef/NormalList")]
public class PlayersStateDef : ScriptableObject
{
    public List<AttackDefinitionData> attackDefs = new List<AttackDefinitionData>();
    public List<KitingDefinitionData> kitingDatas = new List<KitingDefinitionData>();
    public List<AttackDefinitionData> skillDatas = new List<AttackDefinitionData>();
}
