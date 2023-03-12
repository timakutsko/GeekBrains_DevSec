using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson5_CertGenerator
{
    public class CertGenerationException : Exception
    {
        public CertGenerationException(string msg) : base(msg)
        {
        }
    }
}
