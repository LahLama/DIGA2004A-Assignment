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

    private Gamepad gamepad;
    private bool isRumbling = false;
    public bool VibrationsEnabled = true;

    void Update()
    {
        if (!VibrationsEnabled)
        {
            StopRumbleSteam();
        }
    }

    public void RumblePusle(float low, float high, float duration)
    {
        if (VibrationsEnabled)
        {
            gamepad = Gamepad.current;
            if (gamepad != null)
            {

                gamepad.SetMotorSpeeds(low, high);
                Invoke("StopRumblePusle", duration);
            }
        }
        else return;
    }
    public void RumbleStream(float low, float high, float duration)
    {

        if (VibrationsEnabled)
        {
            gamepad = Gamepad.current;
            if (!isRumbling && gamepad != null)
            {
                isRumbling = true;
                gamepad.SetMotorSpeeds(low, high);
                StartCoroutine(RumbleLoop(low, high, duration, duration));
            }
        }
        else return;
    }

    public void StopRumbleSteam()
    {


        if (gamepad != null && isRumbling)
        {
            isRumbling = false;
            gamepad.SetMotorSpeeds(0, 0);
            StopAllCoroutines();
        }

    }

    private void StopRumblePusle()
    {
        gamepad.SetMotorSpeeds(0, 0);
    }

    private IEnumerator RumbleLoop(float low, float high, float rumbleDuration, float pauseDuration)
    {
        if (VibrationsEnabled)
        {
            gamepad = Gamepad.current;
            while (true && VibrationsEnabled)
            {
                if (gamepad != null)
                {
                    gamepad.SetMotorSpeeds(low, high);
                    yield return new WaitForSeconds(rumbleDuration);

                    gamepad.SetMotorSpeeds(0, 0);
                    yield return new WaitForSeconds(pauseDuration);
                }
                else
                {
                    yield return null;
                }
            }
        }
    }
}
