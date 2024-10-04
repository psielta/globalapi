using ACBrLib.Core;

namespace GlobalAPI_ACBrNFe.Lib.ACBr.Generic
{
    public abstract class LibBaseDiversos<T> where T : ACBrLibHandle, IDisposable
    {
        public T ACBrLib { get; set; }
        public abstract void SetConfiguracao(T ACBrLib);
        public LibBaseDiversos()
        {
            ACBrLib = (T)Activator.CreateInstance(typeof(T), DiretoriosACBr.GetPathAcbrLib(), "");
            SetConfiguracao(ACBrLib);
            ACBrLib.ConfigGravar();
        }
        public LibBaseDiversos(string path)
        {
            ACBrLib = (T)Activator.CreateInstance(typeof(T), path, "");
            SetConfiguracao(ACBrLib);
            ACBrLib.ConfigGravar();
        }

        public void Dispose()
        {
            ACBrLib.Dispose();
            ACBrLib = null;
        }

    }
    public static class DiretoriosACBr
    {
        public const string PATH = @"C:\Global\WebAPI\Api\NFe";
        public static string GetPath()
        {
            return PATH;
        }
        public static string GetPathAcbrLib()
        {
            return PATH + @"\bin\ACBrLib.ini";
        }
    }
}
