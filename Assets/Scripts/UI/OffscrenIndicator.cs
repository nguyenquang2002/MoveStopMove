using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OffscrenIndicator : MonoBehaviour
{
    [SerializeField] Image imageIndicator;

    private GameObject target;
    private Camera cam;
    private RectTransform canvasRect;

    [SerializeField] float edgeBuffer = 40f;

    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    public void InitialiseTargetIndicator(GameObject target, Camera mainCamera, Canvas canvas)
    {
        this.target = target;
        cam = mainCamera;
        canvasRect = canvas.GetComponent<RectTransform>();
        gameObject.SetActive(false);
    }

    public void UpdateTargetIndicator()
    {
        SetIndicatorPosition();
    }
    protected void SetIndicatorPosition()
    {
        Vector3 indicatorPosition = cam.WorldToScreenPoint(target.transform.position);
        bool isOnScreen = indicatorPosition.z >= 0f && indicatorPosition.x <= canvasRect.rect.width * canvasRect.localScale.x
         && indicatorPosition.y <= canvasRect.rect.height * canvasRect.localScale.x && indicatorPosition.x >= 0f && indicatorPosition.y >= 0f;

        if (isOnScreen)
        {
            indicatorPosition.z = 0f;
            gameObject.SetActive(false);
        }
        else if(indicatorPosition.z >= 0f)
        {
            indicatorPosition = OutOfRangeindicatorPositionB(indicatorPosition);
            imageIndicator.rectTransform.rotation = Quaternion.Euler(RotateVectorIndicator(indicatorPosition));
            gameObject.SetActive(true);
        }
        else
        {
            indicatorPosition *= -1f;
            indicatorPosition = OutOfRangeindicatorPositionB(indicatorPosition);
            imageIndicator.rectTransform.rotation = Quaternion.Euler(RotateVectorIndicator(indicatorPosition));
            imageIndicator.gameObject.SetActive(true);
        }
        if (!target.activeSelf)
        {
            gameObject.SetActive(false);
        }
        rectTransform.position = indicatorPosition;
    }

    private Vector3 OutOfRangeindicatorPositionB(Vector3 indicatorPosition)
    {
        indicatorPosition.z = 0f;

        Vector3 canvasCenter = new Vector3(canvasRect.rect.width / 2f, canvasRect.rect.height / 2f, 0f) * canvasRect.localScale.x;
        indicatorPosition -= canvasCenter;

        float divX = (canvasRect.rect.width / 2f - edgeBuffer) / Mathf.Abs(indicatorPosition.x);
        float divY = (canvasRect.rect.height / 2f - edgeBuffer) / Mathf.Abs(indicatorPosition.y);

        if (divX < divY)
        {
            float angle = Vector3.SignedAngle(Vector3.right, indicatorPosition, Vector3.forward);
            indicatorPosition.x = Mathf.Sign(indicatorPosition.x) * (canvasRect.rect.width * 0.5f - edgeBuffer) * canvasRect.localScale.x;
            indicatorPosition.y = Mathf.Tan(Mathf.Deg2Rad * angle) * indicatorPosition.x;
        }

        else
        {
            float angle = Vector3.SignedAngle(Vector3.up, indicatorPosition, Vector3.forward);

            indicatorPosition.y = Mathf.Sign(indicatorPosition.y) * (canvasRect.rect.height / 2f - edgeBuffer) * canvasRect.localScale.y;
            indicatorPosition.x = -Mathf.Tan(Mathf.Deg2Rad * angle) * indicatorPosition.y;
        }

        indicatorPosition += canvasCenter;
        return indicatorPosition;
    }

    private Vector3 RotateVectorIndicator(Vector3 indicatorPosition)
    {
        Vector3 canvasCenter = new Vector3(canvasRect.rect.width / 2f, canvasRect.rect.height / 2f, 0f) * canvasRect.localScale.x;

        float angle = Vector3.SignedAngle(Vector3.up, indicatorPosition - canvasCenter, Vector3.forward);

        return new Vector3(0, 0, angle);
    }
}
