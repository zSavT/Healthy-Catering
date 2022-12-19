using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System.Text.RegularExpressions;


/// <summary>
/// Classe per gestire l'interazione con gli NPC passivi<para>
/// <strong>Da aggiungere a:</strong><br></br>
/// Modello NPC passivo interno (non contenitore)
/// </para>
/// </summary>
public class InterazionePassanti : MonoBehaviour
{
    //UNITY
    [Header("Elementi Interazione Passanti")]
    [SerializeField] private GameObject pannelloInterazionePassanti;
    [SerializeField] private Image immagineComando;
    [SerializeField] private TextMeshProUGUI testoUscita;
    [SerializeField] private Button bottoneAvanti;
    [SerializeField] private TextMeshProUGUI testoInterazionePassanti;
    [Header("Audio interazione")]
    [SerializeField] private AudioSource suonoAperturaDialogo;
    [SerializeField] private AudioSource suonoDialogo;


    private bool pannelloInterazionePassantiAperto;

    //TROVA STRINGHE
    /*
    questa lista di tuple (che si puo' vedere, piu' o meno come un dizionario) avra come 
    primo valore (o chiave) la lista di scritte da dare in output e come 
    secondo valore (o valore) la lista degli npc che usano quella stringa

    finche ogni lista di scritta non avra' un npc assegnato ogni lista relativa avra' solo un valore,
    quando tutte le chiavi avranno una lista riempita da almeno 1 valore le liste inizeranno ad avere
    piu' di un valore all'interno (ovvero saranno assegnate a piu' npc)
    */
    private List <(List <string>, List <string>)> scritteENPCsAssegnato = new List<(List<string>, List<string>)> ();
    private List<string> scritteMostrateOra;
    private int indiceScrittaMostrataOra;

    private int numeroDiScritteAssegnate;
    private int numeroDiScritteTotale;
    
    private bool ultimoNPCInteragitoNuovo;

    public static bool parlatoConNPC = false;
    public static bool parlatoConZio = false;
    private int numeroMassimoDiCaratteriPerSchermata = 90;

    private ControllerInput controllerInput;

    private void Start()
    {
        controllerInput = new ControllerInput();
        controllerInput.Enable();
        pannelloInterazionePassanti.SetActive(false);
        pannelloInterazionePassantiAperto = false;
        
        getTutteLeScritteInterazione();
        parlatoConZio = false;
        parlatoConNPC = false;
        bottoneAvanti.onClick.AddListener(mostraProssimaScrittaDaMostrateOra);
    }

    private void Update()
    {
        if (scritteMostrateOra != null)
        {
            modificaInteractableBottoneInBasePosizioneScrittaMostrata();
        }
        if (pannelloInterazionePassantiAperto)
        {
            addattamentoSpriteComandiInput();
            if (controllerInput.UI.Submit.WasPressedThisFrame() && !controlloNumeroPagine())
                mostraProssimaScrittaDaMostrateOra();
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
    /// Il metodo controlla il numero di pagine rimanenti di testo e disattiva o attiva il bottone per andare avanti.
    /// </summary>
    private void modificaInteractableBottoneInBasePosizioneScrittaMostrata()
    {
        if (controlloNumeroPagine())
        {
            bottoneAvanti.interactable = false;
        }
        else
        {
            bottoneAvanti.interactable = true;
        }
    }

    /// <summary>
    /// Il metodo controlla il numero di pagine mancanti da mostrare
    /// </summary>
    /// <returns>True: non ci sono pagine successive, False: ci sono pagine da mostrare</returns>
    private bool controlloNumeroPagine()
    {
        return indiceScrittaMostrataOra == scritteMostrateOra.Count - 1;
    }

    /// <summary>
    /// Il metodo carica e gestisce tutti i testi dell'interazioni con i passanti in base al livello selezionato.
    /// </summary>
    private void getTutteLeScritteInterazione()
    {
        string filePath = "";
        if(PlayerSettings.livelloSelezionato == 0)
        {
            filePath = Path.Combine(Application.streamingAssetsPath, "stringheInterazioniPassantiLivello0.txt");
        } else if (PlayerSettings.livelloSelezionato == 1)
        {
            filePath = Path.Combine(Application.streamingAssetsPath, "stringheInterazioniPassantiLivello1.txt");
        } else if (PlayerSettings.livelloSelezionato == 2)
        {
            filePath = Path.Combine(Application.streamingAssetsPath, "stringheInterazioniPassantiLivello2.txt");
        }
       
        List<string> tutteLeScritte = shuffleList(File.ReadAllLines(filePath).ToList());

        foreach (string scritta in tutteLeScritte)
        {
            List<string> scrittaDivisa = dividiStringa(scritta);
            //assegno ad ogni scritta divisa una lista nuova
            scritteENPCsAssegnato.Add(new(scrittaDivisa, new List<string>()));
        }

        numeroDiScritteAssegnate = 0;
        numeroDiScritteTotale = tutteLeScritte.Count;
    }

    /// <summary>
    /// Il metodo prende una sequenza, come un elenco, e riorganizza l'ordine degli elementi.
    /// </summary>
    /// <param name="values">List di string da spostare</param>
    /// <returns></returns>
    private List<string> shuffleList(List<string> values)
    {
        System.Random rand = new System.Random();
        values = values.OrderBy(_ => rand.Next()).ToList();

        return values;
    }

    /// <summary>
    /// Il metodo divide la stringa per la visualizzazione in pagine del messaggio
    /// </summary>
    /// <param name="scritta">string scritta da controllare e dividere</param>
    /// <returns>List di string divise</returns>
    private List<string> dividiStringa(string scritta)
    {
        List<string> output = new List<string>();

        string[] scrittaDivisaPerSpazi = scritta.Split(' ');

        string temp = scrittaDivisaPerSpazi[0];

        int i = 1; //perche la prima parola l'ho gia inserita

        while (i < scrittaDivisaPerSpazi.Length)
        {
            if (trovaVeraLunghezzaStringaPerColore (temp) > numeroMassimoDiCaratteriPerSchermata)
            {
                //https://docs.microsoft.com/en-us/dotnet/api/system.string.trim?view=net-6.0
                temp = rimuoviUltimaParola(temp.Trim());
                output.Add(temp.Trim());
                temp = "";
                i--;
            }
            else
            {
                temp += " " + scrittaDivisaPerSpazi[i];
                i++;
            }
        }
        output.Add(temp.Trim());

        return output;
    }

    /// <summary>
    /// Il metodo controlla se nella stringa sono presenti dei tag e non li considera per la lunghezza della stringa
    /// </summary>
    /// <param name="temp">string stringa da controllare</param>
    /// <returns>int lunghezza stringa</returns>
    private int trovaVeraLunghezzaStringaPerColore(string temp)
    {
        if ((temp.Contains(Costanti.coloreInizio)) && (temp.Contains(Costanti.fineColore)))
        {
            int numeroColoreInizio = Regex.Matches(temp, Costanti.coloreInizio).Count;
            int numeroColoreFine = Regex.Matches(temp, Costanti.fineColore).Count;
            if (numeroColoreInizio == numeroColoreFine)
            {
                return temp.Length - (numeroColoreInizio * (Costanti.coloreVerde.Length + Costanti.fineColore.Length));
            }
        }

        return temp.Length;
    }

    /// <summary>
    /// Il metodo rimuove l'ultima parola presente nella stringa
    /// </summary>
    /// <param name="temp">string la stringa da controllare</param>
    /// <returns>string stringa aggiornata senza ultima parola</returns>
    private string rimuoviUltimaParola(string temp)
    {
        if (!temp.Equals(""))
        {
            // https://www.tutorialsrack.com/articles/396/how-to-remove-the-last-word-from-the-string-in-csharp
            if (temp.Contains(" "))
            {
                temp = temp.Substring(0, temp.LastIndexOf(' ')).TrimEnd();
            }
        }

        return temp;
    }

    /// <summary>
    /// Il metodo permette di aggiornare l'indice delle frasi da mostrare e aggiorna il testo visualizzato con quello successivo
    /// </summary>
    private void mostraProssimaScrittaDaMostrateOra()
    {
        indiceScrittaMostrataOra++;
        testoInterazionePassanti.text = scritteMostrateOra[indiceScrittaMostrataOra];
    }

    /// <summary>
    /// Il metodo apre il pannello dell'interazioni con i passanti ed attiva il testo corretto da visualizzare in base al nome del NPC puntato
    /// </summary>
    /// <param name="nomeNPC">string nome del NPC puntato</param>
    public void apriPannelloInterazionePassanti(string nomeNPC)
    {
        suonoAperturaDialogo.Play();
        suonoDialogo.PlayDelayed(0.2f);
        parlatoConNPC = true;
        pannelloInterazionePassanti.SetActive(true);

        scritteMostrateOra = trovaScritteDaMostrare(nomeNPC);
        indiceScrittaMostrataOra = 0;
        testoInterazionePassanti.text = scritteMostrateOra [indiceScrittaMostrataOra];
        aggiornaValoreNumeroScritteAssegnate();

        if (numeroDiScritteAssegnate == 0)
        {
            scritteENPCsAssegnato = new List<(List<string>, List<string>)>();
            getTutteLeScritteInterazione();
        }

        pannelloInterazionePassantiAperto = true;

        if (isNPCzio (nomeNPC))
        {
            parlatoConZio = true;
            parlatoConNPC = false;
        }
        addattamentoSpriteComandiInput();
    }

    /// <summary>
    /// Il metodo permette di addattare gli sprite e le immagine in base all'input
    /// </summary>
    private void addattamentoSpriteComandiInput()
    {
        immagineComando.GetComponent<GestoreTastoUI>().impostaImmagineInBaseInput("X");
        PlayerSettings.addattamentoSpriteComandi(testoUscita);
    }

    /// <summary>
    /// Il metodo assegna il giusto testo da inizializzare in base al nome del NPC
    /// </summary>
    /// <param name="nomeNPC">string nome del NPC</param>
    /// <returns>lista di stringhe contenente il messaggio corretto da visualizzare</returns>
    private List<string> trovaScritteDaMostrare(string nomeNPC)
    {
        if (isNPCzio(nomeNPC))
        {
            return Costanti.scritteZio;
        }
        else if (nomeNPC.Equals("EasterEgg1"))
        {
            return Costanti.easterEgg1Frase;
        } else if (nomeNPC.Equals("EasterEgg2"))
        {
            return Costanti.easterEgg2Frase;
        }
        else 
        {
            //se l'npc e' gia presente nel dizionario
            foreach ((List<string>, List<string>) chiaveValore in scritteENPCsAssegnato)
            {
                if (chiaveValore.Item2.Contains(nomeNPC))
                {
                    ultimoNPCInteragitoNuovo = false;
                    return chiaveValore.Item1;
                }
            }

            //ora so che l'npc non ha ancora una scritta corrispondente:
            //aggiungo il nome dell'npc alla lista dei nomi degli npc relativi alla scritta
            scritteENPCsAssegnato[numeroDiScritteAssegnate].Item2.Add(nomeNPC);
            ultimoNPCInteragitoNuovo = true;

            return scritteENPCsAssegnato[numeroDiScritteAssegnate].Item1;
        }
    }

    /// <summary>
    /// Il metodo controlla se il nome del NPC inquadrato è quello dello zio nel tutorial
    /// </summary>
    /// <param name="nomeNPC">string nome NPC</param>
    /// <returns>bool, True: NPC è zio, False: NPC non zio</returns>
    private bool isNPCzio(string nomeNPC)
    {
        return (nomeNPC.ToLower().Contains("zio") || nomeNPC.ToLower().Contains("tutorial"));
    }

    /// <summary>
    /// Il metodo controlla se npc inquadrato è l'ultimo visualizzato o uno nuovo ed aggiorna i valori di conseguenza.
    /// <para>Il metodo permette di visualizzare al punto rimasto in precedenza nella visualizzazione se il giocatore decide di chiudere il pannello 
    /// interazione con i passanti, prima di leggere tutto il testo presente nelle altre schede</para>
    /// </summary>
    private void aggiornaValoreNumeroScritteAssegnate()
    {
        if (ultimoNPCInteragitoNuovo)
        {
            if (numeroDiScritteAssegnate != numeroDiScritteTotale - 1)
            {
                numeroDiScritteAssegnate++;//aumento l'indice se non sono arrivato all'ultimo valore
            }
            else
            {
                numeroDiScritteAssegnate = 0;//altrimenti lo resetto
            }
        }
    }

    /// <summary>
    /// Il metodo restituisce la variabile pannelloInterazionePassantiAperto per il controllo dello stato di apertura del pannello
    /// </summary>
    /// <returns>bool True: Pannello Aperto, False: Pannello Chiuso</returns>
    public bool getPannelloInterazionePassantiAperto()
    {
        return pannelloInterazionePassantiAperto;
    }

    /// <summary>
    /// Il metodo chiude il pannello di interazione passanti 
    /// </summary>
    public void chiudiPannelloInterazionePassanti()
    {
        pannelloInterazionePassanti.SetActive(false);
        pannelloInterazionePassantiAperto = false;
        scritteMostrateOra = new List<string>();
    }
}
