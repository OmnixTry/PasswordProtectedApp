import { AbstractControl, ValidationErrors, ValidatorFn } from "@angular/forms";

export function upperCaseValidator(): ValidatorFn {
    return (control:AbstractControl) : ValidationErrors | null => {
        const value = control.value;

        const hasUpperCase = /[A-Z]+/.test(value);

        return !hasUpperCase ? { noUppercase: true}: null;
    }
}

export function lowerCaseValidator(): ValidatorFn {
    return (control:AbstractControl) : ValidationErrors | null => {
        const value = control.value;
        const hasLowerCase = /[a-z]+/.test(value);

        return !hasLowerCase ? { noLowercase: true}: null;
    }
}

export function numbersValidator(): ValidatorFn {
    return (control:AbstractControl) : ValidationErrors | null => {
        const value = control.value;

        const hasNumeric = /[0-9]+/.test(value);
        return !hasNumeric ? { noNumeric: true }: null;
    }
}

export function specialSymbolValidator(): ValidatorFn {
    return (control:AbstractControl) : ValidationErrors | null => {
        const value = control.value;

        const hasSpecialSymbols  = /[!@#$%^&*<>?]+/.test(value);

        return !hasSpecialSymbols ? { noSpecialSymbols: true}: null;
    }
}