namespace user_client.Utils
{
    public static class ImageUtils
    {
        public static byte[] ConvertFromIFormFile(IFormFile file)
        {
            using MemoryStream stream = new ();
            file.CopyTo(stream);
            return stream.ToArray();
        }
    }
}
