using CLIHelper;

namespace CLIReader;

public class RequiredDateTimeReader : Reader<DateTime>
{
	public RequiredDateTimeReader(
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

	protected override DateTime GetValueOnEnter() => DateTime.Now;

	protected override DateTime GetTypeToReturn(string text) => DateTime.Parse($"{text}:00");
}