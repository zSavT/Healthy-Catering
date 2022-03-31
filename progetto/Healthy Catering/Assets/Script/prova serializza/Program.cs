using System.Globalization;
using System.Data.Common;
using System.Xml;
using System.Reflection;
using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Script;

public class Serializza
{
    static string pathJson = Directory.GetCurrentDirectory();
    
    public static void Main()
    {
        Serializza.salvaOggettoSuFile(new Dieta());
    }

    public Serializza (string className){
        pathJson = pathJson + @"\" + className + ".json";
    }

    public void salvaOggettoSuFile <Oggetto> (Oggetto oggetto)
    {
        string oggettoSerializzato = JsonConvert.SerializeObject(oggetto, Newtonsoft.Json.Formatting.Indented);
        File.WriteAllText(@pathJson, oggettoSerializzato);

        /*
        Esempio di utilizzo:
            Serializza tempSerializza = new Serializza ("Cliente");
            
            Cliente tempCliente = new Cliente
            {
                nome = "Bad Boys",
                dieta = 1996
            };

            tempSerializza.salvaOggettoSuFile <Cliente> (tempCliente);        
        */
    }

    public Oggetto leggiOggettoDaFile <Oggetto> (string filePath)
    {
        using (StreamReader file = File.OpenText(@filePath))
        using (JsonTextReader reader = new JsonTextReader(file))
        {
            JObject oggettoLetto = (JObject) JToken.ReadFrom(reader); //oggetto di tipo JObject
            return oggettoLetto.ToObject <Oggetto>();
        }

        /*
        Esempio di utilizzo:
            string path = @"C:\Users\alex1\Desktop\Healthy-Catering\progetto\Healthy Catering\Assets\C# Script\prova serializza\Cliente.json";
            
            Serializza tempSerializza = new Serializza ("Cliente");
            
            Cliente tempCliente = tempSerializza.leggiOggettoDaFile <Cliente> (path);

            Console.WriteLine (tempCliente.ToString ());
        */
    }
}

/*
La classe Cliente utilizzata negli esempi:
    public class Cliente
    {
        public string nome = "";
        
        public int dieta = 0;

        public override string ToString()
        {
            return nome + "\n" + dieta;
        }
    }
*/