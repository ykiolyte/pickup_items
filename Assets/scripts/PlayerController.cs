using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Joystick joystick;

    private PlayerMovement playerMovement;
    private CameraLook cameraLook;

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        cameraLook = GetComponentInChildren<CameraLook>();

        // Назначаем джойстик
        playerMovement.joystick = joystick;
    }
}