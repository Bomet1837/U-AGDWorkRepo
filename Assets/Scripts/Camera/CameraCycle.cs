using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraCycle : MonoBehaviour
{
    [SerializeField] private CinemachineFreeLook[] _cameras; 
    [SerializeField] private int _currentCameraIndex = 0;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            for (int i = 0; i < _cameras.Length; i++)
            {
                _cameras[i].Priority = 0;
            }
            
            _cameras[_currentCameraIndex].Priority = 10;
        }
    }
}
