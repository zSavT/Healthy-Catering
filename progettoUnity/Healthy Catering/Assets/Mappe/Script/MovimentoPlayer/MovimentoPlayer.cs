using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Classe per la gestione del movimento del giocatore<para>
/// <strong>Da aggiungere a:</strong><br></br>
/// Contenitore del giocatore.
/// </para>
/// </summary>
public class MovimentoPlayer : MonoBehaviour
{
    private CharacterController controller;

    [Header("Movimento")]
    private ModelloPlayer gestoreModelli;
    [SerializeField] private float velocitaBase = 5f;
    [SerializeField] private float velocitaSprint = 8f;
    [SerializeField] private KeyCode tastoSprint;
    private float velocitaAttuale = 0;
    private bool isSprinting;


    [Header("Salto")]
    [SerializeField] private KeyCode tastoSalto;
    [SerializeField] private float altezzaSalto = 0.35f;
    [SerializeField] private float distanzaPavimento = 0.4f;
    [SerializeField] private LayerMask pavimentoMask;
    public bool perTerra;
    private float gravita = -9.8f;
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
    public UnityEvent lockUnlockMovimento;
    private Animator controllerAnimazione;


    // Start is called before the first frame update
    void Start()
    {
        controlloPavimento = GameObject.FindGameObjectWithTag("CheckPavimento").transform;
        controller = GetComponent<CharacterController>();
        puoMuoversi = true;
        gestoreModelli = GetComponent<ModelloPlayer>();
        if (PlayerSettings.caricaGenereModello3D(PlayerSettings.caricaNomePlayerGiocante()) == 1)
            controllerAnimazione = gestoreModelli.getModelloAttivo().GetComponent<Animator>();
        else if (PlayerSettings.caricaGenereModello3D(PlayerSettings.caricaNomePlayerGiocante()) == 0)
            controllerAnimazione = gestoreModelli.getModelloAttivo().GetComponent<Animator>();

    }

    // Update is called once per frame
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
    /// Blocca o sblocca il movimento
    /// </summary>
    public void lockUnlockVisuale()
    {
        this.puoMuoversi = !puoMuoversi;
        lockUnlockMovimento.Invoke();
    }

    /// <summary>
    /// Blocca la possibilità di muoversi del player
    /// </summary>
    public void bloccaMovimento()
    {
        this.puoMuoversi = false;
    }

    /// <summary>
    /// Permette al player di potersi muovere
    /// </summary>
    public void sbloccaMovimento()
    {
        this.puoMuoversi = true;
    }

    /// <summary>
    /// Muove il giocatore
    /// </summary>
    private void movimentoEffettivo()
    {
        controller.Move(movimento * velocitaAttuale * Time.deltaTime);
    }

    /// <summary>
    /// Controlla la pressione dei tasti per il movimento
    /// </summary>
    private void controlloTastiMovimento()
    {
        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");
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
                suonoCamminata.Play();
        }
        else
        {
            suonoCamminata.Stop();
        }
    }

    /// <summary>
    /// Controlla la velocit� dei movimenti.
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
        controller.Move(velocita * Time.deltaTime);
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
        if (Input.GetKey(tastoSprint))
        {
            sprint();
        }
        else if (Input.GetKeyUp(tastoSprint))
        {
            suonoSprint.Stop();
        }
        else
        {
            velocitaAttuale = velocitaBase;
            isSprinting = false;
        }
        if (Input.GetKeyDown(tastoSalto) && perTerra)
        {
            salto();
        }
        if (isFermo())
        {
            idle();
        }
    }


    /// <summary>
    /// Se il giocatore � per terra, pu� sprintare
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
    /// Controlla se il giocotore � fermo o meno.
    /// </summary>
    /// <returns><strong>True</strong>: il giocatore � fermo.<br><strong>False</strong>: il giocatore non � fermo.</br></returns>
    private bool isFermo()
    {
        if (x == 0 && z == 0)
            return true;
        else
            return false;
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