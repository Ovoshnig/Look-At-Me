using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
 
public class SplashImagePasser : MonoBehaviour
{
    private static PlayerInput s_playerInput;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
    private static void StopSplashScreen()
    {
        s_playerInput = new PlayerInput();
    }

    private void OnEnable() => s_playerInput.Enable();

    private void OnDisable() => s_playerInput.Disable();

    private static void PassSplashImage()
    {
        SplashScreen.Stop(SplashScreen.StopBehavior.FadeOut);
        
    }
}
