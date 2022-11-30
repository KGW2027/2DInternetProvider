using System.Collections.Generic;

namespace IP.Objective
{
    public class PaymentPlan
    {
        public readonly Company OwnerCompany;
        private readonly List<City> _cities;
        public string Name;
        public long Budget;
        public int Bandwidth;
        public int Upload;
        public int Download;

        public PaymentPlan(Company comp)
        {
            _cities = new List<City>();
            OwnerCompany = comp;
        }

        public void Service(City c)
        {
            _cities.Add(c);
            c.ServicingPlans.Add(this);
        }

        public void Deservice(City c)
        {
            if (_cities.Contains(c))
            {
                _cities.Remove(c);
                c.ServicingPlans.Remove(this);
            }
        }

        public long GetRevenue()
        {
            long revenue = 0L;
            foreach (City city in _cities)
            {
                revenue += city.GetCustomer(this) * Budget;
            }
            return revenue;
        }

    }
}