using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

namespace Interaction
{
    public class InteractorController : MonoBehaviour
    {
        [System.Serializable]
        private class InteractableElement
        {
            [SerializeField] private GameObject interactor;
            [SerializeField] private InputActionReference inputActionReference;

            public GameObject Interactor => interactor;
            public InputActionReference InputActionReference => inputActionReference;

            public XRRayInteractor XrBaseInteractable =>
                Interactor != null ? Interactor.GetComponent<XRRayInteractor>() : null;
        }

        [SerializeField] private List<InteractableElement> interactableElements = new();

        private void Awake()
        {
            foreach (InteractableElement element in interactableElements)
            {
                element.Interactor.SetActive(false);

                if (element.InputActionReference == null) continue;
                element.InputActionReference.action.performed += ctx => ActivateElement(element);
                element.InputActionReference.action.canceled += ctx => DeactivateElement(element);
            }
        }

        private void ActivateElement(InteractableElement element)
        {
            if (element.Interactor != null)
            {
                element.Interactor.SetActive(true);
            }

            if (element.XrBaseInteractable != null)
            {
                element.XrBaseInteractable.enabled = true;
            }
        }

        private void DeactivateElement(InteractableElement element)
        {
            if (element.Interactor != null)
            {
                element.Interactor.SetActive(false);
            }

            if (element.XrBaseInteractable != null)
            {
                element.XrBaseInteractable.enabled = false;
            }
        }
    }
}