using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class InterazionePassanti : MonoBehaviour
{
    //UNITY
    [SerializeField] private GameObject pannelloInterazionePassanti;
    private bool pannelloInterazionePassantiAperto;
    [SerializeField] private TextMeshProUGUI testoInterazionePassanti;

    //TROVA STRINGHE
    /*
    questa lista di tuple (che si può vedere, più o meno come un dizionario) avra come 
    primo valore (o chiave) la scritta da dare in output e come 
    secondo valore (o valore) la lista degli npc che usano quella stringa

    finche ogni scritta non avrà un npc assegnato ogni lista relativa avrà solo un valore,
    quando tutte le chiavi avranno una lista riempita da almeno 1 valore le liste inizeranno ad avere
    più di un valore all'interno (ovvero saranno assegnate a più npc)
    */
    private List <(string, List <string>)> scrittaENPCsAssegnato = new List<(string, List<string>)> ();
    private int numeroDiScritteAssegnate;
    private int numeroDiScritteTotale;
    private bool ultimoNPCInteragitoNuovo;


    private void Start()
    {
        print(Application.streamingAssetsPath);

        pannelloInterazionePassanti.SetActive(false);
        pannelloInterazionePassantiAperto = false;
        
        getTutteLeScritteInterazione();
    }

    private void Update()
    {
    }

    private void getTutteLeScritteInterazione() {
        string filePath = Path.Combine(Application.streamingAssetsPath, "stringheInterazioniPassanti.txt");

        List<string> tutteLeScritte = shuffleList(File.ReadAllLines(filePath).ToList());

        foreach (string scritta in tutteLeScritte)
        {
            //assegno ad ogni scritta una lista nuova
            scrittaENPCsAssegnato.Add(new(scritta, new List<string>()));
        }

        numeroDiScritteAssegnate = 0;
        numeroDiScritteTotale = tutteLeScritte.Count;
    }

    private List<string> shuffleList (List<string> values)
    {
        System.Random rand = new System.Random();
        values = values.OrderBy(_ => rand.Next()).ToList();
        
        foreach (string val in values)
        {
            print(val);
        }
        
        return values;
    }

    public void apriPannelloInterazionePassanti(string nomeNPC)
    { 
        pannelloInterazionePassanti.SetActive(true);
        testoInterazionePassanti.text = trovaScrittaDaMostrare(nomeNPC);
        aggiornaValoreNumeroScritteAssegnate();
        pannelloInterazionePassantiAperto = true;
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
        print("aggiorna valore:" + numeroDiScritteAssegnate.ToString());
    }

    public bool getPannelloInterazionePassantiAperto()
    {
        return pannelloInterazionePassantiAperto;
    }

    public void chiudiPannelloInterazionePassanti()
    {
        pannelloInterazionePassanti.SetActive(false);
        pannelloInterazionePassantiAperto = false;
    }

    private string trovaScrittaDaMostrare(string nomeNPC)
    {
        //se l'npc è gia presente nel dizionario
        foreach ((string, List<string>) chiaveValore in scrittaENPCsAssegnato)
        {
            if (chiaveValore.Item2.Contains (nomeNPC))
            {
                ultimoNPCInteragitoNuovo = false;
                return chiaveValore.Item1;
            }
        }

        //ora so che l'npc non ha ancora una scritta corrispondente:
        //aggiungo il nome dell'npc alla lista dei nomi degli npc relativi alla scritta
        scrittaENPCsAssegnato[numeroDiScritteAssegnate].Item2.Add(nomeNPC);
        ultimoNPCInteragitoNuovo = true;

        return scrittaENPCsAssegnato[numeroDiScritteAssegnate].Item1;
    }
}
