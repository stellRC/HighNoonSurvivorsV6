using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance;

    [SerializeField]
    private float _globalShakeForce = 1f;

    [SerializeField]
    private CinemachineImpulseListener _impulseListener;

    private CinemachineImpulseDefinition _impulseDefinition;

    [SerializeField]
    private List<ScreenShakeProfile> ProfileList = new();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void ShakeCamera(CinemachineImpulseSource impulseSource)
    {
        impulseSource.GenerateImpulseWithForce(_globalShakeForce);
    }

    public void ScreenShakeFromProfile(int profileID, CinemachineImpulseSource impulseSource)
    {
        // Apply settings
        SetupScreenShakeSettings(ProfileList[profileID], impulseSource);
        // perform shake
        impulseSource.GenerateImpulseWithForce(ProfileList[profileID].impactForce);
    }

    private void SetupScreenShakeSettings(
        ScreenShakeProfile profile,
        CinemachineImpulseSource impulseSource
    )
    {
        if (_impulseListener == null)
        {
            _impulseListener = FindFirstObjectByType<CinemachineImpulseListener>();
        }

        // Time of shake should align with time of special attack
        _impulseDefinition = impulseSource.ImpulseDefinition;

        //  impulse source settings
        _impulseDefinition.ImpulseDuration = GameManager.Instance.LevelData.specialAttackRate;
        impulseSource.DefaultVelocity = profile.defaultVelocity;
        _impulseDefinition.CustomImpulseShape = profile.impulseCurve;

        // impulse listener settings
        _impulseListener.ReactionSettings.AmplitudeGain = profile.listenerAmplitude;
        _impulseListener.ReactionSettings.FrequencyGain = profile.listenerFrequency;
        _impulseListener.ReactionSettings.Duration = profile.listenerDuration;
    }
}
