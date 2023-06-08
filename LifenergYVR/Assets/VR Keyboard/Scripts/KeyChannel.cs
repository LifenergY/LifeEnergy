using UnityEngine;
using UnityEngine.Events;

//[CreateAssetMenu(fileName = "KeyChannel", menuName = "Channels/KeyChannel")]
namespace Keyboard
{
    public class KeyChannel : ScriptableObject
    {
        public UnityAction<string> OnKeyPressed;
        public UnityAction<Color, Color, Color, Color> OnKeyColorsChanged;
        public UnityAction<bool> OnKeysStateChange;
        public UnityEvent onFirstKeyPress;

        public void RaiseKeyPressedEvent(string key) =>
            OnKeyPressed?.Invoke(key);

        public void RaiseKeyColorsChangedEvent(Color normalColor, Color highlightedColor, Color pressedColor,
            Color selectedColor) =>
            OnKeyColorsChanged?.Invoke(normalColor, highlightedColor, pressedColor, selectedColor);

        public void RaiseKeysStateChangeEvent(bool enabled) =>
            OnKeysStateChange?.Invoke(enabled);
    }
}