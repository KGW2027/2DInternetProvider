namespace IP.UIFunc.Builder
{
    public interface IUIBuilder
    {
        public void Build();

        public void SendData(params object[] datas);
    }
}