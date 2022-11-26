using System.Collections.Generic;

namespace IP.Objective
{
    public class PaymentPlan
    {
        public Company OwnerCompany;
        public string Name;
        public List<City> Cities;
        public long Budget;
        public int Bandwidth;
        public int Upload;
        public int Download;

    }
}