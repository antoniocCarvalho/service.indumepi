using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace service.indumepi.Domain.Aggregates.Family
{
    public class Family
    {
        public Guid Id { get; set; }
        public string CodFamilia { get; set; }
        public string CodInt { get; set; }
        public long Codigo { get; set; }
        public string Inativo { get; set; }
        public string NomeFamilia { get; set; }


        public Family() { }



        public Family( string codFamilia, string codInt, long codigo, string inativo, string nomeFamilia)
        {
            this.CodFamilia = codFamilia;
            this.CodInt = codInt;
            this.Codigo = codigo;
            this.Inativo = inativo;
            this.NomeFamilia = nomeFamilia;
        }
    }
}
