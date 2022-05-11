using UnityEngine;
using UnityEngine.Events;

public class MovimentoPlayer : MonoBehaviour
{
    [SerializeField] private CharacterController controller;
    
    [Header("Movimento")]
    [SerializeField] private float velocitaBase = 5f;
    [SerializeField] private float velocitaSprint = 8f;
    [SerializeField] private KeyCode tastoSprint;
    private float velocitaAttuale = 0;
    

    [Header("Salto")]
    [SerializeField] private KeyCode tastoSalto;
    [SerializeField] private float altezzaSalto = 0.35f;
    [SerializeField] private Transform controlloPavimento;
    [SerializeField] private float distanzaPavimento = 0.4f;
    [SerializeField] private LayerMask pavimentoMask;
    [SerializeField] private float gravita = -9.8f;
    private bool perTerra;


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
        puoMuoversi = true;
        controllerAnimazione = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(puoMuoversi)
        {
            controlloCollisionePavimento();

            controlloVelocita();

            Physics.SyncTransforms();

            controlloTastiMovimento();

            controlloComandi();

            movimentoEffettivo();

            controlloGravita();
        }
    }

    public void lockUnlockVisuale()
    {
        this.puoMuoversi = !puoMuoversi;
        lockUnlockMovimento.Invoke();
    }

    private void movimentoEffettivo()
    {
        controller.Move(movimento * velocitaAttuale * Time.deltaTime);
    }

    private void controlloTastiMovimento()
    {
        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");
        movimento = transform.right * x + transform.forward * z;
        if(z > 0 )
        {
            controllerAnimazione.SetBool("camminaIndietro", false);
        } else if (z < 0)
        {
            controllerAnimazione.SetBool("camminaIndietro", true);
        } else if (z == 0)
        {
            controllerAnimazione.SetBool("camminaIndietro", false);
        }
        if (x > 0)
        {
            controllerAnimazione.SetBool("camminaDestra", true);
            controllerAnimazione.SetBool("camminaSinistra", false);
        } else if (x < 0)
        {
            controllerAnimazione.SetBool("camminaSinistra", true);
            controllerAnimazione.SetBool("camminaDestra", false);
        } else if (x == 0)
        {
            controllerAnimazione.SetBool("camminaSinistra", false);
            controllerAnimazione.SetBool("camminaDestra", false);
        }
        controllerAnimazione.SetBool("fermo", false);
        controllerAnimazione.SetBool("cammina", true);
        controllerAnimazione.SetBool("corre", false);
    }

    private void controlloVelocita()
    {
        if (perTerra && velocita.y < 0)
        {
            velocita.y = -2f;
        }
    }

    private void controlloGravita()
    {
        velocita.y += gravita * Time.deltaTime;
        controller.Move(velocita * Time.deltaTime);
    }

    private void controlloCollisionePavimento()
    {
        perTerra = Physics.CheckSphere(controlloPavimento.position, distanzaPavimento, pavimentoMask);
        controllerAnimazione.SetBool("salta", false);
    }

    private void controlloComandi()
    {
        if (Input.GetKey(tastoSprint))
        {
            sprint();
        }
        else
        {
            velocitaAttuale = velocitaBase;
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


    private void sprint()
    {
        if(perTerra)
        {
            velocitaAttuale = velocitaSprint;
        }
        controllerAnimazione.SetBool("fermo", false);
        controllerAnimazione.SetBool("cammina", false);
        controllerAnimazione.SetBool("corre", true);
    }

    private void salto()
    {
        velocita.y = Mathf.Sqrt(altezzaSalto * -2f * gravita);
        controllerAnimazione.SetBool("salta", true);
    }

    private bool isFermo()
    {
        if (x == 0 && z == 0)
            return true;
        else
            return false;
    }

    private void idle()
    {
        controllerAnimazione.SetBool("fermo", true);
        controllerAnimazione.SetBool("cammina", false);
        controllerAnimazione.SetBool("corre", false);
    }

}
