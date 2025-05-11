using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake instance;

    [SerializeField]
    private float globalShakeForce = 1f;

    [SerializeField]
    private PlayerData playerData;

    [SerializeField]
    private CinemachineImpulseListener impulseListener;

    private CinemachineImpulseDefinition impulseDefinition;

    public List<ScreenShakeProfile> profileList = new();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void ShakeCamera(CinemachineImpulseSource impulseSource)
    {
        impulseSource.GenerateImpulseWithForce(globalShakeForce);
    }

    public void ScreenShakeFromProfile(int profileID, CinemachineImpulseSource impulseSource)
    {
        // Apply settings
        SetupScreenShakeSettings(profileList[profileID], impulseSource);
        // perform shake
        impulseSource.GenerateImpulseWithForce(profileList[profileID].impactForce);
    }

    private void SetupScreenShakeSettings(
        ScreenShakeProfile profile,
        CinemachineImpulseSource impulseSource
    )
    {
        if (impulseListener == null)
        {
            impulseListener = FindFirstObjectByType<CinemachineImpulseListener>();
        }

        // Time of shake should align with time of special attack
        impulseDefinition = impulseSource.ImpulseDefinition;

        //  impulse source settings
        impulseDefinition.ImpulseDuration = playerData.specialAttackCount;
        impulseSource.DefaultVelocity = profile.defaultVelocity;
        impulseDefinition.CustomImpulseShape = profile.impulseCurve;

        // impulse listener settings
        impulseListener.ReactionSettings.AmplitudeGain = profile.listenerAmplitude;
        impulseListener.ReactionSettings.FrequencyGain = profile.listenerFrequency;
        impulseListener.ReactionSettings.Duration = profile.listenerDuration;
    }
}
