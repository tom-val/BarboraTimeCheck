using System;
using System.Collections.Generic;

namespace BarboraTimeCheck.Services.Models
{
    public class Hour
    {
        public string id { get; set; }
        public DateTime deliveryTime { get; set; }
        public string hour { get; set; }
        public double price { get; set; }
        public bool available { get; set; }
        public bool isUnavailableAlcSellingTime { get; set; }
        public bool isUnavailableAlcOrEnergySelling { get; set; }
        public bool isLockerOrPup { get; set; }
        public double salesCoefficient { get; set; }
        public string deliveryWave { get; set; }
        public int pickingHour { get; set; }
        public object changeTimeslotShop { get; set; }
    }

    public class Matrix
    {
        public string id { get; set; }
        public bool isExpressDelivery { get; set; }
        public string day { get; set; }
        public string dayShort { get; set; }
        public List<Hour> hours { get; set; }
    }

    public class Params
    {
        public List<Matrix> matrix { get; set; }
    }

    public class Delivery
    {
        public string title { get; set; }
        public Params @params { get; set; }
    }

    public class Deliveries
    {
        public List<Delivery> deliveries { get; set; }
        public int reservationValidForSeconds { get; set; }
        public object messages { get; set; }
    }
}
