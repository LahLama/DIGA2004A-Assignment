using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerRumble : MonoBehaviour
{

    /*
        Title:Add Gamepad RUMBLE with Input System | Easy Unity Tutorial
        Author: Sasquatch B Studios
        Date:  Jan 5 2023
        Availability: https://youtu.be/SmmBC-yCJ28
    */

    private Gamepad gamepad; // Reference to the current connected gamepad
    private bool isRumbling = false; // Tracks if a rumble loop is active
    public bool VibrationsEnabled = true; // Allows vibration to be toggled on and off


    void Update()
    {
        // If vibrations are disabled, ensure all rumble stops
        if (!VibrationsEnabled)
        {
            StopRumbleSteam();
        }
    }

    public void swapBool()
    {
        // Toggle vibration state when this is called
        VibrationsEnabled = !VibrationsEnabled;
    }

    public void RumblePusle(float low, float high, float duration)
    {
        // One time short vibration effect
        if (VibrationsEnabled)
        {
            gamepad = Gamepad.current; // Get the active gamepad
            if (gamepad != null)
            {
                // Set rumble speeds for both motors
                gamepad.SetMotorSpeeds(low, high);

                // Stop rumble after duration time
                Invoke("StopRumblePusle", duration);
            }
        }
        else return; // Exit if vibrations are disabled
    }

    public void RumbleStream(float low, float high, float duration)
    {
        // Continuous rumble effect in a loop
        if (VibrationsEnabled)
        {
            gamepad = Gamepad.current;
            if (!isRumbling && gamepad != null) // Prevent overlapping rumble loops
            {
                isRumbling = true;
                gamepad.SetMotorSpeeds(low, high);
                StartCoroutine(RumbleLoop(low, high, duration, duration)); // Start rumble coroutine
            }
        }
        else return;
    }

    public void StopRumbleSteam()
    {
        // Turns off rumble and stops loop
        if (gamepad != null && isRumbling)
        {
            isRumbling = false;
            gamepad.SetMotorSpeeds(0, 0); // Stop the motors
            StopAllCoroutines(); // Stop rumble coroutine
        }
    }

    private void StopRumblePusle()
    {
        // Stop motors for one-time rumble
        gamepad.SetMotorSpeeds(0, 0);
    }

    private IEnumerator RumbleLoop(float low, float high, float rumbleDuration, float pauseDuration)
    {
        // Continuous loop of rumbling and pausing
        if (VibrationsEnabled)
        {
            gamepad = Gamepad.current;
            while (true && VibrationsEnabled) // Loop until disabled
            {
                if (gamepad != null)
                {
                    gamepad.SetMotorSpeeds(low, high); // Rumble on
                    yield return new WaitForSeconds(rumbleDuration);

                    gamepad.SetMotorSpeeds(0, 0); // Rumble off
                    yield return new WaitForSeconds(pauseDuration);
                }
                else
                {
                    yield return null; // If controller is disconnected, wait until next frame
                }
            }
        }
    }

    public void RumbleTest()
    {
        // Quick vibration test
        if (!VibrationsEnabled) return;
        RumblePusle(1f, 1f, 0.5f);
    }
}
