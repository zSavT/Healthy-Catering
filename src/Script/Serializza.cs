using System;
using Newtonsoft.Json;

public class Serializza
{
    public Serializza (){
        
    }

    /*
    public static void Main(string[] args)
    {
        test da rimuovere
        List <Piatto> var = Serializza.leggiOggettiDaFile<Piatto> (@"C:\Users\alex1\Desktop\Healthy-Catering\src\Database\Piatto.json");
        Console.WriteLine (var[0].nome);
        foreach (OggettoQuantita<int> i in var [0].ingredienti){
            Console.WriteLine (i.oggetto); //poi ci sarebbe da chiamare la funzione da idIngrediente a ingrediente ma vabz 
        }
        
    }
    */

    public static void salvaOggettiSuFile <Oggetto> (List <Oggetto> oggetti)
    {
        if (oggetti.Count > 0){
            string pathJson = getJsonPath (oggetti [0]);
            using (StreamWriter file = File.CreateText(@pathJson))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Formatting = Formatting.Indented;
                serializer.Serialize(file, oggetti);
            }
        }
        else{
            throw new InvalidOperationException("Lista passata vuota, progressi non salvati (?)");
        }

        /*
        Esempio di utilizzo:
            List <Dieta> diete = new List<Dieta> ();
            diete.Add (new Dieta ("1"));
            diete.Add (new Dieta ("2"));
            try{
                Serializza.salvaOggettiSuFile<Dieta> (diete);
            }
            catch (InvalidOperationException e){
                Console.WriteLine (e.Message);
            }
        */
    }

    public static string getJsonPath <Oggetto> (Oggetto oggetto){
        string jsonPath = Directory.GetCurrentDirectory(); 
        jsonPath = jsonPath + @"\..\Database\";
        jsonPath = jsonPath + oggetto.GetType().Name; 
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
            throw new FileNotFoundException ("File non trovato, salva un salvataggio di una lista della classe che mi stai passando per leggerla!");
        }

        /*
        Esempio di utilizzo:
            try{
                List <Dieta> idk = Serializza.leggiOggettiDaFile<Dieta> (@"C:\Users\alex1\Desktop\Healthy-Catering\progetto\Healthy Catering\Assets\Script\prova serializza\Dieta.json");
                foreach (var dieta in idk){
                    Console.WriteLine (dieta);
                }
            }
            catch (FileNotFoundException e){
                Console.WriteLine (e.Message);
            }
        */
    }

    /*
    La classe Cliente utilizzata negli esempi:
        public class Dieta
        {
            public string nome = "";

            public Dieta(string nome)
            {
                this.nome = nome;
            }

            public override string ToString()
            {
                return nome;
            }
        }
    */
}
