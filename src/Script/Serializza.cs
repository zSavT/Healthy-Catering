using System;
using Newtonsoft.Json;

public class Serializza
{
    public static void salvaOggettiSuFile<Oggetto>(List<Oggetto> oggetti)
    {
        if (oggetti.Count < 0)
            throw new InvalidOperationException("Lista passata vuota, progressi non salvati (?)");

        string pathJson = getJsonPath(oggetti);
        using (StreamWriter file = File.CreateText(@pathJson))
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.Formatting = Formatting.Indented;
            serializer.Serialize(file, oggetti);
        }
    }

    public static string getJsonPath<Oggetto>(Oggetto oggetto)
    {
        string jsonPath = Directory.GetCurrentDirectory() + @"\..\Database\";

        string tipoOggetto = ;
        if (tipoOggetto.ToLower().Contains("list")) //se è una lista
            tipoOggetto = getNomeTipoOggettoInLista(oggetto);

        return jsonPath + tipoOggetto + ".json";
    }

    public static string getNomeTipoOggettoInLista<Oggetto>(Oggetto oggetto)
    {
        return oggetto.GetType().GetGenericArguments().Single().ToString();
    }
    public static string getNomeTipo<Oggetto>(Oggetto oggetto)
    {
        return oggetto.GetType().Name;
    }

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
        catch (Exception e)
        {
            throw new FileNotFoundException("File non trovato e non riesco a crearlo, crea un file .json con la stringa \"[]\" per farmi leggere una lista vuota oppure popola il json a mano");
        }

    }
}
