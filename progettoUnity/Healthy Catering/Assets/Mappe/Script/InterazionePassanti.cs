using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class InterazionePassanti : MonoBehaviour
{
    //UNITY
    [SerializeField] private GameObject pannelloInterazionePassanti;
    private bool pannelloInterazionePassantiAperto;
    [SerializeField] private Button bottoneAvanti;
    [SerializeField] private TextMeshProUGUI testoInterazionePassanti;
    [SerializeField] private AudioSource suonoAperturaDialogo;
    [SerializeField] private AudioSource suonoDialogo;

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

    private void Start()
    {
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
    }

    private void modificaInteractableBottoneInBasePosizioneScrittaMostrata()
    {
        if (indiceScrittaMostrataOra == scritteMostrateOra.Count - 1)
        {
            bottoneAvanti.interactable = false;
        }
        else
        {
            bottoneAvanti.interactable = true;
        }
    }

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

    private List<string> shuffleList(List<string> values)
    {
        System.Random rand = new System.Random();
        values = values.OrderBy(_ => rand.Next()).ToList();

        return values;
    }

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

    private void mostraProssimaScrittaDaMostrateOra()
    {
        indiceScrittaMostrataOra++;
        testoInterazionePassanti.text = scritteMostrateOra[indiceScrittaMostrataOra];
    }

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
    }

    private List<string> trovaScritteDaMostrare(string nomeNPC)
    {
        if (!isNPCzio(nomeNPC))
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
        else
        {
            return Costanti.scritteZio;
        }
    }

    private bool isNPCzio(string nomeNPC)
    {
        return (nomeNPC.ToLower().Contains("zio") || nomeNPC.ToLower().Contains("tutorial"));
    }

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

    public bool getPannelloInterazionePassantiAperto()
    {
        return pannelloInterazionePassantiAperto;
    }

    public void chiudiPannelloInterazionePassanti()
    {
        pannelloInterazionePassanti.SetActive(false);
        pannelloInterazionePassantiAperto = false;
        scritteMostrateOra = new List<string>();
    }
}
