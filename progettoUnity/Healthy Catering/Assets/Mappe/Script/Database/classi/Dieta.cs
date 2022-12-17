using System;
public class Dieta
{
    public string nome = "";
    public string descrizione = "";

    public Dieta(string nome, string descrizione)
    {
        this.nome = nome;
        this.descrizione = descrizione;
    }

    public Dieta()
    {
        this.nome = "";
        this.descrizione = "";
    }

    public override bool Equals(object obj)
    {
        // If the passed object is null
        if (obj == null)
        {
            return false;
        }
        if (!(obj is Dieta))
        {
            return false;
        }
        return (this.nome.Equals(((Dieta)obj).nome))
            && (this.descrizione.Equals(((Dieta)obj).descrizione));
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override string ToString()
    {
        return "Dieta:" + "\n\t" + this.nome + "\n" + "Descrizione: " + "\n\t" + this.descrizione + "\n" + "Fine dieta " + this.nome;
    }

    ~Dieta()
    {

    }

    /// <summary>
    /// Il metodo converte il nome della dieta passata in input con ID corrispoondente di tale dieta
    /// </summary>
    /// <param name="dieta">string dieta da convertire</param>
    /// <returns>int id dieta<para><string>0: Vegana, 1: Vegetariana, 2:Onnivora</string></para></returns>
    /// <exception cref="InvalidOperationException">Dieta passata non trovata</exception>
    private static int dietaStringToIdDieta(string dieta)
    {
        if (dieta.ToLower() == "vegana")
            return 0;
        else if (dieta.ToLower() == "vegetariana")
            return 1;
        else if (dieta.ToLower() == "onnivora")
            return 2;
        else
            throw new InvalidOperationException("Dieta inserita non valida");
    }

    /// <summary>
    /// Il metodo permette di convertire ID passatto in input con la stringa corrispondente del nome della dieta
    /// </summary>
    /// <param name="id">int id dieta da convertire <para><string>0: Vegana, 1: Vegetariana, 2:Onnivora</string></para></param>
    /// <returns>string nome dieta</returns>
    /// <exception cref="InvalidOperationException">ID passatto non esistente</exception>
    public static string IdDietaToDietaString(int id)
    {
        if (id == 0)
            return "Vegana";
        else if (id == 1)
            return "Vegetariana";
        else if (id == 2)
            return "Onnivora";
        else
            throw new InvalidOperationException("Id dieta inserito non valido");
    }
}