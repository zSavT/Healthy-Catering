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

    private int ultimaPosizione;

    private Animazione animazione;

    public static bool apertoMenuAiuto = false;//TUTORIAL

    void Start()
    {
        pannelloMenuAiuto.SetActive(false);

        pannelloMenuAiutoAperto = false;

        tastoAvanti.onClick.AddListener(mostraProssimoMessaggioDiAiuto);
        tastoIndietro.onClick.AddListener(mostraPrecedenteMessaggioDiAiuto);

        ultimaPosizione = 0;//cosi la prima volta apre il primo messaggio
        testoNumeroPannelloAttuale.text = "1";
        gestisciBottoniAvantiDietro(ultimaPosizione);

        animazione = immagineSchermata.GetComponent<Animazione>();
    }

    public void apriPannelloMenuAiuto()
    {
        pannelloMenuAiuto.SetActive(true);
        pannelloMenuAiutoAperto = true;

        setMediaPannelloAiuto(ultimaPosizione);

        apertoMenuAiuto = true; //TUTORIAL
    }

    private void setMediaPannelloAiuto(int pagina)
    {
        titoloTestoAiuto.text = Costanti.titoliMessaggiDiAiuto[pagina];
        testoAiuto.text = Costanti.messaggiDiAiuto[pagina];
        cambiaImmagineSchermataAiuto(pagina);

        aggiornaTestoNumeroPannelloAttuale(pagina);

        gestisciBottoniAvantiDietro(pagina);

        ultimaPosizione = pagina;//aggiorno l'ultima posizione
    }

    private void cambiaImmagineSchermataAiuto(int ultimaPosizione)
    {
        animazione.caricaAnimazione(
            "immaginiAiuto",
            Costanti.nomiAnimazioni[ultimaPosizione],
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
        if (posizione == Costanti.messaggiDiAiuto.Count - 1)
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
        if (ultimaPosizione != Costanti.messaggiDiAiuto.Count - 1)
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
