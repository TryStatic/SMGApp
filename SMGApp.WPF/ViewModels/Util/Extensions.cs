namespace SMGApp.WPF.ViewModels.Util
{
    public static class Extensions
    {
        public static string ToUpperΝοintonation(this string str)
        {
            if (string.IsNullOrEmpty(str)) return null;

            //α, ε, η, ι, υ, ο, ω.
            str = str.ToUpper();
            str = str.Replace('Ά', 'Α');
            str = str.Replace('Έ', 'Ε');
            str = str.Replace('Ή', 'Η');
            str = str.Replace('Ί', 'Ι');
            str = str.Replace('Ό', 'Ο');
            str = str.Replace('Ώ', 'Ω');

            str = str.Trim();
            return str;
        }
    }
}