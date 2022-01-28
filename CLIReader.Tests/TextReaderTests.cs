using CLIHelper;
using Moq;
using Xunit;

namespace CLIReader.Tests;

public class TextReaderTests
{
	[Fact]
	public void Test_Read_Empty_Text_3_Times_And_Then_Valid_Text_With_RequiredTextReader()
	{
		var output = new Mock<IOutput>();
		var input = new Mock<ICancelableReadLine>();
		var validator = new Mock<IValidator>();
		var config = new ReadConfig(10, "Read x");
		var failedRead = new ReadData(true, "", config);
		var okRead = new ReadData(true, "abc", config);
		input
			.SetupSequence(m => m.TryReadLine(config))
			.Returns(failedRead)
			.Returns(failedRead)
			.Returns(failedRead)
			.Returns(okRead);
		validator
			.SetupSequence(m => m.Validate(It.IsAny<string>()))
			.Returns(false)
			.Returns(false)
			.Returns(false)
			.Returns(true);
		var textReader = new RequiredTextReader(input.Object, output.Object, validator.Object);

		var actual = textReader.Read(config);

		output.Verify(m => m.WriteLine("Read x Max:10"), Times.Once);
		output.Verify(m => m.WriteLine("Invalid input."), Times.Exactly(3));
		Assert.Equal("abc", actual);
	}
}