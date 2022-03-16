using CLIHelper;
using DIHelper.Unity;
using Unity;
using Unity.Injection;

namespace CLIReader;

public class AppInput 
    : UnityDependencySet
{
    public AppInput(
        IUnityContainer container)
        : base(container)
    {
    }

    public override void Register()
    {
        RegisterInput();
        RegisterInputValidators();
        RegisterInputReaders();
    }

    private void RegisterInput()
    {
        Container
            .RegisterSingleton<ICancelableReadLine, CancelableReadLine>()
            .RegisterSingleton<IInput, Input>();
    }

    private void RegisterInputValidators()
    {
        Container
            .RegisterSingleton<IValidator, TextRequiredValidator>(nameof(TextRequiredValidator))
            .RegisterSingleton<IValidator, TextOptionalValidator>(nameof(TextOptionalValidator))
            .RegisterSingleton<IValidator, DateTimeValidator>(nameof(DateTimeValidator));
    }

    private void RegisterInputReaders()
    {
        RegisterReader<RequiredTextReader, string>(
            nameof(RequiredTextReader)
            , nameof(TextRequiredValidator));
        RegisterReader<OptionalTextReader, string>(
            nameof(OptionalTextReader)
            , nameof(TextOptionalValidator));
        RegisterReader<RequiredDateTimeReader, DateTime>(
            nameof(RequiredDateTimeReader)
            , nameof(DateTimeValidator));
        RegisterReader<OptionalDateTimeReader, DateTime?>(
            nameof(OptionalDateTimeReader)
            , nameof(DateTimeValidator));
    }

    protected void RegisterReader<TReader, TType>(
        string name
        , string validatorName)
            where TReader : IReader<TType>
    {
        Container.RegisterSingleton<IReader<TType>, TReader>(
            name
            , new InjectionConstructor(
                new object[]
                {
                    Container.Resolve<ICancelableReadLine>()
                    , Container.Resolve<IOutput>()
                    , Container.Resolve<IValidator>(validatorName)
                }));
    }
}