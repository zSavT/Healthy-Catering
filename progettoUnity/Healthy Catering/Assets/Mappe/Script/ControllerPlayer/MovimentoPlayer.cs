using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

/// <summary>
/// Classe per la gestione del movimento del giocatore<para>
/// <strong>Da aggiungere a:</strong><br></br>
/// Contenitore del giocatore.
/// </para>
/// </summary>
public class MovimentoPlayer : MonoBehaviour
{
    private CharacterController characterController;
    private ControllerInput controllerInput;

    [Header("Movimento")]
    private ModelloPlayer gestoreModelli;
    [SerializeField] private float velocitaBase = 5f;
    [SerializeField] private float velocitaSprint = 8f;
    private float velocitaAttuale = 0;
    private bool isSprinting;

    [Header("Salto")]
    [SerializeField] private float altezzaSalto = 0.35f;
    [SerializeField] private float distanzaPavimento = 0.4f;
    [SerializeField] private float gravita = -9.8f;
    [SerializeField] private LayerMask pavimentoMask;
    public bool perTerra;
    private Transform controlloPavimento;           //deve avere il tag "CheckPavimento" inserito

    [Header("Suoni Movienti")]
    [SerializeField] private AudioSource suonoCamminata;
    [SerializeField] private AudioSource suonoSprint;
    [SerializeField] private AudioSource suonoSalto;

    private Vector3 velocita;
    private float x;
    private float z;
    private Vector3 movimento;
    private bool puoMuoversi;
    [SerializeField] private ControlloMouse movimentoVisuale;
    private Animator controllerAnimazione;

    void Start()
    {
        inizializzazioneElementi();
    }

    void Update()
    {
        if (puoMuoversi)
        {
            controlloCollisionePavimento();

            controlloVelocita();

            Physics.SyncTransforms();

            controlloTastiMovimento();

            controlloSuonoMovimento();

            controlloComandi();

            movimentoEffettivo();

            controlloGravita();
        }
    }

    /// <summary>
    /// Disattiva il controller alla eliminazione dell'oggetto
    /// </summary>
    private void OnDestroy()
    {
        controllerInput.Disable();
    }

    /// <summary>
    /// Inizializza i valori iniziali degli oggetti della classe
    /// </summary>
    private void inizializzazioneElementi()
    {
        controllerInput = new ControllerInput();
        controllerInput.Enable();
        controlloPavimento = GameObject.FindGameObjectWithTag("CheckPavimento").transform;
        characterController = GetComponent<CharacterController>();
        puoMuoversi = true;
        gestoreModelli = GetComponent<ModelloPlayer>();
        if (PlayerSettings.caricaGenereModello3D(PlayerSettings.caricaNomePlayerGiocante()) == 1)
            controllerAnimazione = gestoreModelli.getModelloAttivo().GetComponent<Animator>();
        else if (PlayerSettings.caricaGenereModello3D(PlayerSettings.caricaNomePlayerGiocante()) == 0)
            controllerAnimazione = gestoreModelli.getModelloAttivo().GetComponent<Animator>();
    }


    /// <summary>
    /// Blocca la possibilità di muoversi del player
    /// </summary>
    public void bloccaMovimento()
    {
        this.puoMuoversi = false;
        movimentoVisuale.bloccaVisuale();
    }


    /// <summary>
    /// Il metodo setta la variabile booleana puoMuoversi.
    /// </summary>
    /// <param name="movimentoAttivo">booleano</param>
    public void setPuoMuoversi(bool movimentoAttivo)
    {
        puoMuoversi = movimentoAttivo;
    }

    /// <summary>
    /// Permette al player di potersi muovere
    /// </summary>
    public void sbloccaMovimento()
    {
        this.puoMuoversi = true;
        movimentoVisuale.attivaMovimento();
    }

    /// <summary>
    /// Muove il giocatore
    /// </summary>
    private void movimentoEffettivo()
    {
        characterController.Move(movimento * velocitaAttuale * Time.deltaTime);
    }

    /// <summary>
    /// Controlla la pressione dei tasti per il movimento
    /// </summary>
    private void controlloTastiMovimento()
    {
        x = controllerInput.Player.Movimento.ReadValue<Vector2>().x;
        z = controllerInput.Player.Movimento.ReadValue<Vector2>().y;
        movimento = transform.right * x + transform.forward * z;
        if (z > 0)
        {
            controllerAnimazione.SetBool("camminaIndietro", false);
        }
        else if (z < 0)
        {
            controllerAnimazione.SetBool("camminaIndietro", true);
        }
        else if (z == 0)
        {
            controllerAnimazione.SetBool("camminaIndietro", false);
        }
        if (x > 0)
        {
            controllerAnimazione.SetBool("camminaDestra", true);
            controllerAnimazione.SetBool("camminaSinistra", false);
        }
        else if (x < 0)
        {
            controllerAnimazione.SetBool("camminaSinistra", true);
            controllerAnimazione.SetBool("camminaDestra", false);
        }
        else if (x == 0)
        {
            controllerAnimazione.SetBool("camminaSinistra", false);
            controllerAnimazione.SetBool("camminaDestra", false);
        }
        controllerAnimazione.SetBool("fermo", false);
        controllerAnimazione.SetBool("cammina", true);
        controllerAnimazione.SetBool("corre", false);
    }

    /// <summary>
    /// Metodo che controlla la riproduzio dei suoni del movimento del giocatore.
    /// </summary>
    private void controlloSuonoMovimento()
    {
        if (!isFermo() && !isSprinting && Interactor.menuApribile)
        {
            if (!suonoCamminata.isPlaying)
            {
                suonoCamminata.Play();
            }
        }
        else
        {
            suonoCamminata.Stop();
        }
    }

    /// <summary>
    /// Controlla la velocità dei movimenti.
    /// </summary>
    private void controlloVelocita()
    {
        if (perTerra && velocita.y < 0)
        {
            velocita.y = -2f;
        }
    }

    /// <summary>
    /// Sistema ed aggiorna la gravita del gioco.
    /// </summary>
    private void controlloGravita()
    {
        velocita.y += gravita * Time.deltaTime;
        characterController.Move(velocita * Time.deltaTime);
    }

    /// <summary>
    /// Controlla la collisione del giocatore con il pavimento.
    /// </summary>
    private void controlloCollisionePavimento()
    {
        perTerra = Physics.CheckSphere(controlloPavimento.position, distanzaPavimento, pavimentoMask);
        controllerAnimazione.SetBool("salta", false);
    }

    /// <summary>
    /// Controllo degli input.
    /// </summary>
    private void controlloComandi()
    {
        if (controllerInput.Player.Corsa.IsPressed())
        {
            sprint();
        }
        else
        {
            suonoSprint.Stop();
            velocitaAttuale = velocitaBase;
            isSprinting = false;
        }
        if (controllerInput.Player.Salto.WasPressedThisFrame() && perTerra && controllerInput.Player.Salto.enabled)
        {
            Debug.Log(puoMuoversi);
            salto();
        }
        if (isFermo())
        {
            idle();
        }
    }

    /// <summary>
    /// Se il giocatore è per terra, può sprintare
    /// </summary>
    private void sprint()
    {
        if (perTerra)
        {
            velocitaAttuale = velocitaSprint;
        }
        controllerAnimazione.SetBool("fermo", false);
        controllerAnimazione.SetBool("cammina", false);
        controllerAnimazione.SetBool("corre", true);
        isSprinting = true;
        if (!suonoSprint.isPlaying && !isFermo())
            suonoSprint.Play();
    }

    /// <summary>
    /// Avvia il salto del giocatore
    /// </summary>
    private void salto()
    {
        velocita.y = Mathf.Sqrt(altezzaSalto * -2f * gravita);
        controllerAnimazione.SetBool("salta", true);
        if (!suonoSalto.isPlaying && Interactor.menuApribile)
            suonoSalto.Play();
    }


    /// <summary>
    /// Controlla se il giocotore è fermo o meno.
    /// </summary>
    /// <returns><strong>True</strong>: il giocatore è fermo.<br><strong>False</strong>: il giocatore non è fermo.</br></returns>
    private bool isFermo()
    {
        return x == 0 && z == 0;
    }

    /// <summary>
    /// Avvia le animazioni dell'idle.
    /// </summary>
    public void idle()
    {
        controllerAnimazione.SetBool("fermo", true);
        controllerAnimazione.SetBool("cammina", false);
        controllerAnimazione.SetBool("corre", false);
    }

}