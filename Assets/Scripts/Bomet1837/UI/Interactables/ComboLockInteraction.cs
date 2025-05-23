using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerControls;

public class ComboLockInteraction : MonoBehaviour
{
    public bool isInteractable = false;
    
    public GameObject comboLockUI;
    public UIFunctions UIFunctions;
    
    private void Start()
    {
        UIFunctions = GameObject.FindObjectOfType<UIFunctions>();
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(PlayerControls.PlayerControls.interactKey))
        {
            Interact();
        }
    }

    private void OnTriggerStay(Collider other)
    {
       if (!comboLockUI.activeSelf)
       {
           isInteractable = true;
       }
    }
    
    private void OnTriggerExit(Collider other)
    {
       isInteractable = false;
       UIFunctions.CloseUI(comboLockUI);
    }
    
    private void Interact()
    {
        if (isInteractable)
        {
            UIFunctions.ToggleUI(comboLockUI);
            
        }
    }
}