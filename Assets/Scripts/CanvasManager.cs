using Lightbug.CharacterControllerPro.Core;
using Unity.VisualScripting;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] private GameObject inventoryUI;
    [SerializeField] private GameObject defaultUI;
    
    [SerializeField] private CharacterActor characterActor;
    [SerializeField] private GameObject playerCamera;
    private bool isOpen = false;
    
    public void OpenInventory(bool toOpen)
    {
        if (isOpen == toOpen)
            return;

        isOpen = toOpen;
        inventoryUI.gameObject.SetActive(toOpen);
        defaultUI.gameObject.SetActive(!toOpen);

        characterActor.enabled = !toOpen;
        playerCamera.SetActive(!toOpen);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            OpenInventory(!isOpen);
        }
    }
}
