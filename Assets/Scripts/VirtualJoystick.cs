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

    private Vector2 initialAnchoredPosition;

    private void Start()
    {
        // store anchored position (safer than world position for UI)
        initialAnchoredPosition = joystickBackground.rectTransform.anchoredPosition;

        // Ensure handle is centered at start
        joystickHandle.rectTransform.position = joystickBackground.rectTransform.TransformPoint(Vector3.zero);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 localPoint;

        // Get local point inside the background rect. Use the event camera if available.
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
            joystickBackground.rectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out localPoint))
        {
            // Fallback to main camera if conversion fails (safe fallback)
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                joystickBackground.rectTransform,
                eventData.position,
                Camera.main,
                out localPoint);
        }

        // Use the rect size (handles scaled UI correctly)
        Vector2 rectSize = joystickBackground.rectTransform.rect.size;

        // Calculate normalized (-1..1) input
        InputDirection = new Vector2(
            localPoint.x / (rectSize.x * 0.5f),
            localPoint.y / (rectSize.y * 0.5f)
        );

        if (InputDirection.magnitude > 1f)
            InputDirection = InputDirection.normalized;

        // Compute handle offset in local space
        Vector2 handleLocal = new Vector2(
            InputDirection.x * (rectSize.x * 0.5f) * handleRange,
            InputDirection.y * (rectSize.y * 0.5f) * handleRange
        );

        // Place handle using world position so it works even if handle is not parented to background
        Vector3 worldPos = joystickBackground.rectTransform.TransformPoint(handleLocal);
        joystickHandle.rectTransform.position = worldPos;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Reset input and place handle back to the background center
        InputDirection = Vector2.zero;
        joystickHandle.rectTransform.position = joystickBackground.rectTransform.TransformPoint(Vector3.zero);
    }
}