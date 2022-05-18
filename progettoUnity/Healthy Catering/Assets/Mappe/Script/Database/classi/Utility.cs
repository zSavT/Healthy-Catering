public class Utility{
    public static float calcolaCostoPercentuale (float costoBase, float percentuale){
        return ((costoBase * percentuale) / 100);
    }

    public static float valoreAssoluto (float numero){
        if (numero < 0)
            return numero - numero*2; // -2 --> -2 - (-2)*2 = -2 - (-4) = -2 + 4 = 2
        return numero;
    }

    public static bool compresoFra (float numero, float estremoInferiore, float estremoSuperiore)
    {
        return ((estremoInferiore <= numero) && (numero <= estremoSuperiore));
    }
}