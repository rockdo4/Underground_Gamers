using System;
using UnityEngine;

public class SwipeDetector : MonoBehaviour
{
    public event Action OnSwipeLeft;
    public event Action OnSwipeRight;

    [HideInInspector]
    public bool isSwipeable = false;
    public float swipeThreshold = 50f;
    private Vector2 touchStartPos;

    private void Update()
    {
        if (isSwipeable)
        {
            UpdateSwipe();
        }
    }

    private void UpdateSwipe()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    touchStartPos = touch.position;
                    break;

                case TouchPhase.Ended:
                    Vector2 touchEndPos = touch.position;
                    float swipeDistance = Vector2.Distance(touchStartPos, touchEndPos);

                    if (swipeDistance > swipeThreshold)
                    {
                        float swipeDirection = Mathf.Sign(touchEndPos.x - touchStartPos.x);

                        if (swipeDirection > 0f)
                        {
                            OnSwipeRight?.Invoke();
                        }
                        else
                        {
                            OnSwipeLeft?.Invoke();
                        }
                    }
                    break;
            }
        }
    }
}