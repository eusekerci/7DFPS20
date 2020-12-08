using System.Collections;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public Camera MainCamera;
    public Camera WeaponCamera;
    public AnimationCurve FovKickCurve;
    public bool IsInverted = false;

    private const int SensitivityMax = 300;
    private const int SensitivityMin = 50;

    private const int FovMax = 110;
    private const int FovMin = 60;

    private float _sensitivity = 150;
    private float _zoomSensitivity = 30;

    private float _fov = 60;
    private const float ZoomFov = 10;
    private const float ZoomSpeed = 40;

    private float _currentFov = 60;
    private float _pitch = 0;

    public void OnSensitivityChanged(float normalizedValue)
    {
        _sensitivity = Mathf.Lerp(SensitivityMin, SensitivityMax, normalizedValue);
        _zoomSensitivity = _sensitivity / 5f;
    }

    public void OnFovChanged(float normalizedValue)
    {
        _fov = Mathf.Lerp(FovMin, FovMax, normalizedValue);
    }

    public void Tick(float dt)
    {
        var t = Mathf.Lerp(0f, 1f, (_currentFov - ZoomFov) / (_fov - ZoomFov));
        var sensitivity = Mathf.Lerp(_zoomSensitivity, _sensitivity, t);

        _pitch += Input.GetAxis("Mouse Y") * sensitivity * dt * (IsInverted ? 1 : -1);
        _pitch = Mathf.Clamp(_pitch, -89, 89);

        MainCamera.transform.localRotation = Quaternion.Euler(Vector3.right * _pitch);
        transform.rotation *= Quaternion.Euler(Input.GetAxis("Mouse X") * sensitivity * dt * Vector3.up);

        MainCamera.fieldOfView = Mathf.Lerp(MainCamera.fieldOfView, _currentFov, ZoomSpeed * dt);
        WeaponCamera.fieldOfView = Mathf.Lerp(WeaponCamera.fieldOfView, _currentFov, ZoomSpeed * dt);

        if (Input.GetKey(KeyCode.LeftAlt))
        {
            _currentFov = ZoomFov;
        }
        else
        {
            _currentFov = _fov;
        }
    }

    public void ResetAt(Transform t, Transform lookAt)
    {
        Gravity.Set(Vector3.down);

        transform.rotation = Quaternion.LookRotation(t.forward, Gravity.Up);

        Vector3 lookDir;
        if (lookAt != null)
        {
            lookDir = (lookAt.position - t.position).normalized;
        }
        else
        {
            lookDir = t.forward;
        }

        Quaternion resetRot = Quaternion.LookRotation(lookDir, Gravity.Up);
        if (resetRot.eulerAngles.x < 90)
        {
            _pitch = resetRot.eulerAngles.x;
        }
        else
        {
            _pitch = resetRot.eulerAngles.x - 360;
        }

        transform.rotation *= Quaternion.FromToRotation(transform.forward.WithY(0), lookDir.WithY(0));
    }

    public void FovKick(float duration, float targetFovCoeff)
    {
        StartCoroutine(FovKickCoroutine(MainCamera, duration, targetFovCoeff));
        StartCoroutine(FovKickCoroutine(WeaponCamera, duration, targetFovCoeff));
    }

    private IEnumerator FovKickCoroutine(Camera cam, float duration, float targetFovCoeff)
    {
        float baseFov = cam.fieldOfView;
        float targetFov = cam.fieldOfView * targetFovCoeff;

        for (float f = 0; f < duration; f += Time.deltaTime)
        {
            cam.fieldOfView = Mathf.Lerp(baseFov, targetFov, FovKickCurve.Evaluate(f / duration));
            yield return null;
        }

        cam.fieldOfView = baseFov;
    }
}
