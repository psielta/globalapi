using ACBrLib.NFe;
using static ACBrLib.NFe.ACBrNFe;

namespace GlobalAPI_ACBrNFe.Lib.ACBr.NFe
{
    public class ACBrNFeManager //: IDisposable
    {
       /* private IntPtr _libHandle;
        private bool _disposed;

        public ACBrNFeManager(string eArqConfig = "", string eChaveCrypt = "")
        {
            // Inicializa a ACBrLib com um novo ponteiro para esta thread
            var inicializar = GetMethod<NFE_Inicializar>();
            var ret = inicializar(ref _libHandle, ToUTF8(eArqConfig), ToUTF8(eChaveCrypt));

            CheckResult(ret);
        }

        public ACBrNFe GetNFe()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(ACBrNFeManager));

            return CreateNFeInstance();
        }

        private ACBrNFe CreateNFeInstance()
        {
            var nfe = new ACBrNFe(); // Supondo que seja o construtor padrão
            nfe.SetLibHandle(_libHandle); // Método para definir o libHandle na instância
            return nfe;
        }

        public void Dispose()
        {
            if (_disposed)
                return;

            if (_libHandle != IntPtr.Zero)
            {
                var finalizar = GetMethod<NFE_Finalizar>();
                var ret = finalizar(_libHandle);
                CheckResult(ret);
                _libHandle = IntPtr.Zero;
            }

            _disposed = true;
        }

        private T GetMethod<T>() where T : Delegate
        {
            // Lógica para obter o método a partir da DLL carregada
            // Exemplo simplificado
            return default;
        }

        private void CheckResult(int result)
        {
            if (result != 0)
            {
                throw new Exception($"Erro ao executar operação: {result}");
            }
        }

        private string ToUTF8(string value)
        {
            // Conversão para UTF-8
            return value;
        }*/
    }


}
