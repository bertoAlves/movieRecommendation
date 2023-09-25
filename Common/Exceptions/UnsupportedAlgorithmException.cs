namespace Common.Exceptions
{
    /// <summary>
    /// Unsupported Algorithm Exception
    /// </summary>
    public class UnsupportedAlgorithmException : Exception
    {
        public UnsupportedAlgorithmException(string algorithmName)
            : base($"Algorithm '{algorithmName}' is not implemented.")
        {
        }
    }
}
