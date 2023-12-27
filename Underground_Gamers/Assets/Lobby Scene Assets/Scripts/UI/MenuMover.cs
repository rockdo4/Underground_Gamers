using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuMover : MonoBehaviour
{
    [SerializeField]
    private List<Button> func1_story = new List<Button>();
    [SerializeField]
    private List<Button> func2_official = new List<Button>();
    [SerializeField]
    private List<Button> func3_scrimmage = new List<Button>();
    public void StartMenuMove()
    {
        switch(GamePlayerInfo.instance.willOpenMenu)
        {
            case 1:
                foreach (var item in func1_story)
                {
                    item.onClick.Invoke();
                }
                break;
            case 2:
                foreach (var item in func2_official)
                {
                    item.onClick.Invoke();
                }
                break;
            case 3:
                foreach (var item in func3_scrimmage)
                {
                    item.onClick.Invoke();
                }
                break;
            default:
                break;
        }
        GamePlayerInfo.instance.willOpenMenu = -1;
        GamePlayerInfo.instance.SaveFile();
    }

}
