using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace GlobalAPI_ACBrNFe.Lib.Enum
{
    public enum SituacaoNFe
    {
        AutorizadoUso = 100,
        Cancelado = 101,
        Inutilizado = 102,
        Denegado = 110, // Uso denegado, não pode ser reenviado
        LoteRecebido = 103,
        LoteProcessado = 104,
        LoteEmProcessamento = 105,
        Rejeitado = 106, // Rejeição: Erro no preenchimento ou validação dos dados
        NaoConstaNaSEFAZ = 217
    }


    [AttributeUsage(AttributeTargets.Field)]
    public class EnumValueAttribute : Attribute
    {
        public string Value { get; private set; }

        public EnumValueAttribute(string value)
        {
            this.Value = value;
        }
    }
    public static class EnumExtensions
    {
        public static string GetEnumValue(this System.Enum value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());
            var attribute = (EnumValueAttribute)Attribute.GetCustomAttribute(fieldInfo, typeof(EnumValueAttribute));

            return attribute != null ? attribute.Value : value.ToString();
        }
    }
    public static class EnumConverter
    {
        public static Status GetStatus(int cStat)
        {
            switch (cStat)
            {
                case 100:
                    return Status.Autorizado;

                case 101:
                    return Status.Cancelado;
                case 217:
                    return Status.Pendente;
                default:
                    return Status.Pendente;
            }

        }
    }
    public enum Status
    {
        [EnumValue("01")]
        Pendente,

        [EnumValue("02")]
        Autorizado,

        [EnumValue("11")]
        Cancelado
    }
}
