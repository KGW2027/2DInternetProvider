namespace IP.Objective
{
    public class Bank
    {
        public string Name { get; }
        public float Interest { get; }
        public long MaxLoanSize { get; }
        
        
        public Bank(string name)
        {
            Name = name;
            Interest = 2.7f;
            MaxLoanSize = 5000;
        }
    }
}