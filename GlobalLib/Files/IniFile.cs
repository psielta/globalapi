using GlobalLib.Files;
using System;
using System.Runtime.InteropServices;
using System.Text;

public class IniFile
{
    private string filePath;

    [DllImport("kernel32", CharSet = CharSet.Unicode)]
    private static extern long WritePrivateProfileString(string section, string key, string value, string filePath);

    [DllImport("kernel32", CharSet = CharSet.Unicode)]
    private static extern int GetPrivateProfileString(string section, string key, string defaultValue, StringBuilder value, int size, string filePath);

    public IniFile(string filePath)
    {
        this.filePath = filePath;
    }

    public void Write(string section, string key, string value)
    {
        WritePrivateProfileString(section, key, value, this.filePath);
    }

    public string Read(string section, string key, string defaultValue = "")
    {
        StringBuilder value = new StringBuilder(1024);
        GetPrivateProfileString(section, key, defaultValue, value, value.Capacity, this.filePath);
        return value.ToString();
    }
    public static string GetConnectionString()
    {
        IniFile iniFile = new IniFile($@"{DiretoriosPadroes.DIRETORIO_PADRAO}\GlobalPostGre.ini");
        string hostname = iniFile.Read("Banco de Dados", "hostname");
        string database = iniFile.Read("Banco de Dados", "database");
        string port = iniFile.Read("Banco de Dados", "port");
        string user = iniFile.Read("Banco de Dados", "user");
        string password = iniFile.Read("Banco de Dados", "password");
        if (string.IsNullOrEmpty(hostname))
        {
            throw new Exception("Servidor não configurado.");
        }
        if (string.IsNullOrEmpty(database))
        {
            throw new Exception("Banco de dados não configurado.");
        }
        if (string.IsNullOrEmpty(port))
        {
            throw new Exception("Porta não configurada.");
        }
        if (string.IsNullOrEmpty(user))
        {
            throw new Exception("Usuário não configurado.");
        }
        if (string.IsNullOrEmpty(password))
        {
            throw new Exception("Senha não configurada.");
        }
        //Obs.: Instalado banco na porta 5435 na versao 16.3, versao 9.2 não funciona no entity pois campos seriais não são suportados
        return $"Host={hostname};Port={port};Database={database};Username={user};Password={password}";
    }
}
