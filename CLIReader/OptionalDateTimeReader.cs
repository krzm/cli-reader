using CLIHelper;

namespace CLIReader;

public class OptionalDateTimeReader : Reader<DateTime?>
{
	public OptionalDateTimeReader(
		ICancelableReadLine input
		, IOutput output
		, IValidator textValidator) : base(input, output, textValidator)
	{
	}

	protected override DateTime? GetValueOnEscape() => default;

	protected override DateTime? GetValueOnEnter() => DateTime.Now;

	protected override DateTime? GetTypeToReturn(string text) => DateTime.Parse($"{text}:00");
}