using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CooldownBar : MonoBehaviour
{
    [SerializeField]
    private Transform _barTransform;

    [SerializeField]
    private Image _cooldownBarFillImage;

    [SerializeField]
    private Image _cooldownBarTrailingFillImage;

    [SerializeField]
    private float _trailDelay = 0.4f;

    private float _currentCool;

    private PlayerMovement _playerCharacter;

    public bool StartDrain;

    private void Awake()
    {
        _playerCharacter = FindAnyObjectByType<PlayerMovement>();
        ResetValues();
    }

    public void ResetValues()
    {
        _currentCool = 0;
        _cooldownBarFillImage.fillAmount = 0f;
        _cooldownBarTrailingFillImage.fillAmount = 0f;
    }

    private void Update()
    {
        // Assign transform when player is destroyed when loading scene
        if (_playerCharacter == null)
        {
            _playerCharacter = FindAnyObjectByType<PlayerMovement>();
        }

        _barTransform.position = new Vector2(
            _playerCharacter.transform.position.x,
            _playerCharacter.transform.position.y - .5f
        );

        if (_cooldownBarFillImage.fillAmount <= 0.09)
        {
            StartDrain = false;
        }

        if (StartDrain)
        {
            DrainCooldown();
        }
    }

    public void RefillCooldown()
    {
        if (_currentCool < 0)
        {
            _currentCool = 0;
        }

        _currentCool += 1f;

        float ratio = _currentCool / GameManager.Instance.LevelData.SpecialAttackRate;

        Sequence sequence = DOTween.Sequence();
        sequence.Append(_cooldownBarFillImage.DOFillAmount(ratio, 0.25f)).SetEase(Ease.InOutSine);
        // Add delay
        sequence.AppendInterval(_trailDelay);
        sequence
            .Append(_cooldownBarTrailingFillImage.DOFillAmount(ratio, 0.3f))
            .SetEase(Ease.InOutSine);
        sequence.Play();
    }

    public void DrainCooldown()
    {
        if (_currentCool < 0)
        {
            _currentCool = 0;
        }

        _currentCool -= 1f;
        float ratio = _currentCool / GameManager.Instance.LevelData.SpecialAttackRate;

        Sequence sequence = DOTween.Sequence();
        sequence.Append(_cooldownBarFillImage.DOFillAmount(ratio, 0.25f)).SetEase(Ease.InOutSine);
        // Add delay
        sequence.AppendInterval(_trailDelay);
        sequence
            .Append(_cooldownBarTrailingFillImage.DOFillAmount(ratio, 0.3f))
            .SetEase(Ease.InOutSine);
        sequence.Play();
    }
}
