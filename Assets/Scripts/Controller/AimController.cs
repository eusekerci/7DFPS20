using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimController : MonoBehaviour
{
    [SerializeField]
    private Transform _weaponRoot;

    [SerializeField]
    private float _aimDistance;

    [SerializeField]
    private LayerMask _aimLayer;

    [SerializeField]
    private Camera _aimCamera;
    
    [SerializeField]
    private RectTransform _imageTopLeft;

    [SerializeField]
    private RectTransform _imageTopRight;

    [SerializeField]
    private RectTransform _imageBottomLeft;

    [SerializeField]
    private RectTransform _imageBottomRight;

    private Transform _aimedTransform;
    private readonly List<Vector3> _aimedMeshVertices = new List<Vector3>();
    private readonly List<Vector3> _aimedMeshScreenPositions = new List<Vector3>();
    private readonly Vector3[] _crosshairTargetPositions = new Vector3[4];
    private readonly Vector3[] _crosshairDefaultPositions = new Vector3[4];
    
    private bool _isEquipped;
    private Transform _equippedTransform;

    private void Start()
    {
        _crosshairDefaultPositions[0] = _imageTopLeft.position;
        _crosshairDefaultPositions[1] = _imageTopRight.position;
        _crosshairDefaultPositions[2] = _imageBottomLeft.position;
        _crosshairDefaultPositions[3] = _imageBottomRight.position;

        StartCoroutine(SetAimedObject());
        StartCoroutine(MouseInput());
    }

    private IEnumerator SetAimedObject()
    {
        while(true)
        {
            yield return null;
            
            RaycastHit hit;
            Ray ray = _aimCamera.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
            if (Physics.Raycast(ray, out hit, _aimDistance, _aimLayer))
            {
                if (hit.transform == _aimedTransform)
                {
                    continue;
                }

                _aimedTransform = hit.transform;
            }
            else
            {
                _aimedTransform = null;
            }
        }
    }

    private IEnumerator MouseInput()
    {
        while (true)
        {
            if (_aimedTransform != null)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (_isEquipped && _aimedTransform.CompareTag("Putable"))
                    {
                        Unequip();
                    }
                    else if (_aimedTransform.CompareTag("Equipable"))
                    {
                        Equip();
                    }
                }
            }

            yield return null;
        }
    }

    private void Equip()
    {
        _aimedTransform.SetParent(_weaponRoot);
        _aimedTransform.localPosition = Vector3.zero;
        _equippedTransform = _aimedTransform;
        _isEquipped = true;
    }

    private void Unequip()
    {
        _equippedTransform.SetParent(_aimedTransform.Find("HolderRoot"));
        _equippedTransform.parent.GetComponent<HolderFollow>().UpdateOffset(_aimCamera.transform);
        _equippedTransform.localPosition = Vector3.zero;
        _equippedTransform = null;
        _isEquipped = false;
    }
}
