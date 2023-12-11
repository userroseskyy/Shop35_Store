using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop35_Store.Domain.Entities.Account
{
    public  class TwoFactorLoginDto
    {
        public string Code {  get; set; }
        public bool  IsPersistent { get; set; }
        public string Provider { get; set; }

    }
}
