using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DartRespawner : MonoBehaviour
{
    // Prefab-ul dart-ului
    public GameObject throwing_dart;

    void Start()
    {
        //spawn initial dart
        SpawnNewDart();
    }

    public void SpawnNewDart()
    {
        GameObject newDart = Instantiate(throwing_dart, transform.position, transform.rotation);

        //when one is grabbed, spawn another one
        XRGrabInteractable grabInteractable = newDart.GetComponent<XRGrabInteractable>();
        if (grabInteractable != null)
        {
            //
            grabInteractable.selectEntered.AddListener(OnGrab);
        }
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        //optional: remove listener from the grabbed dart
        XRGrabInteractable grabInteractable = args.interactableObject.transform.GetComponent<XRGrabInteractable>();
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.RemoveListener(OnGrab);
        }

        //spawn new dart
        SpawnNewDart();
    }
}