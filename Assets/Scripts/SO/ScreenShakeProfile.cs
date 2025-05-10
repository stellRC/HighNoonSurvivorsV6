using UnityEngine;

[CreateAssetMenu(
    fileName = "ScreenShakeProfile",
    menuName = "Scriptable Objects/ScreenShakeProfile"
)]
public class ScreenShakeProfile : ScriptableObject
{
    [Header("Impulse Source Settings")]
    public float impulseTime = 0.2f;
    public float impactForce = 1f;
    public Vector3 defaultVelocity = new(0f, -1f, 0f);
    public AnimationCurve impulseCurve;

    [Header("Impulse Listener Settings")]
    public float listenerAmplitude = 1f;
    public float listenerFrequency = 1f;
    public float listenerDuration = 1f;
}
