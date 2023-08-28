namespace SampleApp.Api.Extensions
{
    public static class ValidatorExtension
    {
        // Example showing how to use extension method inside fluent validation
        public static bool DueDateShouldNotBeLessThanToday(DateTime date)
        {
            return !(date.Date < DateTime.Now.Date);
        }
    }
}
