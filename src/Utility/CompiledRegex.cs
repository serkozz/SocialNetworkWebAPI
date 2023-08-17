using System.Text.RegularExpressions;

namespace Utility;
public static partial class CompiledRegex
{
    [GeneratedRegex("[a-z]", RegexOptions.IgnoreCase, "ru-RU")]
    public static partial Regex ContainsLetter();
    [GeneratedRegex("[0-9]")]
    public static partial Regex ContainsNumber();
    [GeneratedRegex("[!@#$%&*()_+=|<>?{}\\[\\]~-]")]
    public static partial Regex ContainsSpecialCharacter();
    [GeneratedRegex($@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*")]
    public static partial Regex ValidEmail();
}