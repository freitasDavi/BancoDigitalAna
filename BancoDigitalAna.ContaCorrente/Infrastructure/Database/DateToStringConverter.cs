using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Globalization;

namespace BancoDigitalAna.Conta.Infrastructure.Database
{
    public class DateToStringConverter : ValueConverter<DateTime, string>
    {
        public DateToStringConverter()
            : base(
                  v => v.ToString("dd/MM/YYYY"),
                  v => DateTime.ParseExact(v, "dd/MM/YYYY", CultureInfo.InvariantCulture)
                  )
        { }
    }
}
