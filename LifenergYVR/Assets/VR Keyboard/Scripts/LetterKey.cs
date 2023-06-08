using TMPro;
using UnityEngine;

namespace Keyboard
{
    public class LetterKey : Key
    {
        [SerializeField] private string character;
        private TextMeshProUGUI buttonText;

        protected override void Awake()
        {
            base.Awake();
            buttonText = GetComponentInChildren<TextMeshProUGUI>();
            InitializeKey();
        }

        private void InitializeKey() => buttonText.text = keyboard.autoCapsAtStart ? character.ToUpper() : character;

        protected override void OnPress()
        {
            base.OnPress();
            keyChannel.RaiseKeyPressedEvent(character);
        }

        protected override void UpdateKey()
        {
            if(keyboard.IsShiftActive() || keyboard.IsCapsLockActive())
            {
                buttonText.text = character.ToUpper();
            }
            else
            {
                buttonText.text = character.ToLower();
            }
        }
    }
}