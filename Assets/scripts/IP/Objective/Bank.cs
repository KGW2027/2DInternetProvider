using IP.UIFunc;

namespace IP.Objective
{
    public class Bank
    {
        public string Name { get; }
        public float Interest { get; private set; }
        public long MaxLoanSize { get; private set; }
        
        private long _curLoanSize;
        private long _minLoanSize;
        private float _minInterest;
        
        private float _multiply;
        
        
        public Bank(string name, float multiply, float mi)
        {
            Name = name;
            
            _minInterest = mi;
            _minLoanSize = 5000L;
            _curLoanSize = 0L;
            
            _multiply = multiply;
            
            CalcNewInterest();
            CalcNewLoanSize();
        }

        public void CalcNewLoanSize()
        {
            MaxLoanSize = (long) (_minLoanSize + GetCompanyRevenue()*_multiply - _curLoanSize);
        }

        public void CalcNewInterest()
        {
            Interest = _minInterest;
        }

        public void Loaned(long amount)
        {
            _curLoanSize += amount;
            CalcNewLoanSize();
        }

        public void Repayed(long amount)
        {
            AlertBox.New(AlertBox.AlertType.LoanEnd, Name, $"{amount:N0}");
            Loaned(amount*-1);
        }

        private long GetCompanyRevenue()
        {
            return GameManager.Instance.Company.RecentRevenue(12);
        }
    }
}