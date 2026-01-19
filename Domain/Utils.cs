namespace Application.Common
{
    public static class Utils
    {
        public static string FormataCpfSemPontuacao(this string cpf)
        {

            return cpf.Replace(".", "").Replace("-", "");
        }

 
 
 
    }
}
