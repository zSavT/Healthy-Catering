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

    public void getNewNomeClienteFromUtente (string output){
        Console.WriteLine (output);
        return Console.ReadLine();
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