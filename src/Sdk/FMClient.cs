namespace Enfile
{
    public static class FMClient
    {
        public static IFMSession Connect(string baseAddress)
        {
            return new FMSession(baseAddress);
        }
    }
}