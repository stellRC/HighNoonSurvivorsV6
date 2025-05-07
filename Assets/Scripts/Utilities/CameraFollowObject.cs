using UnityEngine;

public class CameraFollowObject : MonoBehaviour
{
    private float flipYRotationTime;
    private Transform playerTransform;

    private PlayerMovement playerMovement;
    private bool isFacingRight;

    private void Awake()
    {
        playerMovement = FindAnyObjectByType<PlayerMovement>();
        playerTransform = playerMovement.transform;
        isFacingRight = playerMovement.IsFacingRight;

        flipYRotationTime = 0.5f;
    }

    private void FixedUpdate()
    {
        // cameraFollowObject follows player's position
        transform.position = playerTransform.position;
    }

    public void CallTurn()
    {
        LeanTween.rotateY(gameObject, DetermineEndRotation(), flipYRotationTime).setEaseInOutSine();
    }

    private float DetermineEndRotation()
    {
        isFacingRight = !isFacingRight;
        if (isFacingRight)
        {
            return 180f;
        }
        else
        {
            return 0f;
        }
    }
}
