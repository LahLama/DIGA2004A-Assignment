using System.Collections;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerRumble : MonoBehaviour
{

    //https://youtu.be/SmmBC-yCJ28
    private Gamepad gamepad;
    private bool isRumbling = false;

    public void RumbleStream(float low, float high, float duration)
    {
        gamepad = Gamepad.current;
        if (!isRumbling && gamepad != null)
        {
            isRumbling = true;
            gamepad.SetMotorSpeeds(low, high);
            StartCoroutine(RumbleLoop(low, high, duration, duration));
        }
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

    private IEnumerator RumbleLoop(float low, float high, float rumbleDuration, float pauseDuration)
    {
        gamepad = Gamepad.current;
        while (true)
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
