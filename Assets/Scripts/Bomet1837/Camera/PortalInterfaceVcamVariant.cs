using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

using Cinemachine;

public class PortalInterfaceVcamVariant: MonoBehaviour, IInteraction
{
    [Header("Camera Settings")]

    [Tooltip("The index of the camera that will be used.")]
    [SerializeField] private int _currentCameraIndex = 0;
    
    [Tooltip("How far the player moves along the X axis.")]
    [SerializeField] private float _travelLocation = 1000;

    [Header("Audio Settings")]
    public AudioSource warpSound;
    
    public BoxCollider boxCollider; //Used for drawing the box of the travel location
    
    
    
    public void Interact()
    {
        TTCCinemachineVariant.Instance.TimeTravel(_currentCameraIndex, _travelLocation);
        warpSound.Play();
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (boxCollider == null)
        {
            boxCollider = GetComponent<BoxCollider>();
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        //Used to help calculate the travel location
        if (boxCollider == null)
            boxCollider = GetComponent<BoxCollider>();

        if (boxCollider != null)
        {
            //Draw the cube
            Gizmos.color = Color.red;
            Vector3 newPosition = new Vector3(this.transform.position.x + _travelLocation, this.transform.position.y, this.transform.position.z);
            Gizmos.DrawWireCube(newPosition + boxCollider.center, boxCollider.size);
            
            //Draw the text
            GUIStyle style = new GUIStyle();
            style.normal.textColor = Color.white;
            style.alignment = TextAnchor.MiddleCenter;
            style.fontSize = 12;

            Handles.Label(newPosition + boxCollider.center, "Portal Destination", style);
        }
    }
#endif
    
    
}