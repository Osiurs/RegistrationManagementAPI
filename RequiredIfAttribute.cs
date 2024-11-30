using System;
using System.ComponentModel.DataAnnotations;

public class RequiredIfAttribute : ValidationAttribute
{
    private readonly string _dependentProperty;
    private readonly string _targetValue;

    public RequiredIfAttribute(string dependentProperty, string targetValue)
    {
        _dependentProperty = dependentProperty;
        _targetValue = targetValue;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        // Lấy thuộc tính phụ thuộc
        var property = validationContext.ObjectType.GetProperty(_dependentProperty);
        if (property == null)
        {
            return new ValidationResult($"Unknown property: {_dependentProperty}");
        }

        // Lấy giá trị của thuộc tính phụ thuộc
        var dependentValue = property.GetValue(validationContext.ObjectInstance)?.ToString();

        // Kiểm tra nếu Role khớp với targetValue và giá trị cần kiểm tra (value) là null
        if (dependentValue == _targetValue && value == null)
        {
            return new ValidationResult(ErrorMessage ?? $"{validationContext.DisplayName} is required.");
        }

        // Trả về thành công nếu điều kiện không thỏa mãn
        return ValidationResult.Success;
    }
}
