using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalLib.Utils
{
    public static class DateUtils
    {
        /// <summary>
        /// Converte um objeto DateTime em um objeto DateOnly.
        /// </summary>
        /// <param name="dateTime">O objeto DateTime a ser convertido.</param>
        /// <returns>O objeto DateOnly convertido.</returns>
        public static DateOnly DateTimeToDateOnly(DateTime dateTime)
        {
            return DateOnly.FromDateTime(dateTime);
        }

        public static TimeOnly DateTimeToTimeOnly(DateTime dateTime)
        {
            return TimeOnly.FromDateTime(dateTime);
        }

        /// <summary>
        /// Converte um objeto DateOnly em um objeto DateTime.
        /// </summary>
        /// <param name="dateOnly">O objeto DateOnly a ser convertido.</param>
        /// <returns>O objeto DateTime convertido.</returns>
        public static DateTime DateOnlyToDateTime(DateOnly dateOnly)
        {
            return dateOnly.ToDateTime(TimeOnly.MinValue);
        }

        /// <summary>
        /// Verifica se uma data é um fim de semana (sábado ou domingo).
        /// </summary>
        /// <param name="dateOnly">O objeto DateOnly a ser verificado.</param>
        /// <returns>True se a data for um fim de semana, caso contrário, false.</returns>
        public static bool IsWeekend(DateOnly dateOnly)
        {
            DayOfWeek day = dateOnly.DayOfWeek;
            return day == DayOfWeek.Saturday || day == DayOfWeek.Sunday;
        }

        /// <summary>
        /// Verifica se uma data está dentro de um intervalo de datas.
        /// </summary>
        /// <param name="date">O objeto DateOnly a ser verificado.</param>
        /// <param name="startDate">A data de início do intervalo.</param>
        /// <param name="endDate">A data de término do intervalo.</param>
        /// <returns>True se a data estiver dentro do intervalo, caso contrário, false.</returns>
        public static bool IsWithinRange(DateOnly date, DateOnly startDate, DateOnly endDate)
        {
            return date >= startDate && date <= endDate;
        }

        /// <summary>
        /// Calcula o número de dias entre duas datas.
        /// </summary>
        /// <param name="startDate">A data de início.</param>
        /// <param name="endDate">A data de término.</param>
        /// <returns>O número de dias entre as duas datas.</returns>
        public static int DaysBetween(DateOnly startDate, DateOnly endDate)
        {
            return Math.Abs((endDate.ToDateTime(TimeOnly.MinValue) - startDate.ToDateTime(TimeOnly.MinValue)).Days);
        }

        /// <summary>
        /// Retorna a data atual como um objeto DateOnly.
        /// </summary>
        /// <returns>A data atual como um objeto DateOnly.</returns>
        public static DateOnly Today()
        {
            return DateOnly.FromDateTime(DateTime.Now);
        }

        /// <summary>
        /// Adiciona um número especificado de dias a um objeto DateOnly e retorna o resultado.
        /// </summary>
        /// <param name="date">O objeto DateOnly ao qual adicionar dias.</param>
        /// <param name="days">O número de dias a serem adicionados.</param>
        /// <returns>O objeto DateOnly com os dias adicionados.</returns>
        public static DateOnly AddDays(DateOnly date, int days)
        {
            return date.AddDays(days);
        }

        /// <summary>
        /// Converte uma string em formato de data em um objeto DateOnly.
        /// </summary>
        /// <param name="dateString">A string em formato de data a ser convertida.</param>
        /// <returns>O objeto DateOnly convertido.</returns>
        public static DateOnly ParseDateOnly(string dateString)
        {
            return DateOnly.Parse(dateString);
        }

        /// <summary>
        /// Tenta converter uma string em formato de data em um objeto DateOnly, retornando um booleano indicando o sucesso.
        /// </summary>
        /// <param name="dateString">A string em formato de data a ser convertida.</param>
        /// <param name="date">O objeto DateOnly convertido.</param>
        /// <returns>True se a conversão for bem-sucedida, caso contrário, false.</returns>
        public static bool TryParseDateOnly(string dateString, out DateOnly date)
        {
            return DateOnly.TryParse(dateString, out date);
        }
    }
    public static class TimeOnlyExtensions
    {
        public static TimeOnly TruncateToMinutes(this TimeOnly time)
        {
            return new TimeOnly(time.Hour, time.Minute);
        }
    }

}
