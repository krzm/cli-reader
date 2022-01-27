using CLIHelper;

namespace CLIReader;

public class RequiredTextReader : Reader<string>
{
	public RequiredTextReader(
		ICancelableReadLine input
		, IOutput output
		, IValidator textValidator) : base(input, output, textValidator)
	{
	}

	protected override ReadData TryReadText(ReadConfig config)
	{
		var data = Input.TryReadLine(config);
		if (data.Result == false) throw new Exception("Command Escaped.");
		return data;
	}

	protected override string GetTypeToReturn(string text) => text;
}