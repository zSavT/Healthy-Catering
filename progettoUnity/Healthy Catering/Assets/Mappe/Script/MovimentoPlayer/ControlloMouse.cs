using UnityEngine;

/// <summary>
/// Classe per la gestione della camera del gioco<para>
/// <strong>Da aggiungere a:</strong><br></br>
/// Camera principale del gioco.
/// </para>
/// </summary>
public class ControlloMouse : MonoBehaviour
{
    [Header("Impostazioni Camera")]
    [SerializeField] private Transform modelloPlayer;
    [SerializeField] private Transform posizioneCameraIniziale;
    [SerializeField] private float posizioneCameraFovMassimo;
    private Camera cameraGioco;
    [Header("Impostazioni Mouse")]
    [SerializeField] private float sensibilitaMouse = 250f;
    [SerializeField] private float rangeVisuale = 90f;

    private float xRotation = 0f;
    private float mouseX;
    private float mouseY;
    bool puoCambiareVisuale;
    private float posizioneZcamera;


    // Start is called before the first frame update
    void Start()
    {
        posizioneZcamera = posizioneCameraIniziale.transform.position.z;
        if (PlayerSettings.caricaPrimoAvvioSettaggiSensibilita() == 0)
        {
            PlayerSettings.salvaPrimoAvvioSettaggiSensibilita();
            PlayerSettings.salvaImpostazioniSensibilita(250f);
        }
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        this.puoCambiareVisuale = true;
        cameraGioco = GetComponent<Camera>();
        aggiornamentoFovInGame();
    }

    // Update is called once per frame
    void Update()
    {
        sensibilitaMouse = PlayerSettings.caricaImpostazioniSensibilita();
        if (puoCambiareVisuale)
        {
            mouseX = Input.GetAxis("Mouse X") * sensibilitaMouse * Time.deltaTime;
            mouseY = Input.GetAxis("Mouse Y") * sensibilitaMouse * Time.deltaTime;
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -rangeVisuale, rangeVisuale);
            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            modelloPlayer.Rotate(Vector3.up * mouseX);
        }
        if(!Interactor.pannelloAperto)
        {
            aggiornamentoFovInGame();
        }
    }


    /// <summary>
    /// Aggiorna il valore de fov
    /// </summary>
    public void aggiornamentoFovInGame()
    {
        cameraGioco.fieldOfView = PlayerSettings.caricaImpostazioniFov();
        posizioneCameraIniziale.localPosition = new Vector3(posizioneCameraIniziale.localPosition.x, posizioneCameraIniziale.localPosition.y, calcoloPosizioneCameraFovAsseZ(PlayerSettings.caricaImpostazioniFov()));
        cameraGioco.transform.position = posizioneCameraIniziale.transform.position;
    }


    /// <summary>
    /// Metodo che calcola la posizione della camera in base al fov.
    /// </summary>
    /// <param name="valoreFov">Valore del fov</param>
    /// <returns>Valore posizione della camera</returns>
    private float calcoloPosizioneCameraFovAsseZ(float valoreFov)
    {   
        return (posizioneZcamera + ((valoreFov - 60)/40) * (posizioneCameraFovMassimo - posizioneZcamera));
    }

    /// <summary>
    /// Blocca il movimento della visuale
    /// </summary>
    public void bloccaVisuale()
    {
        this.puoCambiareVisuale = false;
    }



    /// <summary>
    /// Sblocca o blocca la visuale del giocatore.
    /// </summary>
    public void lockUnlockVisuale()
    {
        this.puoCambiareVisuale = !puoCambiareVisuale;
    }

}
