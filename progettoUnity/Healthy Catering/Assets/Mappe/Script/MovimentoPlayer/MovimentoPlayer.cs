using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Va aggiunto all'oggetto che contiene la struttura del giocatore
 */
public class MovimentoPlayer : MonoBehaviour
{
    [Header("Movemento")]
    public float velocitaMovimento = 5;
    public float attritoAlSuolo = 8.5f;


    [Header("Salto")]
    public KeyCode tastoSalto = KeyCode.Space;
    public float forzaSalto = 4;
    public float timerSalto = 0.5f;
    public float molltiplicatoreVelocitaSalto = 0.2f;
    bool prontoASaltare;


    [Header("Controllo pavimento")]
    public float altezzaGiocatore = 0;
    public LayerMask isGround;
    bool perTerra;

    public Transform orientamento;              //da aggiungere su Unity con l'oggetto l'orientamento 

    float xInput;
    float yInput;

    Vector3 moveDirection;

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        prontoASaltare = true;
        attritoAlSuolo = 8.5f;
    }

    private void Update()
    {
        controlloInputGiocatore();
        controlloVelocita();
        if (perTerra)
        {
            rb.drag = attritoAlSuolo;
        } else {
            rb.drag = 0;
        }
    }

    private void FixedUpdate()
    {
        Movimento();
        perTerra = Physics.Raycast(transform.position, Vector3.down, altezzaGiocatore * 0.5f + 0.01f, isGround);
    }

    private void controlloInputGiocatore()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");
        if(Input.GetKey(tastoSalto) && prontoASaltare && perTerra)
        {
            prontoASaltare = false;
            salto();
            Invoke(nameof(resetSalto), timerSalto);
        }
    }

    private void Movimento()
    {
        moveDirection = orientamento.forward * yInput + orientamento.right * xInput;
        if (perTerra)
        {
            rb.AddForce(moveDirection.normalized * velocitaMovimento * 10f, ForceMode.Force);
        }
        else if (!perTerra)
        {
            rb.AddForce(moveDirection.normalized * velocitaMovimento * 10f * molltiplicatoreVelocitaSalto, ForceMode.Force);
        }
    }

    private void controlloVelocita()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        if (flatVel.magnitude > velocitaMovimento)
        {
            Vector3 limiteVelocita = flatVel.normalized * velocitaMovimento;
            rb.velocity = new Vector3(limiteVelocita.x, rb.velocity.y, limiteVelocita.z);
        }
    }

    private void salto()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * forzaSalto, ForceMode.Impulse);
    }

    private void resetSalto()
    {
        prontoASaltare = true;
    }
}
