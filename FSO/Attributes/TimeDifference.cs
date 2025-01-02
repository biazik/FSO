using System;
using System.ComponentModel.DataAnnotations;

namespace FSO.Attributes
{
    public class TimeDifference : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // Sprawdź, czy obiekt walidacji to FSO.Models.Event
            if (validationContext.ObjectInstance is FSO.Models.Event eventModel)
            {
                // Pobierz wartości Start i End
                var startDate = eventModel.Start;
                var endDate = eventModel.End;

                // Walidacja: Koniec wydarzenia nie może być wcześniej niż początek
                if (endDate < startDate)
                {
                    return new ValidationResult("Data końcowa nie może być wcześniejsza niż początkowa.");
                }

                // Walidacja: Maksymalna różnica między datami to 7 dni
                var difference = endDate - startDate;
                if (difference.TotalDays > 7)
                {
                    return new ValidationResult("Różnica między datami nie może być większa niż tydzień.");
                }

                return ValidationResult.Success;
            }
            // Sprawdź, czy obiekt walidacji to FSO.Models.EventView
            else if (validationContext.ObjectInstance is FSO.Models.EventView eventViewModel)
            {
                // Pobierz wartości Start i End z EventView
                var startDate = eventViewModel.Event.Start;
                var endDate = eventViewModel.Event.End;

                // Walidacja: Koniec wydarzenia nie może być wcześniej niż początek
                if (endDate < startDate)
                {
                    return new ValidationResult("Data końcowa nie może być wcześniejsza niż początkowa.");
                }

                // Walidacja: Maksymalna różnica między datami to 7 dni
                var difference = endDate - startDate;
                if (difference.TotalDays > 7)
                {
                    return new ValidationResult("Różnica między datami nie może być większa niż tydzień.");
                }

                return ValidationResult.Success;
            }

            // Jeśli obiekt nie jest żadnym z oczekiwanych typów, zwróć błąd
            return new ValidationResult("Nieobsługiwany typ obiektu walidacji.");
        }
    }
}
