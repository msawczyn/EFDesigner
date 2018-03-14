<Query Kind="Program">
  <Output>DataGrids</Output>
</Query>

void Main()
{
   Console.WriteLine(string.Join("|",patterns));
   Console.WriteLine("");
   foreach(string p in patterns)
      Console.WriteLine(p);
}

private const string NAME = "(?<name>[A-Za-z_][A-za-z0-9_]*[!]?)";
private const string TYPE = "(?<type>[A-Za-z_][A-za-z0-9_]*[?]?)";
private const string LENGTH = @"(?<length>\d+)";
private const string STRING_TYPE = "(?<type>[Ss]tring)";
private const string VISIBILITY = @"(?<visibility>public\s+|protected\s+)";
private const string INITIAL = @"(=\s*(?<initialValue>.+))";
private const string WS = @"\s*";
private const string BODY = @"(\{.+)";

private static readonly string[] patterns =
{
         //    foo
         //    foo = 12
         //    public foo = 12
         $@"^{WS}{VISIBILITY}?{NAME}{WS}{INITIAL}?",

         //    string[50] foo
         //    string[50] foo = "hello"
         //    public string[50] foo
         //    public string[50] foo = "hello"
         $@"^{WS}{VISIBILITY}?{STRING_TYPE}\[{LENGTH}\]\s+{NAME}{WS}{INITIAL}?",

         //    string(50) foo
         //    string(50) foo = "hello"
         //    public string(50) foo
         //    public string(50) foo = "hello"
         $@"^{WS}{VISIBILITY}?{STRING_TYPE}\({LENGTH}\)\s+{NAME}{WS}{INITIAL}?",

         //    foo : string[50]
         //    foo : string[50] = "hello"
         //    public foo : string[50]
         //    public foo : string[50] = "hello"
         $@"^{WS}{VISIBILITY}?{NAME}{WS}:{WS}{STRING_TYPE}\[{LENGTH}\]{WS}{INITIAL}?",

         //    foo : string(50)
         //    foo : string(50) = "hello"
         //    public foo : string(50)
         //    public foo : string(50) = "hello"
         $@"^{WS}{VISIBILITY}?{NAME}{WS}:{WS}{STRING_TYPE}\({LENGTH}\){WS}{INITIAL}?",

         //    int foo
         //    int foo = 12
         //    int foo = 12;
         //    int foo { anything...
         //    public int foo
         //    public int foo = 12
         //    public int foo = 12;
         //    public int foo { anything...
         $@"^{WS}{VISIBILITY}?{TYPE}\s+{NAME}{WS}(({INITIAL}?;?)|{BODY})?$",

         //    foo : int
         //    foo : int = 12
         //    public foo : int
         //    public foo : int = 12
         $@"^{WS}{VISIBILITY}?{NAME}{WS}:{WS}{TYPE}{WS}{INITIAL}?"
         };
