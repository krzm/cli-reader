using CLIHelper;
using System.Text;

namespace CLIReader;

public abstract class Reader<TEntity> 
	: IReader<TEntity>
{
	private const string ValidationError = "Invalid input.";
	private const string ReaderExit = "Reader Escaped.";
	protected readonly ICancelableReadLine Input;
	protected readonly IOutput Output;

	private readonly IValidator textValidator;
	
	public Reader(
		ICancelableReadLine input
		, IOutput output
		, IValidator textValidator)
	{
		Input = input;
		Output = output;
		this.textValidator = textValidator;
	}

	public TEntity? Read(ReadConfig config)
	{
		Output.WriteLine(ProduceDesc(config));
		ReadData data;
		try
		{
			data = TryReadText(config);
			while (!textValidator.Validate(data.Line))
			{
				Output.WriteLine(ValidationError);
				data = TryReadText(config);
			}
		}
		catch (Exception ex)
		{
			if (ex.Message == ReaderExit)
			{
				return GetValueOnEscape();
			}
			else throw;
		}
		if (string.IsNullOrWhiteSpace(data.Line))
			return GetValueOnEnter();
		return GetTypeToReturn(data.Line);
	}

	protected virtual string ProduceDesc(ReadConfig config)
	{
		var sb = new StringBuilder();
		if (string.IsNullOrWhiteSpace(config.Message) == false)
			sb.Append(config.Message);
		if (string.IsNullOrWhiteSpace(config.Instructions) == false)
		{
			sb.Append(" ");
			sb.Append(config.Instructions);
		}
		if (string.IsNullOrWhiteSpace(config.Format) == false)
		{
			sb.Append(" ");
			sb.Append($"{nameof(ReadConfig.Format)}:{config.Format}");
		}
		sb.Append(" ");
		sb.Append($"Max:{config.Max}");
		return sb.ToString();
	}

	protected virtual ReadData TryReadText(ReadConfig config)
	{
		var data = Input.TryReadLine(config);
		if (data.Result == false) throw new Exception(ReaderExit);
		return data;
	}

	protected virtual TEntity? GetValueOnEscape()
	{
		return default;
	}

	protected virtual TEntity? GetValueOnEnter()
	{
		return default;
	}

	protected abstract TEntity GetTypeToReturn(string text);
}
