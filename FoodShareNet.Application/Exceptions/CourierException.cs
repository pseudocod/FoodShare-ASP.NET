using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShareNet.Application.Exceptions
{
    public class CourierException : Exception
    {
        public CourierException(String message) : base(message) { }
    }
}
