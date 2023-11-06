using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class VirtualJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    private Image joystickBackground; // 搖桿的背景
    private Image joystickHandle; // 搖桿的手柄

    private Vector3 inputVector;

    private void Start()
    {
        joystickBackground = GetComponent<Image>();
        joystickHandle = transform.GetChild(0).GetComponent<Image>(); // 假設手柄是搖桿物件的子物件
    }

    public virtual void OnDrag(PointerEventData ped)
    {
        Vector2 pos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(joystickBackground.rectTransform, ped.position, ped.pressEventCamera, out pos))
        {
            pos.x = (pos.x / joystickBackground.rectTransform.sizeDelta.x);
            pos.y = (pos.y / joystickBackground.rectTransform.sizeDelta.y);

            inputVector = new Vector3(pos.x * 2 - 1, 0, pos.y * 2 - 1);
            inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;

            // 移動手柄位置
            joystickHandle.rectTransform.anchoredPosition = new Vector3(inputVector.x * (joystickBackground.rectTransform.sizeDelta.x / 3),
                                                                        inputVector.z * (joystickBackground.rectTransform.sizeDelta.y / 3));
        }
    }

    public virtual void OnPointerDown(PointerEventData ped)
    {
        OnDrag(ped);
    }

    public virtual void OnPointerUp(PointerEventData ped)
    {
        inputVector = Vector3.zero;
        joystickHandle.rectTransform.anchoredPosition = Vector3.zero; // 將手柄歸位
    }

    public float Horizontal()
    {
        if (inputVector.x != 0)
            return inputVector.x;
        else
            return Input.GetAxisRaw("Horizontal");
    }

    public float Vertical()
    {
        if (inputVector.z != 0)
            return inputVector.z;
        else
            return Input.GetAxisRaw("Vertical");
    }
}