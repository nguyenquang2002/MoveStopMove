using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class JoystickManager : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    private Image imgJoystickBG, imgJoystickHandle;
    private Vector2 posInput;

    // Start is called before the first frame update
    void Start()
    {
        imgJoystickBG = transform.GetChild(0).GetComponent<Image>();
        imgJoystickHandle = transform.GetChild(0).GetChild(0).GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnDrag(PointerEventData eventData)
    {
        if(RectTransformUtility.ScreenPointToLocalPointInRectangle(
            imgJoystickBG.rectTransform, eventData.position, eventData.pressEventCamera, out posInput))
        {
            posInput.x = posInput.x / (imgJoystickBG.rectTransform.sizeDelta.x);
            posInput.y = posInput.y / (imgJoystickBG.rectTransform.sizeDelta.y);

            if(posInput.magnitude > 1.0f)
            {
                posInput = posInput.normalized;
            }

            imgJoystickHandle.rectTransform.anchoredPosition = new Vector2(
                posInput.x * imgJoystickBG.rectTransform.sizeDelta.x / 2, 
                posInput.y * imgJoystickBG.rectTransform.sizeDelta.y / 2);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        posInput = Vector2.zero;
        imgJoystickHandle.rectTransform.anchoredPosition = Vector2.zero;
    }

    public float InputHorizontal()
    {
        if (posInput.x != 0)
        {
            return posInput.x;
        }
        else return Input.GetAxisRaw("Horizontal");
    }

    public float InputVertical()
    {
        if (posInput.y != 0)
        {
            return posInput.y;
        }
        else return Input.GetAxisRaw("Vertical");
    }
}
