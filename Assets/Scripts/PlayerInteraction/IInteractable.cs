//Interface For Interaction With Details And Other Objects such as CarLift
public interface IInteractable
{
    void Interact(PlayerInteract player);

    void Highlight(bool toHighlight);

    void Drop();
}
