namespace IP.Objective
{
    public class Bank
    {
        public string Name { get; }
        public float Interest { get; private set; }
        public long MaxLoanSize { get; private set; }
        
        private long _curLoanSize;
        private float _buildWeight;
        private long _minLoanSize;
        private float _minInterest;
        
        
        public Bank(string name, float bw, float mi)
        {
            Name = name;
            _buildWeight = bw;
            _minInterest = mi;
            _minLoanSize = 5000L;
            _curLoanSize = 0L;
            
            CalcNewInterest();
            CalcNewLoanSize();
        }

        public void CalcNewLoanSize()
        {
            MaxLoanSize = _minLoanSize - _curLoanSize;
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
    }
}