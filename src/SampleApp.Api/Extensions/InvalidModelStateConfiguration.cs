namespace SampleApp.Api.Extensions
{
    public static class InvalidModelStateConfiguration
    {
        public static IMvcBuilder WithPreventAutoValidation(this IMvcBuilder builder)
        {
            return builder.ConfigureApiBehaviorOptions(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
        }
    }
}
