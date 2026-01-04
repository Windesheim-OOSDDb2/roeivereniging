using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoeiVereniging.Core.Interfaces.Services
{
    public interface IQrCodeService
    {
        public MemoryStream GenerateQrCode(string content);
    }
}
