using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupEnding : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> objs;
    [SerializeField]
    private List<Sprite> imgs;
    [SerializeField]
    private List<int> typeNum;
    private GameObject currObj;

    [SerializeField]
    private Image imageToResize;
    [SerializeField]
    private TMP_Text nameText;
    [SerializeField]
    private TMP_Text infoText;
    [SerializeField]
    private Image typeImage;
    private int currIndex = 0;
    private StringTable st;
    private PlayerTable pt;
    private void Start()
    {
        if (objs.Count > 0)
        {
            currObj = objs[0];
        }
        currIndex = 0;
        RectTransform parentRect = transform.parent.GetComponent<RectTransform>();
        RectTransform imageRect = imageToResize.rectTransform;

        float parentWidth = parentRect.rect.width;
        float parentHeight = parentRect.rect.height;

        float imageWidth = imageToResize.sprite.rect.width;
        float imageHeight = imageToResize.sprite.rect.height;

        float widthRatio = parentWidth / imageWidth;
        float heightRatio = parentHeight / imageHeight;

        float ratio = Mathf.Min(widthRatio, heightRatio);
        float newWidth = imageWidth * ratio;
        float newHeight = imageHeight * ratio;

        imageRect.sizeDelta = new Vector2(newWidth, newHeight);
        imageRect.position = parentRect.position;
        imageRect.anchoredPosition3D = new Vector2(0, 0);

        st = DataTableManager.instance.Get<StringTable>(DataType.String);
        pt = DataTableManager.instance.Get<PlayerTable>(DataType.Player);

        nameText.text = st.Get("alb_name0");
        infoText.text = st.Get("alb_info0");
        typeImage.sprite = pt.playerTypeSprites[typeNum[currIndex]];
    }

    public void OnEnter()
    {
        SoundPlayer.instance.EnterLobbyMusic(1);
    }

    public void ToLeft()
    {
        if (st == null)
        {
            st = DataTableManager.instance.Get<StringTable>(DataType.String);
            pt = DataTableManager.instance.Get<PlayerTable>(DataType.Player);
        }
        currIndex = (currIndex - 1 + imgs.Count) % imgs.Count;

        currObj.SetActive(false);
        if (currIndex < objs.Count && objs[currIndex] != null)
        {
            currObj = objs[currIndex];
        }
        currObj.SetActive(true);
        imageToResize.sprite = imgs[currIndex];

        RectTransform parentRect = transform.parent.GetComponent<RectTransform>();
        RectTransform imageRect = imageToResize.rectTransform;

        float parentWidth = parentRect.rect.width;
        float parentHeight = parentRect.rect.height;

        float imageWidth = imageToResize.sprite.rect.width;
        float imageHeight = imageToResize.sprite.rect.height;

        float widthRatio = parentWidth / imageWidth;
        float heightRatio = parentHeight / imageHeight;

        float ratio = Mathf.Min(widthRatio, heightRatio);
        float newWidth = imageWidth * ratio;
        float newHeight = imageHeight * ratio;

        imageRect.sizeDelta = new Vector2(newWidth, newHeight);
        imageRect.position = parentRect.position;
        imageRect.anchoredPosition3D = new Vector2(0, 0);

        nameText.text = st.Get($"alb_name{currIndex}");
        infoText.text = st.Get($"alb_info{currIndex}");
        typeImage.sprite = pt.playerTypeSprites[typeNum[currIndex]];
    }

    public void ToRight()
    {
        if (st == null)
        {
            st = DataTableManager.instance.Get<StringTable>(DataType.String);
            pt = DataTableManager.instance.Get<PlayerTable>(DataType.Player);
        }
        currIndex = (currIndex + 1) % imgs.Count;

        currObj.SetActive(false);
        if (currIndex < objs.Count && objs[currIndex] != null)
        {
            currObj = objs[currIndex];
        }
        currObj.SetActive(true);
        imageToResize.sprite = imgs[currIndex];

        RectTransform parentRect = transform.parent.GetComponent<RectTransform>();
        RectTransform imageRect = imageToResize.rectTransform;

        float parentWidth = parentRect.rect.width;
        float parentHeight = parentRect.rect.height;

        float imageWidth = imageToResize.sprite.rect.width;
        float imageHeight = imageToResize.sprite.rect.height;

        float widthRatio = parentWidth / imageWidth;
        float heightRatio = parentHeight / imageHeight;

        float ratio = Mathf.Min(widthRatio, heightRatio);
        float newWidth = imageWidth * ratio;
        float newHeight = imageHeight * ratio;

        imageRect.sizeDelta = new Vector2(newWidth, newHeight);
        imageRect.position = parentRect.position;
        imageRect.anchoredPosition3D = new Vector2(0, 0);

        nameText.text = st.Get($"alb_name{currIndex}");
        infoText.text = st.Get($"alb_info{currIndex}");
        typeImage.sprite = pt.playerTypeSprites[typeNum[currIndex]];
    }
}
