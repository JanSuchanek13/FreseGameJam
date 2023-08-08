using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoBackInUI : MonoBehaviour
{

    [Header("REFERENCES")]
    [Tooltip("Reference to the PlayerInput Action Mapping")]
    private PlayerInput playerInput;
    private Transform[] UIPages;
    private Transform currentPage;
    private Transform currentBackButton;

    private void OnEnable()
    {
        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }

    private void Awake()
    {
        playerInput = new PlayerInput();
    }

    private void Update()
    {
        SetCurrentPage();
        SetCurrentBackButton();
        if (playerInput.CharacterControls.GOBackInUI.triggered)
        {
            GoBack();
        }
    }

    private void SetCurrentPage()
    {
        foreach (Transform page in transform)
        {
            if (page.gameObject.activeInHierarchy)
            {
                currentPage = page;
            }
        }
    }

    private void SetCurrentBackButton()
    {
        if(currentPage == null)
        {
            return;
        }

        foreach (Transform child in currentPage)
        {
            foreach (Transform grandchild in child)
            {
                string name = grandchild.name;
                if ("BUT_Back".Equals(name) || "BUTTON_No".Equals(name) || "Button_Back".Equals(name) || "Button_No".Equals(name))
                //if (grandchild.name == "BUT_Back" || grandchild.name == "BUTTON_No")
                {
                    currentBackButton = grandchild;
                }
            }
        }
    }

    private void GoBack()
    {
        Debug.Log("Go back in UI");
        if (currentBackButton.gameObject.activeInHierarchy)
        {
            currentBackButton.GetComponent<Button>().onClick.Invoke();
        }
        
    }
}
