using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using System.Collections;

public class ButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [Header("Text Settings")]
    public TextMeshProUGUI buttonText;
    public Color normalColor = Color.white;
    public Color hoverColor = Color.yellow;
    public Color pressedColor = Color.red;

    [Header("Pressed Timing")]
    public float pressedHoldTime = 0.15f; // how long the pressed color stays

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip hoverSound;
    public AudioClip pressSound;

    private bool isHovering;
    private Coroutine pressedRoutine;

    void Start()
    {
        if (buttonText == null)
            buttonText = GetComponentInChildren<TextMeshProUGUI>();

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        buttonText.color = normalColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;

        if (pressedRoutine == null) // don’t override pressed state early
            buttonText.color = hoverColor;

        if (audioSource && hoverSound)
            audioSource.PlayOneShot(hoverSound);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;

        if (pressedRoutine == null) // only return if not in pressed delay
            buttonText.color = normalColor;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (pressedRoutine != null)
            StopCoroutine(pressedRoutine);

        buttonText.color = pressedColor;

        if (audioSource && pressSound)
            audioSource.PlayOneShot(pressSound);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (pressedRoutine != null)
            StopCoroutine(pressedRoutine);

        pressedRoutine = StartCoroutine(PressHoldDelay());
    }

    private IEnumerator PressHoldDelay()
    {
        // Keep the pressed color visible
        yield return new WaitForSeconds(pressedHoldTime);

        // After hold, switch back correctly
        buttonText.color = isHovering ? hoverColor : normalColor;

        pressedRoutine = null;
    }
}
