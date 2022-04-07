using System;
using Newtonsoft.Json;

public class Serializza
{
    public Serializza (){
        
    }

    public static void salvaOggettiSuFile <Oggetto> (List <Oggetto> oggetti)
    {
        string pathJson = getJsonPath (oggetti);
        using (StreamWriter file = File.CreateText(@pathJson))
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.Formatting = Formatting.Indented;
            serializer.Serialize(file, oggetti);
        }
        if (oggetti.Count < 0){
            //throw new InvalidOperationException("Lista passata vuota, progressi non salvati (?)");
            Console.WriteLine("Lista passata vuota, sto salvando dei progressi vuoti");
        }
    }

    public static string getJsonPath <Oggetto> (Oggetto oggetto){
        string jsonPath = Directory.GetCurrentDirectory(); 
        jsonPath = jsonPath + @"\..\Database\";
        
        string tipoOggetto = oggetto.GetType().Name;
        if (tipoOggetto.ToLower ().Contains ("list"))
            tipoOggetto = oggetto.GetType().GetGenericArguments().Single().ToString();
        
        jsonPath = jsonPath + tipoOggetto;
        
        jsonPath = jsonPath + ".json";
        return jsonPath;
    }

    public static List<Oggetto> leggiOggettiDaFile <Oggetto> (string filePath)
    {
        if (File.Exists(filePath)){
            string json = File.ReadAllText(@filePath);
            return JsonConvert.DeserializeObject<List<Oggetto>>(json);
        }
        else{
            try{
                Console.WriteLine ("File non trovato, provo a crearlo");
                File.Create (@filePath);
                salvaOggettiSuFile <Oggetto> (new List <Oggetto> ());
                return leggiOggettiDaFile <Oggetto> (@filePath);
            }
            catch (Exception e){
                throw new FileNotFoundException ("File non trovato e non riesco a crearlo, crea un salvataggio di una lista della classe che mi stai passando per leggerla!");
            }
        }
    }
}
