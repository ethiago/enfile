namespace Enfile.Commons.Model
{
    public enum FileEncoding : byte
    {
        ASCII,
        UTF8,
    }

    public static class FileEncodingConverterExtension
    {
        public static System.Text.Encoding ConvertToSystemEncoding(this FileEncoding enconding)
        {
            switch(enconding)
            {
                case FileEncoding.ASCII:
                    return System.Text.Encoding.ASCII;
                case FileEncoding.UTF8:
                    return System.Text.Encoding.UTF8;
                default:
                    return System.Text.Encoding.Default;
            }
        }
    }

}