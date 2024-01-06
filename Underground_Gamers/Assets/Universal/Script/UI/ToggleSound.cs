using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ToggleSound : MonoBehaviour
{
    public EffectType effectType = EffectType.Option_Button;
    private Toggle toggle;

    private void Awake()
    {
        toggle = GetComponent<Toggle>();
        EventTrigger eventTrigger = GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick; // 이벤트 유형 설정

        // 이벤트에 실행할 함수 추가
        entry.callback.AddListener((eventData) => SoundPlayer.instance.PlayEffectSound((int)effectType)) ;

        // EventTrigger에 이벤트 핸들러 추가
        eventTrigger.triggers.Add(entry);
    }
}
