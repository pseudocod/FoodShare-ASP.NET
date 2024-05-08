using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShareNet.Application.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(String name, Object key) 
            : base($"Entity '{name}' ({key}) was not found.") 
        {
        }

        public NotFoundException(String message) : base(message) { }
    }
}
