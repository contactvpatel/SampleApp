using SampleApp.Api.Dto;
using FluentValidation;

namespace SampleApp.Api.Validators
{
    public class TaskCreateRequestValidator : AbstractValidator<TaskCreateRequest>
    {
        public TaskCreateRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is a required field.")
                .MaximumLength(100).WithMessage("Name shouldn't be more than 100 characters long.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is a required field.")
                .MaximumLength(250).WithMessage("Description shouldn't be more than 250 characters long.");

            RuleFor(x => x.DueDate)
                .NotNull().WithMessage("Due Date is a required field.") // Shouldn't be empty
                .Must(date => date != default)
                .WithMessage("Valid Due Date is required. Default Date value is not allowed.")
                .Must(Extensions.ValidatorExtension.DueDateShouldNotBeLessThanToday)
                .WithMessage("Due Date shouldn't be less than today.")
                .GreaterThanOrEqualTo(d => d.StartDate).WithMessage("Due Date shouldn't be less than Start date.")
                .GreaterThanOrEqualTo(d => d.EndDate).WithMessage("Due Date shouldn't be less than End date.");

            RuleFor(x => x.StartDate)
                .NotNull().WithMessage("Start Date is a required field.") // Shouldn't be empty
                .Must(date => date != default)
                .WithMessage("Valid Start Date is required. Default Date value is not allowed.")
                .LessThanOrEqualTo(d => d.EndDate).WithMessage("Start Date shouldn't be greater than End date.");

            RuleFor(x => x.EndDate)
                .NotNull().WithMessage("End Date is a required field.") // Shouldn't be empty
                .Must(date => date != default)
                .WithMessage("Valid End Date is required. Default Date value is not allowed.");

            RuleFor(x => x.Priority)
                .NotNull().WithMessage("Priority is a required field.")
                .NotEmpty().WithMessage("Priority is a required field.");

            RuleFor(x => x.Status)
                .NotNull().WithMessage("Status is a required field.")
                .NotEmpty().WithMessage("Status is a required field.");
        }
    }

    public class TaskUpdateRequestValidator : AbstractValidator<TaskUpdateRequest>
    {
        public TaskUpdateRequestValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id is a required field.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is a required field.")
                .MaximumLength(100).WithMessage("Name shouldn't be more than 100 characters long.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is a required field.")
                .MaximumLength(250).WithMessage("Description shouldn't be more than 250 characters long.");

            RuleFor(x => x.DueDate)
                .NotNull().WithMessage("Due Date is a required field.") // Shouldn't be empty
                .Must(date => date != default)
                .WithMessage("Valid Due Date is required. Default Date value is not allowed.")
                .Must(Extensions.ValidatorExtension.DueDateShouldNotBeLessThanToday)
                .WithMessage("Due Date shouldn't be less than today.")
                .GreaterThanOrEqualTo(d => d.StartDate).WithMessage("Due Date shouldn't be less than Start date.")
                .GreaterThanOrEqualTo(d => d.EndDate).WithMessage("Due Date shouldn't be less than End date.");

            RuleFor(x => x.StartDate)
                .NotNull().WithMessage("Start Date is a required field.") // Shouldn't be empty
                .Must(date => date != default)
                .WithMessage("Valid Start Date is required. Default Date value is not allowed.")
                .LessThanOrEqualTo(d => d.EndDate).WithMessage("Start Date shouldn't be greater than End date.");

            RuleFor(x => x.EndDate)
                .NotNull().WithMessage("End Date is a required field.") // Shouldn't be empty
                .Must(date => date != default)
                .WithMessage("Valid End Date is required. Default Date value is not allowed.");

            RuleFor(x => x.Priority)
                .NotNull().WithMessage("Priority is a required field.")
                .NotEmpty().WithMessage("Priority is a required field.");

            RuleFor(x => x.Status)
                .NotNull().WithMessage("Status is a required field.")
                .NotEmpty().WithMessage("Status is a required field.");
        }
    }
}