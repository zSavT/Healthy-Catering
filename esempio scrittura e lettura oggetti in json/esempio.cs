using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class Movie
{
    public string nome { get; set; }
    public int anno { get; set; }

    public override string ToString()
    {
        return base.ToString() + "Il film si chiama: " + nome + "\nEd è uscito nell' anno :" + anno;
    }

    public static void Main(){
        string basePath = Directory.GetCurrentDirectory();
        string jsonFilePath = basePath + @"\movie.json";

        Movie movie = new Movie
        {
            nome = "Bad Boys",
            anno = 1996
        };

        //salvataggio
        string oggetoSerializzato = JsonConvert.SerializeObject(movie, Formatting.Indented);
        File.WriteAllText(@jsonFilePath, oggetoSerializzato);


        //lettura
        using (StreamReader file = File.OpenText(@jsonFilePath))
        using (JsonTextReader reader = new JsonTextReader(file))
        {
            JObject oggettoLetto = (JObject) JToken.ReadFrom(reader); //oggetto di tipo JObject
            Movie filmLetto = oggettoLetto.ToObject <Movie>(); //cast a movie
            Console.WriteLine(filmLetto.nome);
            Console.WriteLine(filmLetto.anno);
            Console.WriteLine(filmLetto.ToString ());
        }
    }
}