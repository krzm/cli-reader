namespace CLIReader;

public record ReadData(
	bool Result
	, string Line
	, ReadConfig Config);