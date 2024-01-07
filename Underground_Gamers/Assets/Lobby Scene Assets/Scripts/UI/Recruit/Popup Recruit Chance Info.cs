using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class PopupRecruitChanceInfo : MonoBehaviour
{
    [SerializeField]
    private GameObject[] charImages = new GameObject[21];
    [SerializeField]
    private TMP_Text[] chanceText = new TMP_Text[3];

    [SerializeField]
    private int[] recruit1;
    [SerializeField]
    private int[] recruit2;
    [SerializeField]
    private int[] recruit3;
    [SerializeField]
    private int[] tryout;

    public void OpenChanceInfo(int code)
    {
        int[] currentCode;
        switch (code)
        {
            case 0:
                currentCode = recruit1;
                chanceText[0].text = "2.8 %";
                chanceText[1].text = "13.9 %";
                chanceText[2].text = "83.3 %";
                break;
            case 1:
                currentCode = recruit2;
                chanceText[0].text = "4.8 %";
                chanceText[1].text = "24.0 %";
                chanceText[2].text = "72.0 %";
                break;
            case 2:
                currentCode = recruit3;
                chanceText[0].text = "8.2 %";
                chanceText[1].text = "30.6 %";
                chanceText[2].text = "61.2 %";
                break;
            default:
                currentCode = tryout;
                chanceText[0].text = "2.0 %";
                chanceText[1].text = "19.6 %";
                chanceText[2].text = "78.4 %";
                break;
        }

        for (int i = 0; i < charImages.Length; i++)
        {
            charImages[i].SetActive(currentCode.Contains(i));
        }

        gameObject.SetActive(true);
    }

}
