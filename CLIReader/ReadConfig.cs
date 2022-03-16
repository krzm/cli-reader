namespace CLIReader;

public class ReadConfig
{
	public int? Max { get; init; }

	public string? Message { get; init; }

	public string? Format { get; init; }

	public string? DefaultValue { get; init; }

	public string? Instructions { get; init; }

	public ReadConfig(int max)
	{
		Max = max;
	}

	public ReadConfig(
		int max
		, string message) : this(max)
	{
		Message = message;
	}

	public ReadConfig(
		int max
		, string message
		, string format) : this(max, message)
	{
		Format = format;
	}
	
	public ReadConfig(
		int max
		, string message
		, string format
		, string defaultValue) : this(max, message, format)
	{
		DefaultValue = defaultValue;
	}

	public ReadConfig(
		int max
		, string message
		, string format
		, string defaultValue
		, string instructions) : this(max, message, format)
	{
		Instructions = instructions;
	}
}