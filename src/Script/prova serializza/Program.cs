using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class Serializza
{
    public static void salvaOggettiSuFile <Oggetto> (List <Oggetto> oggetti)
    {
        if (oggetti.Count > 0){
            string pathJson = Directory.GetCurrentDirectory() + @"\" + oggetti[0].GetType().Name + ".json";
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
