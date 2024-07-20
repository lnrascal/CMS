using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    void Interact(PlayerInteract player);

    void Highlight(bool toHighlight);

    void Drop();
}
