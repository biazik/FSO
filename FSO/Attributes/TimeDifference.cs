using System;
using System.ComponentModel.DataAnnotations;

namespace FSO.Attributes
{
    public class TimeDifference : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // Losowo wyrzucało potrzebę innego contextu przez ViewModel, więc jest if obsługujący oba
            if (validationContext.ObjectInstance is FSO.Models.Event eventModel)
            {
                var startDate = eventModel.Start;
                var endDate = eventModel.End;

                if (endDate < startDate)
                {
                    return new ValidationResult("Data końcowa nie może być wcześniejsza niż początkowa.");
                }

                var difference = endDate - startDate;
                if (difference.TotalDays > 7)
                {
                    return new ValidationResult("Różnica między datami nie może być większa niż tydzień.");
                }

                return ValidationResult.Success;
            }
            else if (validationContext.ObjectInstance is FSO.Models.EventView eventViewModel)
            {
                var startDate = eventViewModel.Event.Start;
                var endDate = eventViewModel.Event.End;

                if (endDate < startDate)
                {
                    return new ValidationResult("Data końcowa nie może być wcześniejsza niż początkowa.");
                }

                var difference = endDate - startDate;
                if (difference.TotalDays > 7)
                {
                    return new ValidationResult("Różnica między datami nie może być większa niż tydzień.");
                }

                return ValidationResult.Success;
            }

            return new ValidationResult("Nieobsługiwany typ obiektu walidacji.");
        }
    }
}
