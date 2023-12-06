using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TrainToggle : MonoBehaviour
{
    public int id;
    public int cost;
    [SerializeField]
    public TMP_Text statText;
    [SerializeField]
    private TMP_Text PotentialText;
    public void MakeButton()
    {
        StringTable st = DataTableManager.instance.Get<StringTable>(DataType.String);
        TrainingInfo ti = DataTableManager.instance.Get<PlayerTable>(DataType.Player).GetTrainingInfo(id);

        statText.text = ti.type switch
        {
            TrainingType.MoveSpeed => st.Get("move speed"),
            TrainingType.Sight => st.Get("sight"),
            TrainingType.Range => st.Get("range"),
            TrainingType.DetectionRange => st.Get("detection range"),
            TrainingType.Accuracy => st.Get("accuracy"),
            TrainingType.ReactionSpeed => st.Get("reaction speed"),
            TrainingType.AtkRate => st.Get("atk rate"),
            TrainingType.Critical => st.Get("critical"),
            TrainingType.Atk => st.Get("atk"),
            TrainingType.Hp => st.Get("hp"),
            _ => ""
        };
        if (ti.type == TrainingType.AtkRate)
        {
            statText.text += $" + {ti.value.ToString("F1")}";
        }
        else
        {
            statText.text += $" + {ti.value.ToString("F0")}";
        }
        cost = ti.needPotential;
        PotentialText.text = $"{st.Get("potential")} + {ti.needPotential}";
    }
}
