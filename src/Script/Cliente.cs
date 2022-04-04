public class Cliente
{
    public string nome = "";
    
    public int dieta = 0;
    public List <int> listaIdPatologie = null;

    //costruttore
    public Cliente(string nome, int dieta, List<int> listaIdPatologie)
    {
        this.nome = nome;
        this.dieta = dieta;
        this.listaIdPatologie = listaIdPatologie;
    }

    public int dietaStringToIdDieta (string dieta){
        if (dieta.ToLower () == "vegana")
            return 0;
        else if (dieta.ToLower () == "vegetariana")
            return 1;
        else if (dieta.ToLower () == "onnivora")
            return 2;
        else
            throw new InvalidOperationException ("Dieta inserita non valida");
    } 

    public void getNewNomeClienteFromUtente (){
        Console.WriteLine ("Inserisci il nome del cliente");
        this.nome = Console.ReadLine();
    }

    public void getNewDietaClienteFromUtente (){
        string dietaTemp;
        Console.WriteLine ("Inserisci il nome della dieta del cliente");
        dietaTemp = Console.ReadLine();
        try{
            this.dieta = this.dietaStringToIdDieta(dietaTemp);
        }
        catch (InvalidOperationException e){
            Console.WriteLine (e.Message);
        }
    }

    public void getNewListaIdPatologieFromUtente (){
        List <string> patologieInput = fillListaPatologieStringhe ();
        //returna una lista vuota di interi (id) se la lista data dall'utente è vuota
        if (patologieInput.Count == 0){
            this.listaIdPatologie = new List <int> ();
            return;
        }
        this.listaIdPatologie = convertiListaPatologieStringToIdPatologia (patologieInput);
    }

    private List <int> convertiListaPatologieStringToIdPatologia (List <string> listaPatolgie){
        List <int> patologieConvertite = new List <int> ();
        int patologiaConvertitaTemp;
        foreach (string patologiaString in listaPatolgie){
            try{
                patologiaConvertitaTemp = Patologia.patologiaStringToIdPatologia (patologiaString);
                patologieConvertite.Add (patologiaConvertitaTemp);
            }
            catch (InvalidOperationException e){
                Console.WriteLine (e.Message);
            }
        }
        return patologieConvertite;
    }

    private List <string> fillListaPatologieStringhe (){
        Console.WriteLine ("Inserisci le patologie del cliente e la keyword 'fine' quando hai finito l'inserimento");
        
        string patologiaTemp = "";
        List <string> patologieInput = new List <string> ();
        
        while (true){
            patologiaTemp = Console.ReadLine ();
            if (patologiaTemp.Equals("no") || patologiaTemp.Equals ("fine"))
                break;
            patologieInput.Add (patologiaTemp);
        }

        return patologieInput;
    }

    //distruttore
    ~Cliente() 
    {
    
    }
}