using Unity.Cinemachine;
using UnityEngine;

public class CameraFollowObject : MonoBehaviour
{
    private float flipYRotationTime;

    [SerializeField]
    private PlayerMovement playerTransform;

    private bool isFacingRight;

    private void Awake()
    {
        flipYRotationTime = 0.5f;
    }

    void Start()
    {
        isFacingRight = playerTransform.IsFacingRight;
    }

    private void FixedUpdate()
    {
        // cameraFollowObject follows player's position
        transform.position = playerTransform.transform.position;
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


// target offset -3 when moving left and 2 when facing right
