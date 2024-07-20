using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frame : Detail
{
    public override int InstalledNode { get; set; } = 0;
    public override bool IsInstalled { get; } = true;
    public override bool IsUninstallable { get; } = false;

    
    public override bool Remove()
    {
        return false;
    }
    
    public override void SetInstalledNode(int installedNode)
    {
        return;
    }
}
