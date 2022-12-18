using System.Linq;
using System.Collections.Generic;

public class Cliente
{
    public string nome = "";

    public int dieta = 0;
    public List<int> listaIdPatologie = null;

    //costruttore
    public Cliente(string nome, int dieta, List<int> listaIdPatologie)
    {
        this.nome = nome;
        this.dieta = dieta;
        this.listaIdPatologie = listaIdPatologie;
    }

    public Cliente()
    {
        this.nome = "";
        this.dieta = -1;
        this.listaIdPatologie = new List<int>();
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

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override string ToString()
    {
        string listaIdPatologieString = Patologia.listIdToListPatologie(this.listaIdPatologie);

        string output = "Cliente:" + "\n\t" + this.nome + "\n" + "Dieta:" + "\n\t" + this.dieta + "\n";

        if (!(listaIdPatologieString.Equals("")))
            output = output + "Patologie:" + listaIdPatologieString + "\n";

        return output + "Fine cliente " + this.nome;
    }

    //distruttore
    ~Cliente()
    {

    }

    /// <summary>
    /// Il metodo restituisce la lista degli ingredienti non campatibili con la patologia del cliente della lista degli ingredienti passati in input
    /// </summary>
    /// <param name="ingredientiPiatto">List<Ingrediente> ingredientiPiatto da controllare</param>
    /// <returns>List ingredienti non compatibili</returns>
    public List<Ingrediente> getListaIngredientiNonCompatibiliPatologie(List<Ingrediente> ingredientiPiatto)
    {
        List<Ingrediente> ingredientiNonCompatibili = new List<Ingrediente>();

        foreach (int idPatologia in this.listaIdPatologie)
        {
            foreach (Ingrediente ingrediente in ingredientiPiatto)
            {
                List<int> listaPatologieCompatibiliConIngrediente = ingrediente.listaIdPatologieCompatibili;

                if (!(listaPatologieCompatibiliConIngrediente.Contains(idPatologia)))
                {
                    ingredientiNonCompatibili.Add(ingrediente);
                }
            }
        }

        return ingredientiNonCompatibili;
    }


    /// <summary>
    /// Il metodo restituisce la lista degli ingredienti non campatibili con la dieta del cliente della lista degli ingredienti passati in input
    /// </summary>
    /// <param name="ingredientiPiatto">List<Ingrediente> ingredientiPiatto da controllare</param>
    /// <returns>List ingredienti non compatibili</returns>
    public List<Ingrediente> getListaIngredientiNonCompatibiliDieta(List<Ingrediente> ingredientiPiatto)
    {
        List<Ingrediente> ingredientiNonCompatibili = new List<Ingrediente>();

        foreach (Ingrediente ingrediente in ingredientiPiatto)
        {
            if (this.dieta < ingrediente.dieta)
            {
                ingredientiNonCompatibili.Add(ingrediente);
            }
        }

        return ingredientiNonCompatibili;
    }
}