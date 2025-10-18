using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputDeviceDetector : MonoBehaviour
{
    private InputAction input;
    public DeviceType currentDevice;

    public static Func<DeviceType, DeviceType> OnDeviceChange;

    void Awake()
    {
        input = new InputAction(binding: "/*/<button>");
        input.performed += OnInputPressed;
        currentDevice = DeviceType.Keyboard;
    }

    void OnEnable()
    {
        input.Enable();
    }

    void OnDisable()
    {
        input.Disable();
    }

    void OnInputPressed(InputAction.CallbackContext context)
    {
        InputDevice inputDevice = context.control.device;
        string deviceName = inputDevice.displayName.ToLower();

        if (inputDevice is Gamepad)
        {
            currentDevice = DeviceType.Gamepad;
        }
        else if (inputDevice is Keyboard)
        {
            currentDevice = DeviceType.Keyboard;
        }
        else if (inputDevice is Mouse)
        {
            currentDevice = DeviceType.Mouse;
        }

        OnDeviceChange?.Invoke(currentDevice);
    }

    public enum DeviceType
    {
        Keyboard,
        Mouse,
        Gamepad
    }
}