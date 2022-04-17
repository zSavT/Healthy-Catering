using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *   Questo script va inserito nell'oggetto camera.
 */

public class CameraGiocatore : MonoBehaviour
{
    [Header("Sensibilità Mouse")]
    [SerializeField] public float sensX;                     //sensibilità mouse asse x
    [SerializeField] public float sensY;                     //sensibilità mouse asse y

    [Header("Campo visuale")]
    public float campoVisualeNegativo = -30f;
    public float campoVisualePositivo = 30f;

    public Transform orientamento;                          //per l'orientazione del player in game (Va aggiunto al collegamento dell'oggetto orientamento in Unity)

    float xRotation;
    float yRotation;

    bool puoCambiareVisuale;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        this.puoCambiareVisuale = true;
    }

    private void Update()
    {
        if (puoCambiareVisuale)
        {
            float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
            float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;
            yRotation += mouseX;
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, campoVisualeNegativo, campoVisualePositivo);                          //permette di muovere la visuale solo di 90 grandi in su e in gi�

            //movimento camera
            transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
            orientamento.rotation = Quaternion.Euler(0, yRotation, 0);
        }
    }

    public void lockUnlockVisuale()
    {
        this.puoCambiareVisuale = !puoCambiareVisuale;
    }
}


