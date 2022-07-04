using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Interactable : MonoBehaviour
{
    public int IDCliente;
    private Animator controllerAnimazione;
    [SerializeField] private ParticleSystem effettoPositivo;
    [SerializeField] private ParticleSystem effettoNegativo;
    private GameObject contenitoreCliente;
    private GameObject modelloCliente3D;           //assicurarsi che il modello 3d sia il primo figlio del contenitore
    [SerializeField] private GestoreClienti gestioneCliente;
    [SerializeField] private AudioSource suonoContento;


    //Controller della mappa percorribile degli NPC
    NavMeshAgent agent;
    //Waypoint percorso degli NPC
    public Transform[] waypoints;
    //Indice per la gestione dei waypoint raggiunti
    int waypointIndex;
    //Vettore per calcolare la distanza tra il waypoint ed NPC
    Vector3 target;

    public bool raggiuntoBancone = false;
    public bool servito = false;
    public static int numeroCliente = 0;
    private bool distruggi = false;

    void Start()
    {
        contenitoreCliente = this.gameObject;
        modelloCliente3D = contenitoreCliente.transform.GetChild(0).gameObject;
        SetMaterialTransparent();

        StartCoroutine(attendi(2f));

        
        controllerAnimazione = GetComponentInChildren<Animator>();
        effettoPositivo.Stop();
        effettoNegativo.Stop();

        //Inizializza il controller
        agent = GetComponent<NavMeshAgent>();
        
    }

    // Update is called once per frame
    void Update()
    {
        //Controllo della distanza minima per considerare il waypoint raggiunto, in caso positivo si
        if (Vector3.Distance(transform.position, target) < 0.5f)
        {
            if(waypoints.Length -2 == waypointIndex)
            {
                raggiuntoBancone = true;
                animazioneIdle();
                if (servito == true)
                {
                    raggiuntoBancone = false;
                    if (controllerAnimazione.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !controllerAnimazione.IsInTransition(0))
                    {
                        iterazioneIndex();
                        updateDestinazione();
                        raggiuntoBancone = false;
                        animazioneCamminata();
                    }
                }
            } else
            {
                SetMaterialTransparent();
                iTween.FadeTo(contenitoreCliente, 0, 1);
                StartCoroutine(attendiEDistruggi(2f));
            }
        }
    }


    /// <summary>
    /// Distruggi NPC con effetto trasparenza.
    /// </summary>
    /// <param name="attesa">Durata attesa</param>
    /// <returns></returns>
    IEnumerator attendiEDistruggi(float attesa)
    {
        
        yield return new WaitForSecondsRealtime(attesa);
        if(distruggi==false)
        {
            gestioneCliente.attivaClienteSuccessivo();
            distruggi = true;
        } 
        Destroy(contenitoreCliente);
    }

    /// <summary>
    /// Attiva modello NPC con fade trasparenza ed avvia l'animazione camminata.
    /// </summary>
    /// <param name="attesa">Durata attesa</param>
    /// <returns></returns>
    IEnumerator attendi(float attesa)
    {
        iTween.FadeTo(contenitoreCliente, 1, attesa);
        yield return new WaitForSecondsRealtime(attesa);
        SetMaterialOpaque();
        animazioneCamminata();
        updateDestinazione();
    }


    /// <summary>
    /// Imposta il materiale dell'oggetto su transparente.
    /// </summary>
    private void SetMaterialTransparent()

    {

        foreach (Material m in modelloCliente3D.GetComponent<Renderer>().materials)

        {

            m.SetFloat("_Mode", 2);

            m.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);

            m.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);

            m.SetInt("_ZWrite", 0);

            m.DisableKeyword("_ALPHATEST_ON");

            m.EnableKeyword("_ALPHABLEND_ON");

            m.DisableKeyword("_ALPHAPREMULTIPLY_ON");

            m.renderQueue = 3000;

        }

    }



    /// <summary>
    /// Imposta il material dell'oggetto su Opaque.
    /// </summary>
    private void SetMaterialOpaque()

    {

        foreach (Material m in modelloCliente3D.GetComponent<Renderer>().materials)

        {

            m.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);

            m.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);

            m.SetInt("_ZWrite", 1);

            m.DisableKeyword("_ALPHATEST_ON");

            m.DisableKeyword("_ALPHABLEND_ON");

            m.DisableKeyword("_ALPHAPREMULTIPLY_ON");

            m.renderQueue = -1;

        }

    }

    /// <summary>
    /// Avvia animazione Contenta del cliente dopo piatto servito corretto.
    /// </summary>
    public void animazioneContenta()
    {
        controllerAnimazione.SetBool("servito", true);
        controllerAnimazione.SetBool("affinitaPatologiePiatto", true);
        controllerAnimazione.SetBool("affinitaDietaPiatto", true);
        effettoPositivo.Play();
        servito = true;
        suonoContento.Play();
    }


    /// <summary>
    /// Avvia animazione Scontenta del cliente dopo piatto servito non corretto.
    /// </summary>
    public void animazioneScontenta()
    {
        controllerAnimazione.SetBool("servito", true);
        controllerAnimazione.SetBool("affinitaPatologiePiatto", false);
        controllerAnimazione.SetBool("affinitaDietaPiatto", false);
        effettoNegativo.Play();
        servito = true;
    }


    /// <summary>
    /// Avvia animazione idle per il cliente.
    /// </summary>
    public void animazioneIdle()
    {
        controllerAnimazione.SetBool("finito", false);
    }


    /// <summary>
    /// Avvia animazione camminata per il cliente.
    /// </summary>
    public void animazioneCamminata()
    {
        controllerAnimazione.SetBool("finito", true);
    }

    /// <summary>
    /// Imposta la destinazione del WayPoint Successiva.
    /// </summary>
    private void updateDestinazione()
    {
        target = waypoints[waypointIndex].position;
        agent.SetDestination(target);
    }

    /// <summary>
    /// Reset dell'indice dei waypoint da raggiungere.<br></br>
    /// Permette di far camminare all'infinito gli NPC una volta che hanno raggiunto tutti i waypoint facendoli ri-percorre tutti i waypoint.
    /// </summary>
    private void iterazioneIndex()
    {
        waypointIndex++;
        /*  Se si vuole far percorrere il percorso all'infinito, eliminare il commento
        if (waypointIndex == waypoints.Length)
        {
            waypointIndex = 0;
        }
        */
    }
}


