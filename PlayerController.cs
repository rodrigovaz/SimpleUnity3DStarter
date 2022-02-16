using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    CharacterController characterController;
    public float MovementSpeed =1;
    public float Gravity = 9.8f;
    private float velocity = 0;
 
    // horizontal rotation speed
    public float horizontalSpeed = 1f;
    // vertical rotation speed
    public float verticalSpeed = 1f;
    private float xRotation = 0.0f;
    private float yRotation = 0.0f;

    public GameObject buttonPrefab;
    public GameObject uiCanvas;

    public GameObject bulletPrefab;
    public Text spottedObjectName;
    private Camera cam;

    public GameObject enemiesSpawner;

    private bool isKeyInInventory;
    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        cam = Camera.main;
        isKeyInInventory = false;
    }
 
    void Update()
    {
        RaycastHit pickItemRay;
        spottedObjectName.text = "";

        //cast a ray to be aware of every close GameObject that the player might be pointing to
        if (Physics.Raycast(cam.transform.position, cam.transform.TransformDirection(Vector3.forward), out pickItemRay, 8.0f))
        {
            spottedObjectName.text = pickItemRay.collider.gameObject.tag;
        }

        if (Input.GetButtonDown("Interact"))
        {
            //in this example, item is always the "key"
            if(spottedObjectName.text == "item") {
                GameObject newButton = Instantiate(buttonPrefab) as GameObject;
                newButton.transform.SetParent(uiCanvas.transform, false);
                Destroy(pickItemRay.collider.gameObject);
                isKeyInInventory = true;
            } else if(spottedObjectName.text == "door" && isKeyInInventory) {
                Destroy(pickItemRay.collider.gameObject);
            }
        }

        float mouseX = Input.GetAxis("Mouse X") * horizontalSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * verticalSpeed;
 
        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        transform.localRotation = Quaternion.Euler(0.0f, yRotation, 0f);
        cam.transform.localRotation = Quaternion.Euler(xRotation, 0.0f, 0.0f);

        if(Input.GetButtonDown("Fire1")) {

            RaycastHit shoot;

            //this is a simple shooting strategy, but there is no particle effect to inform that it happened
            if (Physics.Raycast(cam.transform.position, cam.transform.TransformDirection(Vector3.forward), out shoot, 1000.0f))
            {
                if(shoot.collider.gameObject.tag == "enemy") {
                    Destroy(shoot.collider.gameObject);

                    Instantiate(shoot.collider.gameObject.GetComponent<SimpleEnemyAI>().keyPrefab, shoot.point, shoot.collider.gameObject.transform.rotation);

                    enemiesSpawner.GetComponent<SpawnEnemies>().nEnemies--;
                }
            }
        }

        float horizontal = Input.GetAxis("Horizontal") * MovementSpeed;
        float vertical = Input.GetAxis("Vertical") * MovementSpeed;

        if (Input.GetButtonDown("Jump") && characterController.isGrounded)
        {
            velocity = 10.0f;
        }

        velocity -= Gravity * Time.deltaTime;
        characterController.Move((transform.right * horizontal + transform.forward * vertical + new Vector3(0, velocity, 0)) * Time.deltaTime);
    }
}
