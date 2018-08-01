using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RTSCamera : MonoBehaviour {
    public enum CameraPreset { TOPDOWN, ISOMETRIC, CUSTOM };
    public enum ControlsPreset { WASD, ARROWS, CUSTOM};
    [SerializeField] private CameraPreset currentCamera = CameraPreset.TOPDOWN;
    [SerializeField] private ControlsPreset currentControls = ControlsPreset.WASD;

    [Tooltip("X = Min, Y = Max. Cannot Be negative")]
    [SerializeField]private Vector2 minMaxZoom = Vector2.zero;
    [SerializeField] private bool rotateCamera = false;
    [SerializeField] private bool edgeScroll = false;
    [SerializeField] private float moveSpeed = 5f;

    //Internal Input Manager, Replace when adding to project//
    public KeyCode camLeft { get; set; }
    public KeyCode camRight { get; set; }
    public KeyCode camUp { get; set; }
    public KeyCode camDown { get; set; }
    private Camera cam;
    private Vector3 forward, right;
    
	// Use this for initialization
	void Start () {
        cam = gameObject.GetComponent<Camera>();
		switch (currentCamera) {
            case (CameraPreset.TOPDOWN):
                transform.rotation = Quaternion.Euler(90, 0, 0);
                break;
            case (CameraPreset.ISOMETRIC):
                transform.rotation = Quaternion.Euler(30, 45, 0);
                break;

            case (CameraPreset.CUSTOM):
                break;
        }
        if (transform.forward == Vector3.down) {
            forward = transform.up;
        } else {
            forward = transform.forward;
        }
        forward.y = 0;
        forward = Vector3.Normalize(forward);
        right = Quaternion.Euler(new Vector3(0, 90, 0)) * forward;

        switch (currentControls) {
            case (ControlsPreset.WASD):
                camLeft = KeyCode.A;
                camRight = KeyCode.D;
                camUp = KeyCode.W;
                camDown = KeyCode.S;
                break;
            case (ControlsPreset.ARROWS):
                camLeft = KeyCode.LeftArrow;
                camRight = KeyCode.RightArrow;
                camUp = KeyCode.UpArrow;
                camDown = KeyCode.DownArrow;
                break;

            case (ControlsPreset.CUSTOM):
                //Fill In with custom controls as per project needs
                break;
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.anyKey) {
            Move();
        }
	}

    void Move() {
        float horizontalAxis = 0;
        float verticalAxis = 0;

        
        if (Input.GetKey(camLeft)) {
            horizontalAxis = -1;
        } else if (Input.GetKey(camRight)) {
            horizontalAxis = 1;
        }

        if (Input.GetKey(camUp)) {
            verticalAxis = 1;
        } else if (Input.GetKey(camDown)){
            verticalAxis = -1;
        }

        Vector3 direction = new Vector3(horizontalAxis, 0, verticalAxis);
        Vector3 rightMovement = right * moveSpeed * Time.deltaTime * horizontalAxis;
        Vector3 upMovement = forward * moveSpeed * Time.deltaTime * verticalAxis;

        Vector3 heading = Vector3.Normalize(rightMovement + upMovement);
        transform.position += heading;
    }
}
