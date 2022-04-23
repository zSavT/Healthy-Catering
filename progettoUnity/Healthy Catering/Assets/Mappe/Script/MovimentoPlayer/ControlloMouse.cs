using UnityEngine;

public class ControlloMouse : MonoBehaviour
{
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
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        this.puoCambiareVisuale = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(puoCambiareVisuale)
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
}
