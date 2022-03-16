using System.Text;

namespace CLIReader;

/// <summary>
/// https://stackoverflow.com/questions/19734531/how-do-i-interrupt-not-using-thread-console-readline-when-a-key-is-pressed
/// </summary>
public class SimpleCancelableReadLine : ICancelableReadLine
{
	public ReadData TryReadLine(ReadConfig config)
	{
		var buf = new StringBuilder();
		for (; ; )
		{
			var key = Console.ReadKey(true);
			if (key.Key == ConsoleKey.Escape || key.Key == ConsoleKey.F4)
			{
				return new ReadData(false, "", config);
			}
			else if (key.Key == ConsoleKey.Enter)
			{
				Console.WriteLine();
				return new ReadData(true, buf.ToString(), config);
			}
			else if (key.Key == ConsoleKey.Backspace && buf.Length > 0)
			{
				buf.Remove(buf.Length - 1, 1);
				Console.Write("\b \b");
			}
			else if (key.KeyChar != 0 && buf.Length < config.Max)
			{
				buf.Append(key.KeyChar);
				Console.Write(key.KeyChar);
			}
		}
	}
}