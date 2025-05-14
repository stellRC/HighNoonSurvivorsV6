using Unity.Cinemachine;
using UnityEngine;

public class CameraFollowObject : MonoBehaviour
{
    private float _flipYRotationTime;

    [SerializeField]
    private PlayerMovement _playerTransform;

    private bool _isFacingRight;

    private void Awake()
    {
        _flipYRotationTime = 0.5f;
    }

    void Start()
    {
        _isFacingRight = _playerTransform.IsFacingRight;
    }

    private void FixedUpdate()
    {
        // cameraFollowObject follows player's position
        transform.position = _playerTransform.transform.position;
    }

    public void CallTurn()
    {
        LeanTween
            .rotateY(gameObject, DetermineEndRotation(), _flipYRotationTime)
            .setEaseInOutSine();
    }

    private float DetermineEndRotation()
    {
        _isFacingRight = !_isFacingRight;
        if (_isFacingRight)
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
