using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Serializza
{
    /// <summary>
    /// Il metodo permette di Serializzare su un file json formattato la classe passata in input
    /// </summary>
    /// <typeparam name="Oggetto">Tipologia oggetto passato</typeparam>
    /// <param name="oggetti">List oggetti da serializzare</param>
    /// <exception cref="InvalidOperationException">InvalidOperationException numero oggetti da salvare inesistente</exception>
    public static void salvaOggettiSuFile<Oggetto>(List<Oggetto> oggetti)
    {
        if (oggetti.Count < 0)
            throw new InvalidOperationException("Lista passata vuota, nessuna operazione di salvataggio effettuata.");

        string pathJson = getJsonPath(oggetti);
        using (StreamWriter file = File.CreateText(@pathJson))
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.Formatting = Formatting.Indented;
            serializer.Serialize(file, oggetti);
        }
    }

    /// <summary>
    /// Il metodo permette di ricevere il path dove è presente il file json della classe passata
    /// </summary>
    /// <typeparam name="Oggetto">Tipologia oggetto passato</typeparam>
    /// <param name="oggetto">oggetto da trovare il path</param>
    /// <returns></returns>
    public static string getJsonPath<Oggetto>(Oggetto oggetto){
        string jsonPath = Path.Combine(Application.streamingAssetsPath, "database");

        string tipoOggetto = getNomeTipo (oggetto);
        if (tipoOggetto.ToLower().Contains("list")) //se è una lista
            tipoOggetto = getNomeTipoOggettoInLista(oggetto);

        return Path.Combine(jsonPath, tipoOggetto + ".json");
    }

    /// <summary>
    /// Il metodo restituisce il tipo di oggetto
    /// </summary>
    /// <typeparam name="Oggetto">Tipologia oggetto passato</typeparam>
    /// <param name="oggetto">oggetto da conoscere il tipo</param>
    /// <returns>string tipo oggetto passato</returns>
    private static string getNomeTipoOggettoInLista<Oggetto>(Oggetto oggetto)
    {
        return oggetto.GetType().GetGenericArguments().Single().ToString();
    }

    /// <summary>
    /// Il metodo restituisce il tipo di oggetto
    /// </summary>
    /// <typeparam name="Oggetto">Tipologia oggetto passato</typeparam>
    /// <param name="oggetto">oggetto da conoscere il tipo</param>
    /// <returns>string tipo oggetto passato</returns>
    public static string getNomeTipo<Oggetto>(Oggetto oggetto)
    {
        return oggetto.GetType().Name;
    }

    /// <summary>
    /// Il metodo permette di legge un oggetto da file
    /// </summary>
    /// <typeparam name="Oggetto">Tipologia oggetto passato</typeparam>
    /// <param name="filePath">string del path del file da leggere</param>
    /// <returns>List oggetto letto da file</returns>
    /// <exception cref="FileNotFoundException">Eccezione per file non trovato</exception>
    public static List<Oggetto> leggiOggettiDaFile<Oggetto>(string filePath)
    {
        if (File.Exists(filePath))
            return JsonConvert.DeserializeObject<List<Oggetto>>(File.ReadAllText(@filePath));

        try
        {
            Console.WriteLine("File non trovato, provo a crearlo");
            File.Create(@filePath);
            salvaOggettiSuFile<Oggetto>(new List<Oggetto>());
            return leggiOggettiDaFile<Oggetto>(@filePath);
        }
        catch (Exception)
        {
            throw new FileNotFoundException("File non trovato e non riesco a crearlo, crea un file .json con la stringa \"[]\" per farmi leggere una lista vuota oppure popola il json a mano\n" + filePath);
        }

    }
}
