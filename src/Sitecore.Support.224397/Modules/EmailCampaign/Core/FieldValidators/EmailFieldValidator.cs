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
  public class EmailFieldValidator : StandardValidator
  {
    // Fields
    private RegexValidator _emailRegexValidator;

    // Methods
    public EmailFieldValidator()
    {
    }

    public EmailFieldValidator(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    protected override ValidatorResult Evaluate()
    {
      string controlValidationValue = base.ControlValidationValue;
      if (string.IsNullOrEmpty(controlValidationValue) || this.EmailRegexValidator.IsValid(controlValidationValue.Trim()))
      {
        return ValidatorResult.Valid;
      }
      string[] arguments = new string[] { controlValidationValue.Trim() };
      base.Text = this.GetText("'{0}' is not a valid email address.", arguments);
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
      "EmailFieldValidator";

    private RegexValidator EmailRegexValidator =>
      (this._emailRegexValidator ?? (this._emailRegexValidator = Factory.CreateObject("emailRegexValidator", true) as RegexValidator));
  }
}