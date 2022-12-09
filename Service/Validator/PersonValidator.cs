using FluentValidation;
using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Validator
{
    public class PersonValidator : AbstractValidator<Person>
    {
        public PersonValidator()
        {
            RuleFor(x => x)
                .NotNull()
                .OnAnyFailure(y =>
                {
                    throw new ArgumentNullException("Can't found the object.");
                });

            //RuleFor(x => x.)
            //    .NotEmpty().WithMessage("Is necessary to inform the CPF.")
            //    .NotNull().WithMessage("Is necessary to inform the CPF.");

            //RuleFor(x => x.BirthDate)
            //    .NotEmpty().WithMessage("Is necessary to inform the birth date.")
            //    .NotNull().WithMessage("Is necessary to inform the birth date.");

            //RuleFor(x => x.Name)
            //    .NotEmpty().WithMessage("Is necessary to inform the name.")
            //    .NotNull().WithMessage("Is necessary to inform the name.");
        }
    }
}
