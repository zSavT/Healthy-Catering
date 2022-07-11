using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// Classe per la gestione delle impostazioni presenti nel menu in Game.<para>
/// <strong>Da aggiungere a:</strong><br></br>
/// Pannello menu opzioni in game.
/// </para>
/// </summary>
public class MenuInGame : MonoBehaviour
{
    [Header("Menu Opzioni")]
    [SerializeField] private KeyCode tastoMenu;
    [SerializeField] private GameObject menuPausa;
    [SerializeField] private MenuAiuto menuAiuto;
    [SerializeField] private UnityEvent aperturaMenuGioco;
    [SerializeField] private UnityEvent chiusuraMenuGioco;
    [Header("Elementi")]
    [SerializeField] private TextMeshProUGUI testoUscita;
    [SerializeField] private Button tastoGraficaAudio;
    [SerializeField] private Button tastoImpostazioniControlli;
    [SerializeField] private GameObject impostazioniControlli;
    [SerializeField] private GameObject impostazioniGraficaAudio;
    [SerializeField] private Button tornaIndietro;
    [SerializeField] private GameObject elementiUscita;
    private bool giocoInPausa = false;
    private bool menuApribile;

    private bool menuAperto = false;

    // Start is called before the first frame update
    void Start()
    {
        giocoInPausa = false;
        menuApribile = true;
        menuPausa.SetActive(false);
        attivaElementiIniziali();
    }

    // Update is called once per frame
    void Update()
    {
        checkTastoMenu();
    }

    private void attivaElementiIniziali()
    {
        testoUscita.gameObject.SetActive(true);
        tastoGraficaAudio.gameObject.SetActive(true);
        tastoImpostazioniControlli.gameObject.SetActive(true);
        impostazioniControlli.SetActive(false);
        impostazioniGraficaAudio.SetActive(true);
        tornaIndietro.gameObject.SetActive(true);
        elementiUscita.SetActive(false);
    }

    /// <summary>
    /// Metodo per controllare se il � stato premuto il tasto per aprire il menu opzioni e verificare che sia apribile in quel momento.
    /// </summary>
    private void checkTastoMenu()
    {
        if (Input.GetKeyDown(tastoMenu))
        {
            if (menuApribile)
            {
                if (!Interactor.pannelloAperto)
                {
                    if (giocoInPausa)
                    {
                        if (menuAiuto.getPannelloMenuAiutoAperto())
                        {
                            menuAiuto.chiudiPannelloMenuAiuto();
                        }
                        else
                        {
                            menuAperto = false;
                            resumeGame();
                            PuntatoreMouse.disabilitaCursore();
                        }
                    }
                    else
                    {
                        menuAperto = true;
                        pauseGame();
                        PuntatoreMouse.abilitaCursore();
                    }
                }
            }
        }
    }

    /// <summary>
    /// Metodo per ripristinare lo scorrere del tempo in gioco
    /// </summary>
    void resumeGame()
    {
        chiusuraMenuGioco.Invoke();
        menuPausa.SetActive(false);
        Time.timeScale = 1f; //sblocca il tempo
        giocoInPausa = false;
    }

    /// <summary>
    /// Metodo per bloccare lo scorrere del tempo in gioco.
    /// </summary>
    void pauseGame()
    {
        aperturaMenuGioco.Invoke();
        menuPausa.SetActive(true);
        Time.timeScale = 0f; //blocca il tempo
        giocoInPausa = true;
    }

    /// <summary>
    /// Metodo per impostare il menu su apribile vero
    /// </summary>
    public void menuAttivo()
    {
        menuApribile = true;
    }

    /// <summary>
    /// Metodo per impostare che il menu non � apribile
    /// </summary>
    public void menuDisattivo()
    {
        menuApribile = false;
    }

    public bool getMenuInGameAperto()
    {
        return menuAperto;
    }

}