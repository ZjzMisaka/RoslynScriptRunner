﻿using RoslynScriptRunner;

/**
 * HelloWorld test
 */
string codeHelloWorld = @"
using System;
class Run
{
    public void Main()
    {
        Console.WriteLine(""Hello World"");
    }
}
";
ScriptRunner.Run(codeHelloWorld);

/**
 * Param test
 */
string codeDraw = @"
using System;
using System.Text;
class Run
{
    public string Main(int mid)
    {
        for (int i = 0; true; ++i)
        {
            int x = Math.Abs(mid - i);
            int y = mid * 2 - x;
            OutputLog(x, y, 2 * mid + 1);
            if (x == y && i > 0)
            {
                break;
            }
        }
        return ""codeDraw end"";
    }
    
    private void OutputLog(int x, int y,  int length)
    {
        StringBuilder stringBuilder = new (new string(' ', length));
        stringBuilder[x] = '*';
        stringBuilder[y] = '*';
        string value = stringBuilder.ToString();
        Console.WriteLine(value);
    }
}
";
object[] paramList = new object[1] { 30 };
string codeDrawRes = (string)ScriptRunner.Run(codeDraw, new RunOption(paramList));
Console.WriteLine(codeDrawRes);


/**
 * InstanceObject test
 */
RunOption runOptionHelloWorld = new RunOption();
runOptionHelloWorld.InstanceObject = ScriptRunner.GetInstanceObject(codeHelloWorld);
ScriptRunner.Run(runOptionHelloWorld);


/**
 * Async test
 */
string codeCostTime = @"
using System;
using System.Threading;
class Run
{
    public int Main()
    {
        Console.WriteLine(""Start sleep"");
        Thread.Sleep(3000);
        Console.WriteLine(""Sleeped 3s"");
        return 1024;
    }
}
";
Task<object> task = ScriptRunner.RunAsync(codeCostTime);
Console.WriteLine("output after RunAsync");
Thread.Sleep(1500);
Console.WriteLine("output when sleeping");
int result = (int)task.GetAwaiter().GetResult();
Console.WriteLine("output after GetResult, result is: " + result);

/**
 * Dependency test
 */
string codeMain = @"
using System;
class Run
{
    public void Main()
    {
        Console.WriteLine(""Hello World In Main"");
        ExtraClass.ExtraOutput();
    }
}
";
string codeExtra = @"
using System;
public static class ExtraClass
{
    public static void ExtraOutput()
    {
        Console.WriteLine(""Hello World In Extra"");
    }
}
";
ScriptRunner.Run(new string[] { codeMain, codeExtra });

/**
 * Dependency + InstanceObject test
 */
RunOption runOptionMainExtra = new RunOption();
runOptionMainExtra.InstanceObject = ScriptRunner.GetInstanceObject(new List<string> { codeMain, codeExtra });
ScriptRunner.Run(runOptionMainExtra);

/**
 * VB.NET HelloWorld test
 */
string codeHelloWorldVB = @"
Imports System
Public Class Run
    Public Sub Main()
        Console.WriteLine(""Hello World VB.NET"")
    End Sub
End Class
";
RunOption runOptionVB = new RunOption() { ScriptLanguage = ScriptLanguage.VisualBasic };
ScriptRunner.Run(codeHelloWorldVB, runOptionVB);

/**
 * Private test
 */
string codeHelloWorldPrivate = @"
using System;
class Run
{
    private void Main()
    {
        Console.WriteLine(""Hello World Private"");
    }
}
";
RunOption runOptionHelloWorldPrivate = new RunOption();
runOptionHelloWorldPrivate.NonPublic = true;
ScriptRunner.Run(codeHelloWorldPrivate, runOptionHelloWorldPrivate);

/**
 * Static test
 */
string codeStatic = @"
using System;
public static class Run
{
    public static int count = 1;
    public static void Main()
    {
        Console.WriteLine(""Hello World Static: "" + count++);
    }
}
";
RunOption runOptionStatic = new RunOption() { IsStatic = true };
runOptionStatic.InstanceObject = ScriptRunner.GetInstanceObject(codeStatic, runOptionStatic);
ScriptRunner.Run(runOptionStatic);
ScriptRunner.Run(runOptionStatic);

/**
 * Private Static test
 */
string codePrivateStatic = @"
using System;
static class Run
{
    private static void Main()
    {
        Console.WriteLine(""Hello World Private & Static"");
    }
}
";
RunOption runOptionPrivateStatic = new RunOption() { IsStatic = true, NonPublic = true };
ScriptRunner.Run(codePrivateStatic, runOptionPrivateStatic);

/**
 * Delegate HelloWorld
 */
string codeDelegateHelloWorld = @"
using System;
public class Run
{
    public string DelegateHelloWorldFunc()
    {
        return ""Delegate Hello World"";
    }
}
";
var DelegateHelloWorldFunc = ScriptRunner.GenerateFunc<string>(codeDelegateHelloWorld, new RunOption() { MethodName = "DelegateHelloWorldFunc" });
Console.WriteLine(DelegateHelloWorldFunc(null));

/**
 * Delegate test
 */
string codeDelegate = @"
using System;
using System.Text;
public class Run
{
    public string DelegateTestFunc(int length, char c, int textIndex)
    {
        StringBuilder stringBuilder = new (new string('-', length));
        stringBuilder[textIndex] = c;
        return stringBuilder.ToString();
    }
}
";
var DelegateTestFunc = ScriptRunner.GenerateFunc<string>(codeDelegate, new RunOption() { MethodName = "DelegateTestFunc" });
Console.WriteLine(DelegateTestFunc(new object[] { 10, 't', 3 }));

/**
 * Delegate no generic test
 */
string codeDelegateNoGeneric = @"
using System;
using System.Text;
public class Run
{
    public string DelegateNoGenericTestFunc()
    {
        return ""Delegate no generic test"";
    }
}
";
var DelegateNoGenericTestFunc = ScriptRunner.GenerateFunc(codeDelegateNoGeneric, new RunOption() { MethodName = "DelegateNoGenericTestFunc" });
Console.WriteLine(DelegateNoGenericTestFunc(null));

/**
 * Delegate test
 */
string codeDelegateMultipleParams = @"
using System;
using System.Text;
public class Run
{
    public string CodeDelegateMultipleParamsTestFunc(int length, char c, int textIndex)
    {
        StringBuilder stringBuilder = new (new string('-', length));
        stringBuilder[textIndex] = c;
        return stringBuilder.ToString();
    }
}
";
var CodeDelegateMultipleParamsTestFunc = ScriptRunner.GenerateFunc<int, char, int, string>(codeDelegateMultipleParams, new RunOption() { MethodName = "CodeDelegateMultipleParamsTestFunc" });
Console.WriteLine(CodeDelegateMultipleParamsTestFunc(10, 't', 6));

/**
 * Delegate Static test
 */
string codeDelegateStatic = @"
using System;
public static class Run
{
    public static int count = 1;
    public static void DelegateStaticFunc()
    {
        Console.WriteLine(""Hello World Delegate Static: "" + count++);
    }
}
";
RunOption runOptionDelegateStatic = new RunOption() { IsStatic = true };
runOptionStatic.InstanceObject = ScriptRunner.GetInstanceObject(codeDelegateStatic, runOptionDelegateStatic);
runOptionStatic.MethodName = "DelegateStaticFunc";
var DelegateStaticFunc = ScriptRunner.GenerateFunc<object>(runOptionStatic);
DelegateStaticFunc(null);
DelegateStaticFunc(null);

/**
 * GenerateClassWithFunction test
 */
string codeGenerateClassWithFunction = @"
public int GenerateClassWithFunctionTestFunc()
{
    List<DiffRes> res = DiffTool.Diff(
        new List<string>() { ""11111111"", ""2222222"",  ""3333333"",  ""4444444"",  ""555"", ""666"", ""777"", ""888"", """", ""999"", ""99"", ""88"", ""77"" }, 
        new List<string>() { ""11111111"", ""22222222"", ""33333333"", ""44444444"",                             """", ""666"", ""99"", ""88"", ""77"" });
    List<GroupedDiffRes> groupedDiffRes = DiffTool.GetGroupedResult(res);
    return groupedDiffRes.Count;
}
";
RunOption generateClassWithFunctionOption = new RunOption();
generateClassWithFunctionOption.ExtraDllFileList = new List<string> { "Diff.dll" };
generateClassWithFunctionOption.MethodName = "GenerateClassWithFunctionTestFunc";
try
{
    string codeGeneratedClassWithFunction = ScriptRunner.GenerateClassWithFunction(codeGenerateClassWithFunction, generateClassWithFunctionOption);
    Console.WriteLine(ScriptRunner.Run(codeGeneratedClassWithFunction, generateClassWithFunctionOption));
}
catch
{
    Console.WriteLine("DLL NOT FOUND");
}