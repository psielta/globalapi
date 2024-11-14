namespace GlobalAPI_ACBrNFe.Lib.ACBr.NFe.Utils
{
    public class DFeUtils
    {
        private static readonly int[,] CCodigosDFeInvalidos = new int[,]
        {
        { 0, 1111111, 2222222, 3333333, 4444444, 5555555, 6666666, 7777777, 8888888, 9999999,
          1234567, 2345678, 3456789, 4567890, 5678901, 6789012, 7890123, 8901234, 9012345, 0123456 },
        { 0, 11111111, 22222222, 33333333, 44444444, 55555555, 66666666, 77777777, 88888888, 99999999,
          12345678, 23456789, 34567890, 45678901, 56789012, 67890123, 78901234, 89012345, 90123456, 01234567 }
        };

        public static int GerarCodigoDFe(int AnDF, int ADigitos = 8)
        {
            Random random = new Random();
            int ACodigo;
            int ARange = int.Parse(new string('9', ADigitos));

            do
            {
                ACodigo = random.Next(ARange);
            } while (!ValidarCodigoDFe(ACodigo, AnDF, ADigitos));

            return ACodigo;
        }

        private static bool ValidarCodigoDFe(int AcDF, int AnDF, int ADigitos = 8)
        {
            if (AcDF == AnDF)
                return false;

            int index = ADigitos == 8 ? 1 : 0;

            for (int i = 0; i < 20; i++)
            {
                if (AcDF == CCodigosDFeInvalidos[index, i])
                    return false;
            }

            return true;
        }
    }
}
