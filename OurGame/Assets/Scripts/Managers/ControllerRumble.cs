using System.Collections;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerRumble : MonoBehaviour
{

    //https://youtu.be/SmmBC-yCJ28
    private float _rumbleDuration;
    private float _lowfreq;
    private float _highfreq;
    private bool isMotiveActive = false;
    private Gamepad gamepad;

    public void RumbleStream(float low, float high, float duration)
    {
        //Get the current gamepad
        gamepad = Gamepad.current;

        if (gamepad != null)
        {
            //Start rumble
            gamepad.SetMotorSpeeds(low, high);

        }
    }

    public void StopRumbleSteam()
    {
        if (gamepad != null)
        {
            //Start rumble
            gamepad.SetMotorSpeeds(0, 0);

        }
    }
    private IEnumerator stopRumble(float duration, Gamepad pad)
    {
        float timeElapsed = 0f;
        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        //now duration is done
        pad.SetMotorSpeeds(0, 0);
    }
}
