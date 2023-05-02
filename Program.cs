using Sniff.Services;
using Sniff.Table;

namespace Sniff;

internal static class Program
{
    [Flags]
    private enum ArgumentType
    {
        Command = 0b0001,
        Option = 0b0010,
        ParameterName = 0b0100,
        Other = 0b1000
    }

    private enum ParameterName
    {
        Pattern,
        Path,
        None,
        Limit
    }

    private static readonly string[] Commands = { "sniff", "duplicates", "types" };

    private static ArgumentType _expectedArgument = ArgumentType.Command | ArgumentType.Option | ArgumentType.ParameterName | ArgumentType.Other;

    private static ParameterName _expectedParameter = ParameterName.Path;
    private static AbstractService _chosenService = new TypesService();
    private static bool _paged = false;             // TODO
    private static int? _resultsLimit = null;       // TODO

    /*
     *  sniff
     *  sniff -r
     *  sniff -r /home/alfonz/dev/java
     *  sniff -r -p --dir /home/alfonz/dev/java --pattern "*.j???"
     *  sniff --pattern "*.???"
     *
     *  sniff duplicates -r
     *  sniff sniff
     */

    private static void Main(string[] args)
    {
        for (var i = 0; i < args.Length; i++)
        {
            var arg = args[i];
            var argumentType = GetArgumentType(arg);
            if (!_expectedArgument.HasFlag(argumentType))
                throw new ArgumentException($"Invalid argument: \"{arg}\"");

            switch (argumentType)
            {
                case ArgumentType.Command:
                    ProcessCommand(arg);
                    continue;
                case ArgumentType.Option:
                    ProcessOptions(arg);
                    continue;
                case ArgumentType.ParameterName:
                    ProcessParameterName(arg);
                    _expectedArgument = ArgumentType.Other;
                    continue;
                case ArgumentType.Other:
                    ProcessOther(arg);
                    _expectedArgument = ArgumentType.Option | ArgumentType.ParameterName;
                    _expectedParameter = ParameterName.None;
                    break;
            }

            _expectedArgument &= ~ArgumentType.Command;
        }

        var basicInfoService = new BasicInfoService();
        TablePrinter.Print(basicInfoService.Search());
        Console.WriteLine();

        if (_paged)
        {
            throw new NotImplementedException();
        }
        else if (_resultsLimit != null)
            TablePrinter.Print(_chosenService.Search(), (int)_resultsLimit);
        else
            TablePrinter.Print(_chosenService.Search());
    }

    private static ArgumentType GetArgumentType(string arg)
    {
        if (arg.StartsWith("--"))
        {
            if (arg.Length < 3)
                throw new ArgumentException("Missing parameter");

            return ArgumentType.ParameterName;
        }

        if (arg.StartsWith("-"))
        {
            if (arg.Length < 2)
                throw new ArgumentException("Missing option");

            return ArgumentType.Option;
        }

        if (Commands.Contains(arg))
            return ArgumentType.Command;

        return ArgumentType.Other;
    }

    private static void ProcessCommand(string arg)
    {
        if (arg.Equals("sniff"))
        {
            // todo about
            Environment.Exit(0);
        }
        
        _chosenService = arg switch
        {
            "sniff" => throw new NotImplementedException(),
            "duplicates" => new DuplicatesService(),
            _ => _chosenService
        };
    }

    private static void ProcessOptions(string arg)
    {
        var options = arg.Substring(1);
        foreach (var option in options)
        {
            switch (option)
            {
                case 'r':
                    AbstractService.Recursive = true;
                    break;
                case 'p':
                    _paged = true;
                    break;
                default:
                    throw new ArgumentException($"Invalid switch \"{arg}\"");
            }
        }
    }

    private static void ProcessParameterName(string arg)
    {
        _expectedParameter = arg switch
        {
            "--path" => ParameterName.Path,
            "--pattern" => ParameterName.Pattern,
            "--limit" => ParameterName.Limit,
            _ => throw new ArgumentException($"Invalid parameter name \"{arg}\"")
        };
    }

    private static void ProcessOther(string arg)
    {
        switch (_expectedParameter)
        {
            case ParameterName.Pattern:
                AbstractService.SearchPattern = arg;
                break;
            case ParameterName.Path:
                AbstractService.BasePath = arg;
                break;
            case ParameterName.Limit:
                _resultsLimit = int.Parse(arg);
                break;
            case ParameterName.None:
                throw new ArgumentException($"Unexpected argument \"{arg}\"");
        }
    }
}