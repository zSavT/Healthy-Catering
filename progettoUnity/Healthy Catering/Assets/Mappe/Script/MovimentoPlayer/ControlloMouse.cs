using UnityEngine;

public class ControlloMouse : MonoBehaviour
{
    private Camera cameraGioco;
    public Transform modelloPlayer;
    [Header("Impostazioni Mouse")]
    public float sensibilitaMouse = 250f;
    public float rangeVisuale = 90f;

    private float xRotation = 0f;
    private float mouseX;
    private float mouseY;
    bool puoCambiareVisuale;

    // Start is called before the first frame update
    void Start()
    {
        if(PlayerPrefs.GetInt("primoAvvio") == 0)
        {
            PlayerPrefs.SetInt("primoAvvio", 1);
            PlayerPrefs.SetFloat("sensibilita", 250f);
        }
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        this.puoCambiareVisuale = true;
        cameraGioco = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        sensibilitaMouse = caricaImpostazioniSensibilita();
        cameraGioco.fieldOfView = caricaImpostazioniFov();
        if (puoCambiareVisuale)
        {
            mouseX = Input.GetAxis("Mouse X") * sensibilitaMouse * Time.deltaTime;
            mouseY = Input.GetAxis("Mouse Y") * sensibilitaMouse * Time.deltaTime;
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -rangeVisuale, rangeVisuale);
            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            modelloPlayer.Rotate(Vector3.up * mouseX);
        }
    }
    public void lockUnlockVisuale()
    {
        this.puoCambiareVisuale = !puoCambiareVisuale;
    }

    private float caricaImpostazioniFov()
    {
        return PlayerPrefs.GetFloat("fov");
    }

    private float caricaImpostazioniSensibilita()
    {
        return PlayerPrefs.GetFloat("sensibilita");
    }




}
