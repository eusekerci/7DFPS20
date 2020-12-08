using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private Ui _ui;

    [SerializeField]
    private PlayerMotor _playerMotor;

    [SerializeField]
    private MouseLook _mouseLook;

    private void Start()
    {
        _ui.StartFlash();
    }

    private void Update()
    {
        float dt = Time.deltaTime;
        _mouseLook.Tick(dt);
        _playerMotor.Tick(dt);
    }
}
