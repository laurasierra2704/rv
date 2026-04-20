using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class playercontroler : MonoBehaviour
{
    //Personaje

    Transform playerTr;
    Rigidbody playerRb;

    public float playerSpeed = 0f;

    private Vector2 newDirection;
    //Camara
    public Transform cameraAxis;
    public Transform cameraTrack;
    private Transform theCamera;

    private float rotY = 0f;
    private float rotX = 0f;

    public float camRotSpeed = 200f;
    public float minAngle = -45f;
    public float maxAngle = 45f;
    public float cameraSpeed = 200f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerTr = this.transform;
        playerRb = GetComponent<Rigidbody>();

        theCamera = Camera.main.transform;


    }

    // Update is called once per frame
    void Update()
    {
        MoveLogic();
        CameraLogic();
    }

    public void MoveLogic()
    {
        Vector3 direction = playerRb.velocity;

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        float theTime = Time.deltaTime;

        newDirection = new Vector2(moveX, moveZ);

        Vector3 side = playerSpeed * moveX * theTime * playerTr.right;
        Vector3 forward = playerSpeed * moveZ * theTime * playerTr.forward;

        Vector3 endDirection = side + forward;

        playerRb.velocity = endDirection;
    }

    public void CameraLogic()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        float theTime = Time.deltaTime;

        rotY += mouseY * camRotSpeed * theTime;
        rotX = mouseX * camRotSpeed * theTime;

        playerTr.Rotate(0,rotX,0); //Para que rote con la camara

        rotY = Mathf.Clamp(rotY, minAngle, maxAngle);
        Quaternion localRotation = Quaternion.Euler(-rotY, 0, 0);
        cameraAxis.localRotation = localRotation;

        theCamera.position = Vector3.Lerp(theCamera.position, cameraTrack.position, cameraSpeed * theTime);
        theCamera.rotation = Quaternion.Lerp(theCamera.rotation, cameraTrack.rotation, cameraSpeed * theTime);

    }
}
