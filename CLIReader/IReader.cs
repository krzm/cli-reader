namespace CLIReader;

public interface IReader<TType>
{
	TType? Read(ReadConfig config);
}