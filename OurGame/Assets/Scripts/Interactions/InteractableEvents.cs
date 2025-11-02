using System;
using UnityEngine;

public static class InteractableEvents
{
    public static event Action<string> OnObjectInteracted;

    public static void RaiseObjectInteracted(string objectName)
    {
        OnObjectInteracted?.Invoke(objectName);
    }
}
