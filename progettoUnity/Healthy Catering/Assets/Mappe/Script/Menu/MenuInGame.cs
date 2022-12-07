using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using Newtonsoft.Json.Bson;

/// <summary>
/// Classe per la gestione delle impostazioni presenti nel menu in Game.<para>
/// <strong>Da aggiungere a:</strong><br></br>
/// Pannello menu opzioni in game.
/// </para>
/// </summary>
public class MenuInGame : MonoBehaviour
{
    ControllerInput controllerInput;

    [Header("Menu Opzioni")]
    [SerializeField] private GameObject menuPausa;
    [SerializeField] private MenuAiuto menuAiuto;
    [SerializeField] private GameObject primoElementoSelezionato;
    [SerializeField] private TextMeshProUGUI testoEsci;
    [SerializeField] private UnityEvent aperturaMenuGioco;
    [SerializeField] private UnityEvent chiusuraMenuGioco;
    private OpzioniMenu opzioniMenu;

    [Header("Elementi")]
    [SerializeField] private TextMeshProUGUI testoUscita;
    [SerializeField] private Button tastoGraficaAudio;
    [SerializeField] private Button tastoImpostazioniControlli;
    [SerializeField] private GameObject impostazioniControlli;
    [SerializeField] private GameObject impostazioniGraficaAudio;
    [SerializeField] private Button tornaIndietro;
    [SerializeField] private GameObject elementiUscita;
    [SerializeField] private GameObject bottoneNoUscita;

    [SerializeField] private Interactor interazionePlayer;
    private bool giocoInPausa = false;
    private bool menuApribile;
    private bool uscitaAttiva = false;
    public static bool menuAperto = false;

    // Start is called before the first frame update
    void Start()
    {
        attivaElementiIniziali();
    }

    private void OnEnable()
    {
        controllerInput = new ControllerInput();
        controllerInput.UI.Disable();
        controllerInput.Player.Enable();
        controllerInput.Player.Salto.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        checkTastoMenu();
        resetObjectSelezionatoTastoTornaAlMenu();
        if (elementiUscita.activeSelf && EventSystem.current.currentSelectedGameObject == null && menuAperto)
            EventSystem.current.SetSelectedGameObject(bottoneNoUscita);
    }

    private void OnDisable()
    {
        controllerInput.UI.Disable();
        controllerInput.Player.Salto.Enable();
    }

    /// <summary>
    /// Disattiva il controller alla eliminazione dell'oggetto
    /// </summary>
    private void OnDestroy()
    {
        controllerInput.Disable();
    }

    /// <summary>
    /// Il metodo re-imposta la Navigation del tasto per tornare al menu principale in base alla schermata attiva
    /// </summary>
    private void resetObjectSelezionatoTastoTornaAlMenu()
    {
        if (EventSystem.current.currentSelectedGameObject == tornaIndietro.gameObject)
        {
            //Create a new navigation
            Navigation newNav = new Navigation();
            newNav.mode = Navigation.Mode.Explicit;
            if (impostazioniControlli.activeSelf && !impostazioniGraficaAudio.activeSelf)
            {
                newNav.selectOnDown = impostazioniControlli.GetComponentsInChildren<Slider>()[0];
                tornaIndietro.navigation = newNav;
            }
            else if (impostazioniGraficaAudio.activeSelf && !impostazioniControlli.activeSelf)
            {
                newNav.selectOnDown = impostazioniGraficaAudio.GetComponentsInChildren<TMP_Dropdown>()[1];
                tornaIndietro.navigation = newNav;
            }
        }

    }

    /// <summary>
    /// Metodo che attiva gli elementi base del menu opzioni
    /// </summary>
    private void attivaElementiIniziali()
    {
        controllerInput = new ControllerInput();
        controllerInput.Enable();
        giocoInPausa = false;
        uscitaAttiva = false;
        menuApribile = true;
        menuAperto = false;
        menuPausa.SetActive(false);
        testoUscita.gameObject.SetActive(true);
        tastoGraficaAudio.gameObject.SetActive(true);
        tastoImpostazioniControlli.gameObject.SetActive(true);
        impostazioniControlli.SetActive(false);
        impostazioniGraficaAudio.SetActive(true);
        tornaIndietro.gameObject.SetActive(true);
        elementiUscita.SetActive(false);
        opzioniMenu = this.GetComponent<OpzioniMenu>();
    }

    

    /// <summary>
    /// Metodo per controllare se il è stato premuto il tasto per aprire il menu opzioni e verificare che sia apribile in quel momento.
    /// </summary>
    private void checkTastoMenu()
    {
        if(!uscitaAttiva)
        {
            opzioniMenu.setComandiAttivi(!uscitaAttiva);
            if (controllerInput.Player.UscitaMenu.WasPressedThisFrame())
            {
                if (menuApribile)
                {
                    opzioniMenu.clickSuTastoGrafico();
                    if (!Interactor.pannelloAperto)
                    {
                        EventSystem.current.SetSelectedGameObject(primoElementoSelezionato);
                        PlayerSettings.addattamentoSpriteComandi(testoUscita);
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
        } else
        {
            opzioniMenu.setComandiAttivi(!uscitaAttiva);
        }
    }

    /// <summary>
    /// Metodo per ripristinare lo scorrere del tempo in gioco
    /// </summary>
    void resumeGame()
    {
        chiusuraMenuGioco.Invoke();
        //interazionePlayer.playerRiprendiMovimento.Invoke();
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
        //interazionePlayer.playerStop.Invoke();
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
    /// Metodo per impostare che il menu non è apribile
    /// </summary>
    public void menuDisattivo()
    {
        menuApribile = false;
    }

    /// <summary>
    /// Metodo che restituisce la variabile booleana del menu opzioni aperto o meno
    /// </summary>
    /// <returns>Menu aperto<br>True: Menu Opzioni Aperto<br>False: Menu opzioni non aperto</br></br></returns>
    public bool getMenuInGameAperto()
    {
        return menuAperto;
    }

    public void uscitaAperta()
    {
        uscitaAttiva = true;
    }

    public void uscitaChiusa()
    {
        uscitaAttiva = false;
    }
}