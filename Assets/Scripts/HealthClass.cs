

public class HealthClass 
{
    // Start is called before the first frame update
    public float _vida { get; private set; }
    public float _vidaMax { get; private set; }

    


    //Constructor d'un sistema de Vida
    public HealthClass(float vida, float vidaMax)
    {
        _vida = vida;
        _vidaMax = vidaMax;
    }

    public void CanviarVida(float canviVida)
    {
        _vida += canviVida;
        if(_vida > _vidaMax)
        {
            _vida = _vidaMax;
        }
        else if(_vida < 0)
        {
            _vida = 0;
        }

    }
    public void CanviarVidaMax(float _vidaM)
    {
        _vidaMax = _vidaM;
    }
  

}
