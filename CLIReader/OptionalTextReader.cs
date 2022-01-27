using CLIHelper;

namespace CLIReader;

public class OptionalTextReader : Reader<string>
{
	public OptionalTextReader(
		ICancelableReadLine input
		, IOutput output
		, IValidator textValidator) : base(input, output, textValidator)
	{
	}

	protected override string GetTypeToReturn(string text) => text;
}