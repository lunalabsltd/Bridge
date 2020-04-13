namespace System
{
    /// <summary>
    /// Provides an object representation of a uniform resource identifier (URI) and easy access to the parts of the URI.
    /// </summary>
    [Bridge.Convention(Member = Bridge.ConventionMember.Field | Bridge.ConventionMember.Method, Notation = Bridge.Notation.CamelCase)]
    [Bridge.External]
    [Bridge.Reflectable]
    public class Uri
    {
        public extern Uri(string uriString);

        public extern string AbsoluteUri
        {
            [Bridge.Template("getAbsoluteUri()")]
            get;
        }

        [Bridge.Template("System.Uri.equals({uri1}, {uri2})")]
        public static extern bool operator ==(Uri uri1, Uri uri2);

        [Bridge.Template("System.Uri.notEquals({uri1}, {uri2})")]
        public static extern bool operator !=(Uri uri1, Uri uri2);

        public static string EscapeDataString(string stringToEscape)
        {
            Console.Write("Not Implemented Properly");
            if (stringToEscape == null)
                throw new ArgumentNullException(nameof (stringToEscape));
            if (stringToEscape.Length == 0)
                return string.Empty;
            int destPos = 0;
            char[] chArray = null;//UriHelper.EscapeString(stringToEscape, 0, stringToEscape.Length, (char[]) null, ref destPos, false, char.MaxValue, char.MaxValue, char.MaxValue);
            if (chArray == null)
                return stringToEscape;
            return new string(chArray, 0, destPos);
        }

        public static string EscapeUriString(string stringToEscape)
        {
            Console.Write("Not Implemented Properly");
            if (stringToEscape == null)
                throw new ArgumentNullException(nameof (stringToEscape));
            if (stringToEscape.Length == 0)
                return string.Empty;
            int destPos = 0;
            char[] chArray = null;//UriHelper.EscapeString(stringToEscape, 0, stringToEscape.Length, (char[]) null, ref destPos, true, char.MaxValue, char.MaxValue, char.MaxValue);
            if (chArray == null)
                return stringToEscape;
            return new string(chArray, 0, destPos);
        }
    }
}