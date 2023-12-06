using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class VirtualJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    private Image joystickBackground; // �n�쪺�I��
    private Image joystickHandle; // �n�쪺��`

    private Vector3 inputVector;

    private void Start()
    {
        joystickBackground = GetComponent<Image>();
        joystickHandle = transform.GetChild(0).GetComponent<Image>(); // ���]��`�O�n�쪫�󪺤l����
    }

    public virtual void OnDrag(PointerEventData ped)
    {
        Vector2 pos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(joystickBackground.rectTransform, ped.position, ped.pressEventCamera, out pos))
        {
            pos.x = (pos.x / joystickBackground.rectTransform.sizeDelta.x);
            pos.y = (pos.y / joystickBackground.rectTransform.sizeDelta.y);

            inputVector = new Vector3(pos.x * 2 - 1, pos.y * 2 - 1, 0).normalized;

            // ���ʤ�`��m
            joystickHandle.rectTransform.anchoredPosition = new Vector3(inputVector.x * (joystickBackground.rectTransform.sizeDelta.x / 3),
                                                                        inputVector.y * (joystickBackground.rectTransform.sizeDelta.y / 3));
        }
    }

    public virtual void OnPointerDown(PointerEventData ped)
    {
        OnDrag(ped);
    }

    public virtual void OnPointerUp(PointerEventData ped)
    {
        inputVector = Vector3.zero;
        joystickHandle.rectTransform.anchoredPosition = Vector3.zero; // �N��`�k��
    }

    public float Horizontal()
    {
        return inputVector.x != 0 ? inputVector.x : Input.GetAxisRaw("Horizontal");
    }

    public float Vertical()
    {
        /*if (inputVector.z != 0)
            return inputVector.z;
        else
            return Input.GetAxisRaw("Vertical");*/
        return inputVector.y != 0 ? inputVector.y : Input.GetAxisRaw("Vertical");
    }
}