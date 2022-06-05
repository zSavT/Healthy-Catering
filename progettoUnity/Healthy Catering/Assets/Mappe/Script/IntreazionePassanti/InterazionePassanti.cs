using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using UnityEngine.UI;

public class InterazionePassanti : MonoBehaviour
{
    //UNITY
    [SerializeField] private GameObject pannelloInterazionePassanti;
    private bool pannelloInterazionePassantiAperto;
    [SerializeField] private TextMeshProUGUI testoInterazionePassanti;
    [SerializeField] private Button bottoneAvanti;

    //TROVA STRINGHE
    /*
    questa lista di tuple (che si puo' vedere, piu' o meno come un dizionario) avra come 
    primo valore (o chiave) la scritta da dare in output e come 
    secondo valore (o valore) la lista degli npc che usano quella stringa

    finche ogni scritta non avra' un npc assegnato ogni lista relativa avra' solo un valore,
    quando tutte le chiavi avranno una lista riempita da almeno 1 valore le liste inizeranno ad avere
    piu' di un valore all'interno (ovvero saranno assegnate a piu' npc)
    */
    private List <(List <string>, List <string>)> scritteENPCsAssegnato = new List<(List<string>, List<string>)> ();
    private List<string> scritteMostrateOra;
    private int indiceScrittaMostrataOra;
    private int numeroDiScritteAssegnate;
    private int numeroDiScritteTotale;
    private bool ultimoNPCInteragitoNuovo;
    private int numeroMassimoDiCaratteriPerSchermata = 100;

    private void Start()
    {
        pannelloInterazionePassanti.SetActive(false);
        pannelloInterazionePassantiAperto = false;
        
        getTutteLeScritteInterazione();

        bottoneAvanti.onClick.AddListener(mostraProssimoScrittaMostrateOra);
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
        string filePath = Path.Combine(Application.streamingAssetsPath, "stringheInterazioniPassanti.txt");

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
        string temp = "";
        string tempPrecedente = "";
        
        foreach (string parola in scrittaDivisaPerSpazi)
        {
            temp += " " + parola;
            if (temp.Length < numeroMassimoDiCaratteriPerSchermata - 1)
            {
                tempPrecedente = temp;
            }
            else
            {
                output.Add(rimoviPrimoCarattereSeSpazio(tempPrecedente));
                temp = "";
                tempPrecedente = "";
            }
        }

        if (!temp.Equals("")) // se è stata riempita ma non superava il numeroMassimoDiCaratteriPerSchermata
        {
            output.Add(rimoviPrimoCarattereSeSpazio(temp));
        }

        return output;
    }

    private string rimoviPrimoCarattereSeSpazio(string temp)
    {
        if (!temp.Equals(""))
        {
            if (temp[0] == ' ')
            {
                temp = temp.Remove(0, 1);
            }
        }

        return temp;
    }

    private void mostraProssimoScrittaMostrateOra()
    {
        indiceScrittaMostrataOra++;
        testoInterazionePassanti.text = scritteMostrateOra[indiceScrittaMostrataOra];
    }

    public void apriPannelloInterazionePassanti(string nomeNPC)
    { 
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
    }

    private List<string> trovaScritteDaMostrare(string nomeNPC)
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
