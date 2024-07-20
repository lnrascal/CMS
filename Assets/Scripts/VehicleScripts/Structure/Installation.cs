using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Installation
{
    [field: SerializeField]
    public bool IsInstalled { get; set; }
    [field: SerializeField]
    public Detail Detail { get; set; }

    public void Install(Detail detail)
    {
        IsInstalled = true;
        Detail = detail;
    }

    public void Uninstall()
    {
        IsInstalled = false;
        Detail = null;
    }
}
