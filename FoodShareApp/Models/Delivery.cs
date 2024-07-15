using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShareApp.Models
{
    public class DeliveryModel
    {
        public string Id { get; set; }
        public string city { get; set; }

        public int DonationId { get; set; }

        public int Quantity { get; set; }

        public int DeliveryId { get; set; }

        public DateTime DeliveryDate { get; set; }





        public DeliveryModel() { }

        public DeliveryModel(bool generateId)
        {
            if (generateId)
            {
                Id = Guid.NewGuid().ToString();
                city = "New York";
                DonationId = 1;
                Quantity = 1;
                DeliveryId = 1;
                DeliveryDate = DateTime.Now;

            }
        }




    }
}
