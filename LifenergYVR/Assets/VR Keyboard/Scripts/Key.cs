using UnityEngine;
using UnityEngine.UI;

namespace Keyboard
{
    public class Key : MonoBehaviour
    {
        [SerializeField] protected KeyChannel keyChannel;
        [SerializeField] protected KeyboardManager keyboard;
        protected Button button;

        protected virtual void Awake()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(OnPress);
            keyboard.onKeyboardModeChanged.AddListener(UpdateKey);
            keyChannel.onFirstKeyPress.AddListener(UpdateKey);
            keyChannel.OnKeyColorsChanged += ChangeKeyColors; 
            keyChannel.OnKeysStateChange += ChangeKeyState;
        }

        protected virtual void OnDestroy()
        {
            button.onClick.RemoveListener(OnPress);
            keyboard.onKeyboardModeChanged.RemoveListener(UpdateKey);
            keyChannel.onFirstKeyPress.RemoveListener(UpdateKey);
            keyChannel.OnKeyColorsChanged -= ChangeKeyColors;
            keyChannel.OnKeysStateChange -= ChangeKeyState;
        }

        protected virtual void OnPress()
        {
            keyboard.DeactivateShift();
        }

        protected virtual void UpdateKey()
        {
            // empty method for override in child classes
        }
        
        protected void ChangeKeyColors(Color normalColor, Color highlightedColor, Color pressedColor, Color selectedColor)
        {
            ColorBlock cb = button.colors;
            cb.normalColor = normalColor;
            cb.highlightedColor = highlightedColor;
            cb.pressedColor = pressedColor;
            cb.selectedColor = selectedColor;
            button.colors = cb;
        }

        protected void ChangeKeyState(bool enabled)
        {
            button.interactable = enabled;
        }
    }
}