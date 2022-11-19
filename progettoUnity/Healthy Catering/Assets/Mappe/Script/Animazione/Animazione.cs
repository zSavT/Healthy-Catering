using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class Animazione : MonoBehaviour
{
    /*
        Answer by AQaddora2020
        https://answers.unity.com/questions/931917/animate-image-ui-with-sprite-sheet.html
    */

    //0 è troppo veloce, 1 dovrebbe essere semplicemente un x1 sulla velocita della gif
    private float duration;

    private List<Sprite> sprites;

    private Image image;
    private int index = 0;
    private float timer = 0;

    List<string> nomiAnimazioniOkBoxVideo = new List<string>
    {
        "wasd",
        "salto",
        "shift",
        "interazioneZio",
        "entrareRistorante",
        "clienteCompatibile",
        "clienteNonCompatibile",
        "visualizzareMagazzino",
        "negozio",
        "interazionePassanti",
        "ricettario",
        "menuAiuto",
    };

    List<string> nomiAnimazioniMenuAiuto = new List<string>
    {
        "movimenti",
        "interazioneConIClientiPrimaParte",
        "interazioneConIClienti",
        "movimenti",
        "visualizzareMagazzino",
        "movimenti",
        "interazionePassanti",
        "negozio",
        "ricettario",
        "movimenti",
    };

    void Start()
    {
        image = GetComponent<Image>();
        /*
        //esempio di chiamata 
        caricaAnimazione("immaginiAiuto", "movimenti", "aaa");
        // 33 e' il numero di frame massimo ottenibili dalla scomposizione in frame dal sito https://ezgif.com/split
        // ed e' ovviamente il valore che usiamo per la conversione
        duration = sprites.Count / 33; 
        image = GetComponent<Image>();
        */
        
        foreach (string nome in nomiAnimazioniOkBoxVideo)
        {
            caricaAnimazione("immaginiOGifOkBoxVideo", nome, "default");
        }

        foreach (string nome in nomiAnimazioniMenuAiuto)
        {
            caricaAnimazione("immaginiAiuto", nome, "default");
        }
        
    }

    private void Update()
    {
        if ((timer += Time.deltaTime) >= (duration / sprites.Count))
        {
            timer = 0;
            image.sprite = sprites[index];
            index = (index + 1) % sprites.Count;
        }
    }

    public void caricaAnimazione(string cartella, string nomeGif, string nomeGifDefault)
    {
        sprites = new List<Sprite>();
        int i = 1;
        Sprite immagineTemp = Resources.Load<Sprite>(cartella + "/" + nomeGif + "/" + i.ToString()); ;

        while (true)
        {
            immagineTemp = Resources.Load<Sprite>(cartella + "/" + nomeGif + "/" + i.ToString());
            if (immagineTemp == null)//quando non ci sono più frame nella cartella
            {
                break;
            }
            sprites.Add(immagineTemp);
            i++;
        }

        duration = sprites.Count / 20;

        if (sprites.Count == 0)
        {
            if (!nomeGifDefault.Equals(""))
            {
                caricaAnimazione(cartella, nomeGifDefault, "");
            }
            //la seconda volta che viene chiamato lo script, quindi quando non trova neanche
            //l'immagine di default viene alzata l'eccezione
            else
            {
                throw new System.Exception ("la gif cercata non esiste");
            }
        }

        index = 0;
        timer = 0;
    }
}
