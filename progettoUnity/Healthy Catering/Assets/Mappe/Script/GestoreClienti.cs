using UnityEngine;

/// <summary>
/// Classe per la gestione dello spawn dei clienti all'interno del ristorante<para>
/// <strong>Da aggiungere a:</strong><br></br>
/// Preferibilmente al modello del ristorante interno.
/// </para>
/// </summary>
public class GestoreClienti : MonoBehaviour
{
    [SerializeField] private GameObject[] vettoreClienti;
    [SerializeField] private ProgressoLivello livello;

    private void Start()
    {
        vettoreClienti[0].SetActive(true);
        Interactable.numeroCliente = 0;
    }

    /// <summary>
    /// Metodo che attiva il cliente successivo a quello appena de-allocato se esistono altri clienti da allocare.
    /// </summary>
    public void attivaClienteSuccessivo()
    {
        Interactable.numeroCliente++;
        if (Interactable.numeroCliente < livello.numeroDiClientiMassimi)
            vettoreClienti[Interactable.numeroCliente].SetActive(true);
    }
}
