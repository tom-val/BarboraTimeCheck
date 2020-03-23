using System;
using System.Collections.Generic;
using System.Text;

namespace BarboraTimeCheck.Services.Models
{
    public class DeliveriesResult
    {
        public List<Hour> AvailableDeliveries { get; set; }
        public int TotalDeliveries { get; set; }
    }
}
