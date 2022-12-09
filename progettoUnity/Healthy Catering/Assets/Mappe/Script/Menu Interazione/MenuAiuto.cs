using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class MenuAiuto : MonoBehaviour
{
    private bool pannelloMenuAiutoAperto;

    [SerializeField] private GameObject pannelloMenuAiuto;
    [SerializeField] private TextMeshProUGUI titoloTestoAiuto;
    [SerializeField] private TextMeshProUGUI testoAiuto;
    [SerializeField] private Button tastoAvanti;
    [SerializeField] private Button tastoIndietro;
    [SerializeField] private TextMeshProUGUI testoNumeroPannelloAttuale;
    [SerializeField] private Image immagineSchermata;
    [SerializeField] private Image comandiAvantiImmagine;
    [SerializeField] private Image comandiIndietroImmagine;
    [SerializeField] private TextMeshProUGUI testoUscita;

    private int ultimaPosizione;

    private Animazione animazione;

    public static bool apertoMenuAiuto = false;//TUTORIAL

    private ControllerInput controllerInput;

    void Start()
    {
        pannelloMenuAiuto.SetActive(false);
        controllerInput = new ControllerInput();
        controllerInput.Enable();
        pannelloMenuAiutoAperto = false;
        tastoAvanti.onClick.AddListener(mostraProssimoMessaggioDiAiuto);
        tastoIndietro.onClick.AddListener(mostraPrecedenteMessaggioDiAiuto);

        ultimaPosizione = 0;//cosi la prima volta apre il primo messaggio
        testoNumeroPannelloAttuale.text = "1";
        gestisciBottoniAvantiDietro(ultimaPosizione);

        animazione = immagineSchermata.GetComponent<Animazione>();
    }

    private void Update()
    {
        if (pannelloMenuAiutoAperto)
            if (controllerInput.UI.Avanti.WasPressedThisFrame())
                mostraProssimoMessaggioDiAiuto();
            else if (controllerInput.UI.Indietro.WasPressedThisFrame())
                mostraPrecedenteMessaggioDiAiuto();
    }

    /// <summary>
    /// Disattiva il controller alla eliminazione dell'oggetto
    /// </summary>
    private void OnDestroy()
    {
        controllerInput.Disable();
    }

    public void apriPannelloMenuAiuto()
    {
        PlayerSettings.addattamentoSpriteComandi(testoAiuto);
        pannelloMenuAiuto.SetActive(true);
        pannelloMenuAiutoAperto = true;

        setMediaPannelloAiuto(ultimaPosizione);

        apertoMenuAiuto = true; //TUTORIAL
        PlayerSettings.addattamentoSpriteComandi(testoUscita);
        comandiIndietroImmagine.GetComponent<GestoreTastoUI>().impostaImmagineInBaseInput("L1");
        comandiAvantiImmagine.GetComponent<GestoreTastoUI>().impostaImmagineInBaseInput("R1");
    }

    private void setMediaPannelloAiuto(int pagina)
    {
        titoloTestoAiuto.text = Costanti.titoliMenuAiuto[pagina];
        testoAiuto.text = Costanti.testiMenuAiuto[pagina];
        cambiaImmagineSchermataAiuto(pagina);

        aggiornaTestoNumeroPannelloAttuale(pagina);

        gestisciBottoniAvantiDietro(pagina);

        ultimaPosizione = pagina;//aggiorno l'ultima posizione
    }

    private void cambiaImmagineSchermataAiuto(int ultimaPosizione)
    {
        animazione.caricaAnimazione(
            "immaginiAiuto",
            Costanti.nomiAnimazioniMenuAiuto[ultimaPosizione],
            "default"
        );
    }

    private void aggiornaTestoNumeroPannelloAttuale(int ultimaPosizione)
    {
        testoNumeroPannelloAttuale.text = (ultimaPosizione + 1).ToString();
    }

    private void gestisciBottoniAvantiDietro(int posizione)
    {
        //tasto avanti
        if (posizione == Costanti.testiMenuAiuto.Count - 1)
        {
            tastoAvanti.interactable = false;
        }
        else
        {
            tastoAvanti.interactable = true;
        }

        //tasto indietro
        if (posizione == 0)
        {
            tastoIndietro.interactable = false;
        }
        else
        {
            tastoIndietro.interactable = true;
        }
    }

    public void chiudiPannelloMenuAiuto()
    {
        pannelloMenuAiuto.SetActive(false);
        pannelloMenuAiutoAperto = false;
    }

    public bool getPannelloMenuAiutoAperto()
    {
        return pannelloMenuAiutoAperto;
    }

    void mostraProssimoMessaggioDiAiuto()
    {
        int prossimaPosizione = ultimaPosizione;
        if (ultimaPosizione != Costanti.testiMenuAiuto.Count - 1)
        {
            prossimaPosizione = ultimaPosizione + 1;
        }
        setMediaPannelloAiuto(prossimaPosizione);
    }

    private void mostraPrecedenteMessaggioDiAiuto()
    {
        int precedentePosizione = ultimaPosizione;
        if (ultimaPosizione != 0)
        {
            precedentePosizione = ultimaPosizione - 1;
        }

        setMediaPannelloAiuto(precedentePosizione);
    }
}
