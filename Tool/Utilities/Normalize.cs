using System;

namespace Tool.Utilities
{
    public static class Normalize
    {
        //public static string Normalizar(string valor)
        //{
        //    if (valor != null)
        //    {
        //        string[] palavras = RemoverEspacosVazios(valor).ToUpper().Split(' ');

        //        for (int i = 0; i < palavras.Length; i++)
        //        {
        //            if (palavras[i] != "")
        //            {
        //                if (VerificarSePreposicao(palavras[i]))
        //                {
        //                    palavras[i] = palavras[i].ToLower();
        //                }
        //                else if (!VerificarSeNumeroRomano(palavras[i]))
        //                {
        //                    string primeiraLetra = palavras[i].Substring(0, 1).ToUpper();
        //                    string restante = palavras[i].Substring(1, palavras[i].Length - 1).ToLower();

        //                    palavras[i] = primeiraLetra + restante;
        //                }
        //            }
        //        }

        //        return string.Join(" ", palavras);
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}

        //public static bool VerificarSeNumeroRomano(string palavra)
        //{
        //    return palavra == "I" ||
        //           palavra == "II" ||
        //           palavra == "III" ||
        //           palavra == "IV" ||
        //           palavra == "V" ||
        //           palavra == "VI" ||
        //           palavra == "VII" ||
        //           palavra == "VIII" ||
        //           palavra == "IX" ||
        //           palavra == "X";
        //}

        //public static bool VerificarSePreposicao(string palavra)
        //{
        //    return palavra == "DE" || palavra == "DA" ||
        //           palavra == "DO" || palavra == "NA" ||
        //           palavra == "NO" || palavra == "COM" ||
        //           palavra == "UM" || palavra == "UMA" ||
        //           palavra == "NOS" || palavra == "E" ||
        //           palavra == "DAS" || palavra == "DOS" ||
        //           palavra == "UMAS" || palavra == "NAS" ||
        //           palavra == "EM" || palavra == "O" ||
        //           palavra == "PARA";
        //}

        //public static string RemoverEspacosVazios(string valor)
        //{
        //    valor = valor.Trim();

        //    string palavraAtualizada = "";
        //    for (int i = 0; i < valor.Length; i++)
        //    {
        //        char charAtual = valor[i];
        //        bool adicionarChar = true;

        //        if (i - 1 > 0)
        //        {
        //            char charAnterior = valor[i - 1];

        //            if (charAtual == ' ' && charAnterior == ' ')
        //            {
        //                adicionarChar = false;
        //            }
        //        }


        //        if (adicionarChar)
        //        {
        //            palavraAtualizada += valor[i];
        //        }
        //    }

        //    return palavraAtualizada;
        //}
    }
}
