using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CooldownBar : MonoBehaviour
{
    [SerializeField]
    private Transform barTransform;

    [SerializeField]
    private PlayerData playerData;

    public Image cooldownBarFillImage;

    [SerializeField]
    private Image cooldownBarTrailingFillImage;

    [SerializeField]
    private float trailDelay = 0.4f;

    public float currentCool;

    private GameObject playerCharacter;

    private PlayerCombat playerCombat;

    private bool startRefill;

    private void Awake()
    {
        currentCool = playerData.specialAttackTime;

        cooldownBarFillImage.fillAmount = 0f;
        cooldownBarTrailingFillImage.fillAmount = 0f;
    }

    private void Start()
    {
        playerCharacter = GameObject.Find("PlayerCharacter");
        playerCombat = playerCharacter.GetComponent<PlayerCombat>();
        RefillCooldown();
    }

    private void Update()
    {
        if (playerCharacter == null)
        {
            playerCharacter = GameObject.Find("PlayerCharacter");
        }

        barTransform.position = playerCharacter.transform.position;

        if (currentCool == 0)
        {
            startRefill = true;
        }

        if (startRefill)
        {
            RefillCooldown();
        }

        if (currentCool >= playerData.specialAttackTime)
        {
            playerCombat.canUseSpecial = true;
            startRefill = false;
        }
    }

    private void RefillCooldown()
    {
        currentCool += 10f;

        float ratio = currentCool / playerData.specialAttackTime;

        Sequence sequence = DOTween.Sequence();
        sequence.Append(cooldownBarFillImage.DOFillAmount(ratio, 0.25f)).SetEase(Ease.InOutSine);
        // Add delay
        sequence.AppendInterval(trailDelay);
        sequence.Append(cooldownBarFillImage.DOFillAmount(ratio, 0.3f)).SetEase(Ease.InOutSine);
        sequence.Play();
    }

    public void DrainCooldown()
    {
        currentCool -= 10f;
        float ratio = currentCool / playerData.specialAttackTime;

        Sequence sequence = DOTween.Sequence();
        sequence.Append(cooldownBarFillImage.DOFillAmount(ratio, 0.25f)).SetEase(Ease.InOutSine);
        // Add delay
        sequence.AppendInterval(trailDelay);
        sequence.Append(cooldownBarFillImage.DOFillAmount(ratio, 0.3f)).SetEase(Ease.InOutSine);
        sequence.Play();
    }
}
