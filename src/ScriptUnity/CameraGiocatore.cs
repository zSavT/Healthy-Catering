using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraGiocatore : MonoBehaviour
{
    [SerializeField] public float sensX;                     //sensibilità mouse asse x
    [SerializeField] public float sensY;                     //sensibilità mouse asse y

    public Transform orientamento;                          //per l'orientazione del player in game

    float xRotation;
    float yRotation;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        yRotation += mouseX;

        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);                          //permette di muovere la visuale solo di 90 grandi in su e in giù

        //movimento camera
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientamento.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}


