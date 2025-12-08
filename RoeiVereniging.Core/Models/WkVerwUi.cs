using RoeiVereniging.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoeiVereniging.Core.Models
{
    public class WkVerwUi
    {
        private readonly WkVerw _dto;

        public WkVerwUi(WkVerw dto) => _dto = dto ?? throw new ArgumentNullException(nameof(dto));

        public string Dag => ConvertDateToDayHelper.ConvertToDayName(_dto.Dag) ?? string.Empty;
        public double MinTemp => _dto.MinTemp;
        public double MaxTemp => _dto.MaxTemp;

        public string TempRange => $"{MinTemp:0.#} / {MaxTemp:0.#} °C";

        public string ImageKey => _dto.Image ?? string.Empty;

        public string IconImage => WeatherImageFileMapperHelper.MapToImageFile(ImageKey);
    }
}
