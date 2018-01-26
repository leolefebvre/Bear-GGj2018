using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float sensivity = 5.0f;
    public float smoothing = 2.0f;

    public Vector2 yAxisClamp = new Vector2(-90f, 90f);

    private Vector2 _mouseLook;
    private Vector2 _smoothV;

    

    private GameObject _character;

	// Use this for initialization
	void Start ()
    {
        _character = this.transform.parent.gameObject;
    }
	
	// Update is called once per frame
	void Update ()
    {
        Vector2 mousePosition = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        _smoothV.x = Mathf.Lerp(_smoothV.x, mousePosition.x, 1f/smoothing);
        _smoothV.y = Mathf.Lerp(_smoothV.y, mousePosition.y, 1f / smoothing);

        // Update mouselook posistion
        _mouseLook += _smoothV;
        _mouseLook.y = Mathf.Clamp(_mouseLook.y, yAxisClamp.x, yAxisClamp.y);

        transform.localRotation = Quaternion.AngleAxis(-_mouseLook.y, Vector3.right);
        _character.transform.localRotation = Quaternion.AngleAxis(_mouseLook.x, _character.transform.up);
    }
}
