using System.Text;

namespace CLIReader;

/// <summary>
/// https://stackoverflow.com/questions/31996519/listen-on-esc-while-reading-console-line
/// </summary>
public class CancelableReadLine : ICancelableReadLine
{
	private int clOffset;
	private readonly StringBuilder buffer = new();
	private ConsoleKeyInfo key;
	private ReadConfig? readConfig;

	public ReadData TryReadLine(ReadConfig config)
	{
		InitializeLoop(config);

		LoopOnKeys();

		return GetResult(config);
	}

	private void InitializeLoop(ReadConfig config)
	{
		readConfig = config;
		ArgumentNullException.ThrowIfNull(readConfig);
		clOffset = Console.CursorLeft;
		buffer.Clear();
		if (string.IsNullOrWhiteSpace(config.DefaultValue) == false)
		{
			foreach (var key in config.DefaultValue.ToCharArray())
			{
				OnChars(key);
			}
		}
		key = Console.ReadKey(true);
	}

	private void LoopOnKeys()
	{
		ArgumentNullException.ThrowIfNull(readConfig);
		while (key.Key != ConsoleKey.Enter && key.Key != ConsoleKey.Escape)
		{
			if (key.Key == ConsoleKey.Backspace && Console.CursorLeft - clOffset > 0)
			{
				OnBackspace();
				key = Console.ReadKey(true);
			}
			else if (key.Key == ConsoleKey.Delete && Console.CursorLeft - clOffset < buffer.Length)
			{
				OnDelete();
				key = Console.ReadKey(true);
			}
			else if ((char.IsLetterOrDigit(key.KeyChar) 
				|| char.IsWhiteSpace(key.KeyChar) 
				|| char.IsPunctuation(key.KeyChar)) 
				&& buffer.Length < readConfig.Max)
			{
				OnChars(key.KeyChar);
				key = Console.ReadKey(true);
			}
			else if (key.Key == ConsoleKey.LeftArrow && Console.CursorLeft - clOffset > 0)
			{
				Console.CursorLeft--;
				key = Console.ReadKey(true);
			}
			else if (key.Key == ConsoleKey.RightArrow && Console.CursorLeft - clOffset < buffer.Length)
			{
				Console.CursorLeft++;
				key = Console.ReadKey(true);
			}
			else
			{
				key = Console.ReadKey(true);
			}
		}
	}

	private void OnBackspace()
	{
		var cli = Console.CursorLeft - clOffset - 1;
		buffer.Remove(cli, 1);
		Console.CursorLeft = clOffset;
		Console.Write(new string(' ', buffer.Length + 1));
		Console.CursorLeft = clOffset;
		Console.Write(buffer.ToString());
		Console.CursorLeft = cli + clOffset;
	}

	private void OnDelete()
	{
		var cli = Console.CursorLeft - clOffset;
		buffer.Remove(cli, 1);
		Console.CursorLeft = clOffset;
		Console.Write(new string(' ', buffer.Length + 1));
		Console.CursorLeft = clOffset;
		Console.Write(buffer.ToString());
		Console.CursorLeft = cli + clOffset;
	}

	private void OnChars(char key)
	{
		var cli = Console.CursorLeft - clOffset;
		buffer.Insert(cli, key);
		Console.CursorLeft = clOffset;
		Console.Write(buffer.ToString());
		Console.CursorLeft = cli + clOffset + 1;
	}

	private ReadData GetResult(ReadConfig config)
	{
		if (key.Key == ConsoleKey.Enter)
		{
			Console.WriteLine();
			return new ReadData(true, buffer.ToString(), config);
		}

		return new ReadData(false, "", config);
	}
}