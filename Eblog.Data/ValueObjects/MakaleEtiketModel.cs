namespace Eblog.Data.ValueObjects
{
    using Eblog.Data.Model;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;


    public partial class MakaleEtiketModel
    {
        public makale Makale { get; set; }

        public string[] GelenEtikler { get; set; }

        public IEnumerable<etiket> Etiketler { get; set; }

        public string EtiketAd { get; set; }
    }
}
