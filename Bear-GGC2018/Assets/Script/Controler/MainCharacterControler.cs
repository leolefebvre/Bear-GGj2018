using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacterControler : MonoBehaviour
{
    public float speed = 10.0f;

    [SerializeField] private float _translation = 0f;
    [SerializeField]  private float _strafe = 0f;

	// Use this for initialization
	void Start ()
    {
        // Lock the mouse on the character
        Cursor.lockState = CursorLockMode.Locked;
	}
	
	// Update is called once per frame
	void Update ()
    {
        ManageMovement();

    }

    void ManageMovement ()
    {
        _translation = Input.GetAxis("Vertical") * speed;
        _strafe = Input.GetAxis("Horizontal") * speed;

        _translation *= Time.deltaTime;
        _strafe *= Time.deltaTime;

        transform.Translate(_strafe, 0f, _translation);
    }
}
