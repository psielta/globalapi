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

        private static readonly Dictionary<string, int> UFs = new Dictionary<string, int>
        {
            { "AC", 12 }, { "AL", 27 }, { "AP", 16 }, { "AM", 13 },
            { "BA", 29 }, { "CE", 23 }, { "DF", 53 }, { "ES", 32 },
            { "GO", 52 }, { "MA", 21 }, { "MT", 51 }, { "MS", 50 },
            { "MG", 31 }, { "PA", 15 }, { "PB", 25 }, { "PR", 41 },
            { "PE", 26 }, { "PI", 22 }, { "RJ", 33 }, { "RN", 24 },
            { "RS", 43 }, { "RO", 11 }, { "RR", 14 }, { "SC", 42 },
            { "SP", 35 }, { "SE", 28 }, { "TO", 17 }
        };

        public static int? GetCodigoUF(string uf)
        {
            if (string.IsNullOrEmpty(uf))
                throw new ArgumentException("UF não pode ser nulo ou vazio.");

            uf = uf.ToUpper();

            return UFs.TryGetValue(uf, out int codigo) ? codigo : (int?)null;
        }
    }
}
