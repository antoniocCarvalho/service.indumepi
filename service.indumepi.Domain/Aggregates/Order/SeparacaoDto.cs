using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace service.indumepi.Domain.Aggregates.Order
{
    public class SeparacaoDto
    {
        public Guid Id { get; set; }
        public int PrimeiraSeparacao { get; set; }
        public int SegundaSeparacao { get; set; }
    }

}
