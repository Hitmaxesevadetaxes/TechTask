using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class VirtualJoystick : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    [Header("UI References")]
    public UnityEngine.UI.Image joystickBackground;
    public UnityEngine.UI.Image joystickHandle;

    [Header("Settings")]
    public float handleRange = 1f;

    // This property gives us the (X, Y) input to use in PlayerController
    public Vector2 InputDirection { get; private set; } = Vector2.zero;

    private Vector2 initialPosition;

    private void Start()
    {
        initialPosition = joystickBackground.rectTransform.position;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 position = Vector2.zero;

        // Calculate position relative to the background
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            joystickBackground.rectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out position))
        {
            // Get the size of the background
            Vector2 size = joystickBackground.rectTransform.sizeDelta;

            // Calculate the input direction (Normalized -1 to 1)
            InputDirection = new Vector2(
                position.x / (size.x / 2),
                position.y / (size.y / 2)
            );

            // Clamp values so it doesn't go over 1.0
            InputDirection = (InputDirection.magnitude > 1.0f) ? InputDirection.normalized : InputDirection;

            // Move the Handle Visuals
            joystickHandle.rectTransform.anchoredPosition = new Vector2(
                InputDirection.x * (size.x / 2) * handleRange,
                InputDirection.y * (size.y / 2) * handleRange
            );
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Reset everything when letting go
        InputDirection = Vector2.zero;
        joystickHandle.rectTransform.anchoredPosition = Vector2.zero;
    }
}