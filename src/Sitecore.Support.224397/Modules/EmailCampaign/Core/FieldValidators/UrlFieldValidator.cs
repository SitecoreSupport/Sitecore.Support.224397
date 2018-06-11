namespace Sitecore.Support.Modules.EmailCampaign.Core.FieldValidators
{
  using Sitecore.Configuration;
  using Sitecore.Data.Validators;
  using Sitecore.Diagnostics;
  using Sitecore.Modules.EmailCampaign.Validators;
  using System;
  using System.Reflection;
  using System.Runtime.Serialization;

  [Serializable]
  public class UrlFieldValidator : StandardValidator
  {
    // Fields
    private RegexValidator _urlRegexValidator;

    // Methods
    public UrlFieldValidator()
    {
    }

    public UrlFieldValidator(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    protected override ValidatorResult Evaluate()
    {
      string controlValidationValue = base.ControlValidationValue;
      if (string.IsNullOrEmpty(controlValidationValue) || this.UrlRegexValidator.IsValid(controlValidationValue.Trim()))
      {
        return ValidatorResult.Valid;
      }
      string[] arguments = new string[] { controlValidationValue.Trim() };
      base.Text = this.GetText("'{0}' is not a valid url.", arguments);
      return base.GetFailedResult(ValidatorResult.CriticalError);
    }

    protected override ValidatorResult GetMaxValidatorResult() =>
      base.GetFailedResult(ValidatorResult.CriticalError);

    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      Assert.ArgumentNotNull(info, "info");
      base.GetObjectData(info, context);
      if (info.GetString("Name") == null)
      {
        info.AddValue("Name", this.Name);
      }
      else
      {
        info.GetType().GetMethod("UpdateValue", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(info, new object[] { "Name", this.Name, typeof(System.String) });
      }
    }

    // Properties
    public override string Name =>
      "UrlFieldValidator";

    private RegexValidator UrlRegexValidator =>
      (this._urlRegexValidator ?? (this._urlRegexValidator = Factory.CreateObject("urlRegexValidator", true) as RegexValidator));
  }
}