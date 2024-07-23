//Frame Is Not As Much Interactable As Other Details, Thus It Is In Separate Class
public class Frame : Detail
{
    public override int InstalledNode { get; set; } = 0;
    public bool IsInstalled { get; } = true;
    public bool IsUninstallable { get; } = false;

    
    public bool Remove()
    {
        return false;
    }
    
    public override void SetInstalledNode(int installedNode)
    {
        return;
    }
}
