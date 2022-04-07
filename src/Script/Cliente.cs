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

    public Cliente (){
        this.nome = "";
        this.dieta = -1;
        this.listaIdPatologie = new List <int> ();
    }

    public override bool Equals(object obj)
    {
        // If the passed object is null
        if (obj == null)
        {
            return false;
        }
        if (!(obj is Cliente))
        {
            return false;
        }
        return (this.nome.Equals(((Cliente)obj).nome))
            && (this.dieta == ((Cliente)obj).dieta)
            && (Enumerable.SequenceEqual(this.listaIdPatologie, ((Cliente)obj).listaIdPatologie));
    }

    public override string ToString()
    {
        string listaIdPatologieString = "";
        
        if (this.listaIdPatologie.Count > 0)
            List <Patologia> databasePatologie = Database.getDatabaseOggetto (new Patologia ());
            foreach (int id in listaIdPatologie){
                Patologia temp = Patologia.IdToPatologia(id, databasePatalogie).nome;
                if (temp.idPatologia != -1)
                    listaIdPatologieString = listaIdPatologieString + "\t" + Patologia.IdToPatologia(id).nome + "\n";
            }
        
        string output = "Cliente: " + "\n" + this.nome + "\n" + this.dieta + "\n";
        
        if (!(listaIdPatologieString.Equals ("")))
            output = output + "Patologie:\n" + listaIdPatologieString + "\n";
        
        return output + "fine cliente " + this.nome;
    }

    //distruttore
    ~Cliente() 
    {
    
    }
}