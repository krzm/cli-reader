namespace CLIReader;

public interface ICancelableReadLine
{
	public ReadData TryReadLine(ReadConfig config);
}