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
    private float duration = 1;

    private List<Sprite> sprites;

    private Image image;
    private int index = 0;
    private float timer = 0;

    void Start()
    {
        //esempio di chiamata 
        //caricaAnimazione("immaginiAiuto/immagineAiuto", "Default", "aaa");
        image = GetComponent<Image>();
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

    private void caricaAnimazione(string cartellaEinizioNomeFile, string stringNumeroGif, string nomeGifDefault)
    {
        sprites = new List<Sprite>();
        Sprite immagineTemp;
        
        int i = 0;
        while (true)
        {
            immagineTemp = Resources.Load<Sprite>(cartellaEinizioNomeFile + stringNumeroGif + "/" + i.ToString());
            if (immagineTemp == null)//quando non ci sono più frame nella cartella
            {
                break;
            }
            sprites.Add(immagineTemp);
            i++;
        }
        
        if (sprites.Count == 0)
        {
            if (!nomeGifDefault.Equals(""))
            {
                caricaAnimazione(cartellaEinizioNomeFile, nomeGifDefault, "");
            }
            //la seconda volta che viene chiamato lo script, quindi quando non trova neanche
            //l'immagine di default viene alzata l'eccezione
            else
            {
                throw new System.Exception ("la gif cercata non esiste");
            }
        }
    }
}
