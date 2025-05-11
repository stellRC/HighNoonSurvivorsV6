using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CooldownBar : MonoBehaviour
{
    [SerializeField]
    private Transform barTransform;

    [SerializeField]
    private Image cooldownBarFillImage;

    [SerializeField]
    private Image cooldownBarTrailingFillImage;

    [SerializeField]
    private float trailDelay = 0.4f;

    public float currentCool;

    private PlayerMovement playerCharacter;

    public bool startRefill;

    public bool startDrain;

    private void Awake()
    {
        playerCharacter = FindAnyObjectByType<PlayerMovement>();
        ResetValues();
    }

    public void ResetValues()
    {
        currentCool = 0;
        cooldownBarFillImage.fillAmount = 0f;
        cooldownBarTrailingFillImage.fillAmount = 0f;
    }

    private void Update()
    {
        // Assign transform when player is destroyed when loading scene
        if (playerCharacter == null)
        {
            playerCharacter = FindAnyObjectByType<PlayerMovement>();
        }

        barTransform.position = new Vector2(
            playerCharacter.transform.position.x,
            playerCharacter.transform.position.y - .5f
        );

        if (cooldownBarFillImage.fillAmount <= 0.09)
        {
            startDrain = false;
        }

        if (startDrain)
        {
            DrainCooldown();
        }
    }

    public void RefillCooldown()
    {
        if (currentCool < 0)
        {
            currentCool = 0;
        }

        currentCool += 1f;

        float ratio = currentCool / GameManager.Instance.levelData.specialAttackRate;

        Sequence sequence = DOTween.Sequence();
        sequence.Append(cooldownBarFillImage.DOFillAmount(ratio, 0.25f)).SetEase(Ease.InOutSine);
        // Add delay
        sequence.AppendInterval(trailDelay);
        sequence.Append(cooldownBarFillImage.DOFillAmount(ratio, 0.3f)).SetEase(Ease.InOutSine);
        sequence.Play();
    }

    public void DrainCooldown()
    {
        if (currentCool < 0)
        {
            currentCool = 0;
        }

        currentCool -= 1f;
        float ratio = currentCool / GameManager.Instance.levelData.specialAttackRate;

        Sequence sequence = DOTween.Sequence();
        sequence.Append(cooldownBarFillImage.DOFillAmount(ratio, 0.25f)).SetEase(Ease.InOutSine);
        // Add delay
        sequence.AppendInterval(trailDelay);
        sequence.Append(cooldownBarFillImage.DOFillAmount(ratio, 0.3f)).SetEase(Ease.InOutSine);
        sequence.Play();
    }
}
