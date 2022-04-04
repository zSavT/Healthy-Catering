using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Va aggiunto all'oggetto che contiene la struttura del giocatore
 * 
 */
public class MovimentoPlayer : MonoBehaviour
{
    [Header("Movement")]
    public float velocitaMovimento;

    public float groundDrag;

    [Header("Groud Check")]
    public float altezzaGiocatore;
    public LayerMask isGround;
    bool grounded;

    public Transform orientamento;              //da aggiungere su Unity con l'oggetto l'orientamento 

    float xInput;
    float yInput;

    Vector3 moveDirection;

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, altezzaGiocatore * 0.5f + 0.2f, isGround);
        MyInput();
        controlloVelocita();
        if (grounded)
        {
            rb.drag = groundDrag;
        } else {
            rb.drag = 0;
        }
    }

    private void FixedUpdate()
    {
        Movimento();
    }

    private void MyInput()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");
    }

    private void Movimento()
    {
        moveDirection = orientamento.forward * yInput + orientamento.right * xInput;
        rb.AddForce(moveDirection.normalized * velocitaMovimento * 10f, ForceMode.Force);
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
}
