using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShareNet.Application.Exceptions
{
    public class DonationException : Exception
    {
        public DonationException(String message) : base(message) { }
    }
}
