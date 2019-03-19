public static class ContadorCiclico
{
    public static int AlteraContador(int alteracao, int contadorAtual, int contadorMaximo)
    {

        if (contadorAtual + alteracao < contadorMaximo && contadorAtual + alteracao >= 0)
        {
            return contadorAtual + alteracao;
        }
        else if (contadorAtual + alteracao >= contadorMaximo)
        {
            return 0;
        }
        else if (contadorAtual + alteracao < 0)
            return contadorMaximo - 1;
        else
            return -1;

    }
}